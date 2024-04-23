#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
        
        private void AddRobot(int i, Vector2Int pos)
        {
            SimRobot newR = new(i, pos);
            _allRobots.Add(newR);
            CustomLog.Instance.AddRobotStart(i, pos.x, pos.y, Direction.North);
            RobotAddedEvent?.Invoke(this, new(newR));
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
        /// A ValueTask which has a value of a (bool, SimRobot?, SimRobot?) tuple where the bool value represents if the Steps are valid
        /// The first robot value represents the first robot included in the possible invalid step (if the step is valid, this will be null)
        /// The second robot value represents the second robot included in the possible invalid step (if only one robot was included, or the step is valid, this will be null)
        /// </returns>
        /// <exception cref="ArgumentException">Is thrown when the length of the actions array isn't valid</exception>
        ///<example>
        ///     <code>
        ///         (bool success, SimRobot? robieTheFirst, SimRobot? robieTheSecond) results = await robieMan.CheckValidSteps(_plannedActions,map);
        ///         if (!results.success)
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
        public async Task<(bool,SimRobot?,SimRobot?)> CheckValidSteps(Dictionary<SimRobot, Stack<RobotDoing>> actions,Map mapie)
        {
            if (actions.Count != _allRobots.Count)
            {
                throw new ArgumentException($"Error in checking valid steps, the number of robots ({actions.Count}) given actions does not equal the number of all robots {_allRobots.Count}");
            }
            
            //var tasks = actions.Select(async pair => await Task.FromResult(pair.Key.TryPerformActionRequested(pair.Value.Pop(),mapie)));
            var tasks = actions.Select(pair => Task.FromResult(pair.Key.TryPerformActionRequested(pair.Value.Pop(),mapie)));
            (bool success, SimRobot? whoTripped)[]? results = await Task.WhenAll(tasks);

            SimRobot? hitter = null;
            try
            {
                hitter = results.First(r => r.success == false).whoTripped;
            }
            catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

            if (hitter != null) return (false, hitter, null);
            
            foreach (SimRobot robie in _allRobots)
            {
                //var positionCheckTasks = _allRobots.Select(async thisrob => await Task.FromResult(CheckingFuturePositions(thisrob,robie)));
                var positionCheckTasks = _allRobots.Select(thisrob => Task.FromResult(CheckingFuturePositions(thisrob,robie)));
                (bool good, SimRobot? whoCrashed)[]? maybeMistakes = await Task.WhenAll(positionCheckTasks);
                hitter = null;
                try
                {
                    hitter = maybeMistakes.First(r => r.good == false).whoCrashed;
                }
                catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

                if (hitter != null)
                {
                    CustomLog.Instance.AddError(robie.Id,hitter.Id);
                    return (false, robie, hitter);
                }
            }
            
            return (true,null,null);
        }

        private (bool,SimRobot?) CheckingFuturePositions(SimRobot thisOne, SimRobot notThisOne)
        {
            if (thisOne.RobotData.m_id == notThisOne.RobotData.m_id) return (true,null); //if it's the same robot, we skip the step

            if (notThisOne.NextPos == thisOne.NextPos) return (false,thisOne);
            //we check whether there are matching future positions, because this would mean that the step is invalid

            if (notThisOne.NextPos == thisOne.RobotData.m_gridPosition
                & thisOne.NextPos == notThisOne.RobotData.m_gridPosition) return (false,thisOne); 
            //we check whether they want to step in each other's places ("jump over each other") because this would mean the step is invalid
            
            return (true,null);
        }
    }
}
