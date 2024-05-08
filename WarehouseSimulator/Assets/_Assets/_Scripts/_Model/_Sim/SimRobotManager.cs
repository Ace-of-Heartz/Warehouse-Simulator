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
        /// The list of all the robots
        /// </summary>
        protected List<SimRobot> _allRobots;
        
        /// <summary>
        /// The number of robots in the simulation
        /// </summary>
        public int RobotCount => _allRobots.Count;

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
            _allRobots = new();
        }
        
        /// <summary>
        /// Adds a new robot to the simulation
        /// </summary>
        /// <param name="id">The id of the robot</param>
        /// <param name="pos">The start position of the robot</param>
        private void AddRobot(int id, Vector2Int pos)
        {
            SimRobot newR = new(id, pos);
            _allRobots.Add(newR);
            CustomLog.Instance.AddRobotStart(id, pos.x, pos.y, Direction.North);
            RobotAddedEvent?.Invoke(this, new(newR));
        }
    
        /// <summary>
        /// Assigns tasks to the free robots
        /// </summary>
        /// <param name="from">The <see cref="SimGoalManager"/> that the where the goals come from</param>
        public void AssignTasksToFreeRobots(SimGoalManager from)
        {
            foreach (var robie in _allRobots)
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
            
            int nextid = 0;
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
                AddRobot(nextid,nextRobPos);
                
                nextid++;
            }
        }

        /// <summary>
        /// Checks if the given steps are valid for the robots
        /// </summary>
        /// <param name="actions">Array of (robot,action) tuples</param>
        /// <param name="mapie">The map</param>
        /// <returns>
        /// A ValueTask which has a value of a (Errors, SimRobot?, SimRobot?) tuple where the Erros value means what type of error might have happened
        /// The first robot value represents the first robot included in the possible invalid step (if the step is valid, this will be null)
        /// The second robot value represents the second robot included in the possible invalid step (if only one robot was included, or the step is valid, this will be null)
        /// </returns>
        /// <exception cref="ArgumentException">Is thrown when the length of the actions array isn't valid</exception>
        ///<example>
        ///     <code>
        ///         (Error happened, SimRobot? robieTheFirst, SimRobot? robieTheSecond) results = await robieMan.CheckValidSteps(_plannedActions,map);
        ///         if (results.happened != Errors.None)
        ///         {
        ///             //replan with the robies
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
        public async Task<(Error,SimRobot?,SimRobot?)> CheckValidSteps(Dictionary<SimRobot, Stack<RobotDoing>> actions,Map mapie)
        {
            if (actions.Count != _allRobots.Count)
            {
                throw new ArgumentException($"Error in checking valid steps, the number of robots ({actions.Count}) given actions does not equal the number of all robots {_allRobots.Count}");
            }
            
            //var tasks = actions.Select(async pair => await Task.FromResult(pair.Key.TryPerformActionRequested(pair.Value.Pop(),mapie)));
            var tasks = actions.Select(pair => Task.FromResult(pair.Key.TryPerformActionRequested(pair.Value.Peek(),mapie)));
            (bool success, SimRobot? whoTripped)[]? results = await Task.WhenAll(tasks);

            (Error happened, SimRobot? hitter) error = (Error.None, null);
            try
            {
                error.hitter = results.First(r => r.success == false).whoTripped;
            }
            catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

            if (error.hitter != null) return (Error.RAN_INTO_WALL, error.hitter, null);
            
            foreach (SimRobot robie in actions.Keys)
            {
                //var positionCheckTasks = _allRobots.Select(async thisrob => await Task.FromResult(CheckingFuturePositions(thisrob,robie)));
                var positionCheckTasks = actions.Keys.Select(thisrob => Task.FromResult(CheckingFuturePositions(thisrob,robie)));
                (Error error, SimRobot? whoCrashed)[]? maybeMistakes = await Task.WhenAll(positionCheckTasks);
                error = (Error.None, null);
                try
                {
                    error = maybeMistakes.First(r => r.error != Error.None);
                }
                catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

                if (error.hitter != null)
                {
                    CustomLog.Instance.AddError(robie.Id,error.hitter.Id);
                    return (error.happened, robie, error.hitter);
                }
            }

            foreach (var dicc in actions)
            {
                dicc.Value.Pop();
            }
            
            return (Error.None,null,null);
        }

        /// <summary>
        /// Check if the robots are gonna do something illegal
        /// </summary>
        /// <param name="firstRobot">The robot to check</param>
        /// <param name="secondRobot">The other robot to check</param>
        /// <returns>Tuple:
        /// - Error: The type of error that might have happened
        /// - SimRobot?: null if the Error is None. The value of <paramref name="firstRobot"/> otherwise
        /// </returns>
        /// <remarks>Wall collisions are ignored in this method</remarks>
        private (Error,SimRobot?) CheckingFuturePositions(SimRobot firstRobot, SimRobot secondRobot)
        {
            if (firstRobot.RobotData.m_id == secondRobot.RobotData.m_id) return (Error.None,null); 
            //if it's the same robot, we skip the step

            if (secondRobot.NextPos == firstRobot.NextPos)
            {
                if (secondRobot.NextPos == secondRobot.GridPosition || firstRobot.NextPos == firstRobot.GridPosition)
                {
                    if(secondRobot.State == RobotBeing.Free || firstRobot.State == RobotBeing.Free)
                        return (Error.RAN_INTO_PASSIVE_ROBOT, firstRobot);
                    
                    return (Error.RAN_INTO_ACTIVE_ROBOT, firstRobot);
                }
                return (Error.RAN_INTO_FIELD_OCCUPATION_CONFLICT, firstRobot);
            }
            //we check whether there are matching future positions, because this would mean that the step is invalid

            if (secondRobot.NextPos == firstRobot.RobotData.m_gridPosition
                & firstRobot.NextPos == secondRobot.RobotData.m_gridPosition) return (Error.TRIED_SWAPPING_PLACES,firstRobot); 
            //we check whether they want to step in each other's places ("jump over each other") because this would mean the step is invalid
            
            return (Error.None,null);
        }
    }
}
