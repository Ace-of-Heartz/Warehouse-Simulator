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
    public class SimRobotManager
    {
        protected List<SimRobot> _allRobots;
        
        public int RobotCount => _allRobots.Count;

        public event EventHandler<RobotCreatedEventArgs>? RobotAddedEvent;
        public event EventHandler<GoalAssignedEventArgs>? GoalAssignedEvent;

        public SimRobotManager()
        {
            _allRobots = new();
        }
        
        protected void AddRobot(SimRobot robie)
        {
            _allRobots.Add(robie);
            CustomLog.Instance.AddRobotStart(robie.Id, robie.GridPosition.x, robie.GridPosition.y, Direction.North);
            RobotAddedEvent?.Invoke(this, new(robie));
        }
    
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
                SimRobot robie = new SimRobot(nextid,nextRobPos);
                AddRobot(robie);
                
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
            if (actions.Count != _allRobots.Count)
            {
                throw new ArgumentException($"Error in checking valid steps, the number of robots ({actions.Count}) given actions does not equal the number of all robots {_allRobots.Count}");
            }
            
            var tasks = actions.Select(pair => Task.FromResult(pair.Key.TryPerformActionRequested(pair.Value,mapie)));
            (bool success, SimRobot? whoTripped)[]? results = await Task.WhenAll(tasks);

            (bool happened, SimRobot? hitter) error = (false, null);
            try
            {
                error.hitter = results.First(r => r.success == false).whoTripped;
            }
            catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

            if (error.hitter != null) return false;

            foreach (SimRobot robie in _allRobots)
            {
                var positionCheckTasks = 
                    _allRobots.Select(thisrob => Task.Run( () => CheckingFuturePositions(thisrob,robie)));
                (bool errorHappened, SimRobot? whoCrashed)[]? maybeMistakes = await Task.WhenAll(positionCheckTasks);
                error = (false, null);
                try
                {
                    error = maybeMistakes.First(r => r.errorHappened == true);
                }
                catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

                if (error.hitter != null)
                {
                    CustomLog.Instance.AddError(robie.Id,error.hitter.Id);
                    return false;
                }
            }
            return true;
        }

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
