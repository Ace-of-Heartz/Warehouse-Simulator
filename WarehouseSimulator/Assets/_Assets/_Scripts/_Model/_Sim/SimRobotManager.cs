#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// Manages all the robots in the simulation
    /// </summary>
    public class SimRobotManager
    {
        /// <summary>
        /// The array of all the robots
        /// </summary>
        protected SimRobot[] AllRobots;
        
        /// <summary>
        /// The number of robots in the simulation
        /// </summary>
        public int RobotCount => AllRobots.Length;

        /// <summary>
        /// Invoked when a robot is added to the simulation
        /// </summary>
        public event EventHandler<RobotCreatedEventArgs>? RobotAddedEvent;
        /// <summary>
        /// Invoked when a goal is assigned to a robot
        /// </summary>
        public event EventHandler<GoalAssignedEventArgs>? GoalAssignedEvent;

        /// <summary>
        /// Constructor for the SimRobotManager class. Yes it is redundant. Yes it works. Yes this summary is necessary. And yes, have a good day.
        /// </summary>
        public SimRobotManager()
        {
            AllRobots = new SimRobot[10];
        }
        
        /// <summary>
        /// Adds a robot to the simulation
        /// </summary>
        /// <param name="robie">The robot to add</param>
        /// <param name="idx">The index where to add the robot</param>
        protected void AddRobot(SimRobot robie, int idx)
        {
            AllRobots[idx] = robie;
            CustomLog.Instance.AddRobotStart(robie.Id, robie.GridPosition.x, robie.GridPosition.y, Direction.North);
            RobotAddedEvent?.Invoke(this, new(robie));
        }
    
        /// <summary>
        /// Assigns tasks to the free robots
        /// </summary>
        /// <param name="from">The <see cref="SimGoalManager"/> that the where the goals come from</param>
        public void AssignTasksToFreeRobots(SimGoalManager from)
        {
            foreach (var robie in AllRobots)
            {
                if (robie.State == RobotBeing.Free)
                {
                    SimGoal? next = from.GetNext();
                    if (next == null) { break; }
                    robie.AssignGoal(next);
                    GoalAssignedEvent?.Invoke(this, new(next));
                }
            }
        }
        
        /// <summary>
        /// Loads the robots from a file
        /// </summary>
        /// <param name="from">The path of the file</param>
        /// <param name="mapie">The map where the robots are to be added</param>
        /// <param name="robotN">The number of robots requested</param>
        /// <exception cref="InvalidFileException">Thrown if the file is incorrect</exception>
        public void RoboRead(string from, Map mapie, int robotN)
        {
            using StreamReader rid = new(from);
            if (!int.TryParse(rid.ReadLine(), out int robn))
            {
                throw new InvalidFileException("Invalid .agents file format:\n First line not a number");
            }
            
            if (robn != robotN)
            {
                throw new InvalidFileException($"Invalid .agents file format:\n The number of robots given in the Configuration File ({robotN}) does not equal the number of robots given in the Agents File ({robn})");
            }

            if (robn < 0)
            {
                throw new InvalidFileException($"Invalid .agents file format:\n The number of agents (currently: {robn}) cannot be less than zero!");
            }

            AllRobots = new SimRobot[robn];
            int nextid = 0; //same as next position in the array
            for (int i = 0; i < robn; i++)
            {   
                string? line = rid.ReadLine();
                if (line == null)
                {
                    throw new InvalidFileException("Invalid .agents file format:\n there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidFileException($"Invalid .agents file format:\n {nextid + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidFileException($"Invalid .agents file format:\n {nextid + 2}. line does not provide a valid position");
                }
                
                Vector2Int nextRobPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                mapie.OccupyTile(nextRobPos);
                SimRobot robie = new SimRobot(nextid,nextRobPos);
                AddRobot(robie,nextid);
                
                nextid++;
            }
        }

        /// <summary>
        /// Checks if the given steps are valid for the robots
        /// </summary>
        /// <param name="actions">Dictionary of (robot,action) key-value pairs</param>
        /// <param name="mapie">The map</param>
        /// <returns>
        /// A Task which has a bool value, that is true if the step is valid, and false if it isn't
        /// </returns>
        /// <exception cref="ArgumentException">Is thrown when the length of the actions array isn't valid</exception>
        ///<example>
        ///     <code>
        ///         bool isValidStep = await robieMan.CheckValidSteps(_plannedActions,map);
        ///         if (!isValidStep)
        ///         {
        ///             //replan with the robies if necessary
        ///         }
        ///         else
        ///         {
        ///              foreach (SimRobot robie in _plannedActions.Keys)
        ///              {
        ///                  robie.MakeStep(map);
        ///              }
        ///         }
        ///     </code>
        ///</example>
        public async Task<bool> CheckValidSteps(Dictionary<SimRobot, RobotDoing> actions,Map mapie)
        {
            bool hasErrorHappened = false;
            
            if (actions.Count != AllRobots.Length)
            {
                throw new ArgumentException($"Error in checking valid steps, the number of robots ({actions.Count}) given actions does not equal the number of all robots {AllRobots.Length}");
            }
            
            var tasks = actions.Select(pair => Task.Run(() => pair.Key.TryPerformActionRequested(pair.Value,mapie)));
            (bool success, SimRobot? whoTripped)[] results = await Task.WhenAll(tasks);
            if(results.Any(r => r.success == false))
            {
                hasErrorHappened = true;
            }

            for (int i = 0; i < AllRobots.Length; ++i)
            {
                SimRobot robie = AllRobots[i];
                int numberOfRemainingRobies = AllRobots.Length - (i+1);
                SimRobot[] compareRobs = new SimRobot[numberOfRemainingRobies];
                Array.Copy(AllRobots,i+1,compareRobs,0,numberOfRemainingRobies);
                var positionCheckTasks = 
                    compareRobs.Select(thisrob => Task.Run( () => CheckingFuturePositions(thisrob,robie)));
                (bool errorHappened, SimRobot? whoCrashed)[]? maybeMistakes = await Task.WhenAll(positionCheckTasks);
                if (maybeMistakes is not null)
                {
                    foreach ((bool isOk, SimRobot? whoCrashed) in maybeMistakes)
                    {
                        if (!isOk)
                        {
                            hasErrorHappened = true;
                            var id = whoCrashed!.Id;
                            CustomLog.Instance.AddError(robie.Id, id);
                        }
                    }
                }
            }
            return hasErrorHappened;
        }

        /// <summary>
        /// Check if the robots are gonna do something illegal
        /// </summary>
        /// <param name="firstRobie">The robot to check</param>
        /// <param name="secondRobie">The other robot to check</param>
        /// <returns>Tuple:
        /// - bool: if they are doing everything right
        /// - SimRobot: <paramref name="firstRobie"/> if bool is false, null if true
        /// </returns>
        /// <remarks>Wall collisions are ignored in this method</remarks>
        private (bool,SimRobot?) CheckingFuturePositions(SimRobot firstRobie, SimRobot secondRobie)
        {
            if (firstRobie.RobotData.m_id == secondRobie.RobotData.m_id) return (true,null); 
            //if it's the same robot, we skip the step

            if (secondRobie.NextPos == firstRobie.NextPos)
            {
                if (secondRobie.NextPos == secondRobie.GridPosition || firstRobie.NextPos == firstRobie.GridPosition)
                {
                    if(secondRobie.State == RobotBeing.Free || firstRobie.State == RobotBeing.Free)
                        return (false, firstRobie);
                    
                    return (false, firstRobie);
                }
                return (false, firstRobie);
            }
            //we check whether there are matching future positions, because this would mean that the step is invalid

            if (secondRobie.NextPos == firstRobie.RobotData.m_gridPosition
                & firstRobie.NextPos == secondRobie.RobotData.m_gridPosition) return (false,firstRobie); 
            //we check whether they want to step in each other's places ("jump over each other") because this would mean the step is invalid
            
            return (true,null);
        }
    }
}
