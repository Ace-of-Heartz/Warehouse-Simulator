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
        private List<SimRobot> _allRobots;

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
            RobotAddedEvent?.Invoke(this, new(newR));
        }
    
        public void AssignTasksToFreeRobots(SimGoalManager from)
        {
            if (from is null)
            {
                throw new ArgumentException("Error in robot managing! The GoalManager doesn't exist!");
            }
            
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
        public void RoboRead(string from, Map mapie)
        {
            using StreamReader rid = new(from);
            if (!int.TryParse(rid.ReadLine(), out int robn))
            {
                throw new InvalidFileException("Invalid file format: First line not a number");
            }

            int nextid = 0;
            for (int i = 0; i < robn; i++)
            {   
                string? line = rid.ReadLine();
                if (line == null)
                {
                    throw new InvalidFileException("Invalid file format: there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidFileException($"Invalid file format: {nextid + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidFileException($"Invalid file format: {nextid + 2}. line does not provide a valid position");
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
        public async Task<(bool,SimRobot?,SimRobot?)> CheckValidSteps((SimRobot robie, RobotDoing action)[] actions,Map mapie)
        {
            if (actions.Length != _allRobots.Count)
            {
                throw new ArgumentException($"Error in checking valid steps, the number of robots ({actions.Length}) given actions does not equal the number of all robots {_allRobots.Count}");
            }
            // foreach ((SimRobot robie,RobotDoing what) in actions)
            // {
            //     if (!robie.TryPerformActionRequested(what, mapie)) return false; //TODO => Blaaa: Async?
            // }
            
            var tasks = actions.Select(async tuple => await Task.FromResult(tuple.robie.TryPerformActionRequested(tuple.action, mapie)));
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
                var positionCheckTasks = _allRobots.Select(async thisrob => await Task.FromResult(CheckingFuturePositions(thisrob,robie)));
                (bool good, SimRobot? whoCrashed)[]? maybeMistakes = await Task.WhenAll(positionCheckTasks);
                hitter = null;
                try
                {
                    hitter = maybeMistakes.First(r => r.good == false).whoCrashed;
                }
                catch (Exception) { /*ignored because this means there were no problems in the operations so far*/ }

                if (hitter != null) return (false, robie, hitter);

                // foreach (SimRobot compRobie in _allRobots)
                // {
                //     if (robie.RobotData.m_id == compRobie.RobotData.m_id) continue; //if it's the same robot, we skip the step
                //     
                //     if (compRobie.NextPos == robie.NextPos) return (false,compRobie,robie); 
                //     //we check whether there are matching future positions, because this would mean that the step is invalid
                //
                //     if (compRobie.NextPos == robie.RobotData.m_gridPosition
                //         & robie.NextPos == compRobie.RobotData.m_gridPosition) return (false,compRobie,robie);
                //     //we check whether they want to step in each other's places ("jump over each other") because this would mean the step is invalid
                //     
                //     //TODO => Nincs más, amit meg kéne nézni?
                // }
            }
            
            return (true,null,null);
        }

        private (bool,SimRobot?) CheckingFuturePositions(SimRobot thisOne, SimRobot notThisOne)
        {
            if (thisOne.RobotData.m_id == notThisOne.RobotData.m_id) return (true,null); //if it's the same robot, we skip the step

            if (notThisOne.NextPos == thisOne.NextPos) return (false,thisOne);//(false,notThisOne,thisOne); 
            //we check whether there are matching future positions, because this would mean that the step is invalid

            if (notThisOne.NextPos == thisOne.RobotData.m_gridPosition
                & thisOne.NextPos == notThisOne.RobotData.m_gridPosition) return (false,thisOne); //(false,notThisOne,thisOne);
            //we check whether they want to step in each other's places ("jump over each other") because this would mean the step is invalid
            //TODO => Nincs más, amit meg kéne nézni?
            return (true,null);
        }
    }
}
