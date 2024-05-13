using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// A* path planner with asynchronous path planning
    /// </summary>
    public class AStarAsync_PathPlanner : IPathPlanner
    {
        #region Fields
        private Map _map;
        private Dictionary<int, Stack<RobotDoing>> _cache;
        #endregion
        
        /// <summary>
        /// Constructor for the AStarAsync_PathPlanner
        /// </summary>
        /// <param name="map"></param>
        public AStarAsync_PathPlanner()
        {
            _cache = new();
        }

        #region  Methods
        
        /// <summary>
        /// Set map for path planner
        /// </summary>
        /// <param name="map"></param>
        public void SetMap(Map map)
        {
            _map = map;
        }

        /// <summary>
        /// Clear the cache of the path planner
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }
        
        /// <summary>
        /// Gets the next steps for the list of robots to take.
        /// </summary>
        /// <param name="robots"></param>
        /// <returns></returns>
        public Dictionary<SimRobot, RobotDoing> GetNextSteps(List<SimRobot> robots) 
        {
            foreach (var robot in robots)
            {
                if(!_cache.ContainsKey(robot.Id))
                    _cache.Add(robot.Id,new Stack<RobotDoing>());
            }
            
            List<Task<(int, Stack<RobotDoing>)>> tasks = new List<Task<(int, Stack<RobotDoing>)>>();
            Dictionary<SimRobot, RobotDoing> instructions = new();
            foreach (var robot in robots)
            {
                Task<(int, Stack<RobotDoing>)> task = null;
                if (robot.Goal != null)
                {
                    if(_cache[robot.Id].Count == 0)
                    {
                        task = Task.Run(() => (robot.Id, GetPath(robot.GridPosition, robot.Goal.GridPosition, robot.Heading)));
                        tasks.Add(task);
                    }
                }
            }
            
            Task.WaitAll(tasks.ToArray());
            
            foreach(var task in tasks)
            {
                var (id, stack) = task.Result;
                _cache[id] = stack;
            }
            
            foreach (var robot in robots)
            {
                try
                {
                    instructions.Add(robot, _cache[robot.Id].Pop());
                }
                catch (System.InvalidOperationException) //For when our GetPath can't find a path to the goal or no goal is set
                {
                    instructions.Add(robot, RobotDoing.Wait);
                }
                
            }

            return instructions;
        }
		
		
		
		/// <summary>
        /// Calculates the path for a robot to take from start to finish.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="facing"></param>
        /// <param name="disallowedPosition"></param>
        /// <returns>
        /// A stack of instructions for the robot to take. Top of the stack is the first instruction.
        /// </returns>
        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish, Direction facing)
        {
            Dictionary<
                    (Vector2Int, Direction)
                    ,
                    ((Vector2Int, Direction), RobotDoing, int,int)>
                pathDict = new();

            bool is_finish_found = false;

            MinHeap<int, (Vector2Int, Direction)> aStarQueue = new();
            Stack<RobotDoing> instructions = new();
            pathDict[(start, facing)] = ((start,facing), RobotDoing.Wait, 0, PathPlannerUtility.GetWeightFactor(0, facing, start, finish)); //Arbitrary value
            aStarQueue.Add(new Tuple<int, (Vector2Int, Direction)>(0, (start, facing)));

            var k = 0;
            (var currentNode, var currentDir) = (Vector2Int.zero, Direction.North);
            //Find finish
            while (aStarQueue.Count > 0)
            {
                (k, (currentNode, currentDir)) = aStarQueue.ExtractDominating();
                

                
                if (currentNode == finish)
                {
                    is_finish_found = true;
                    break;
                }
                int t = pathDict[(currentNode, currentDir)].Item3;
                foreach ((var node, var dir, var inst) in PathPlannerUtility.GetNeighbouringNodes(currentNode,
                             currentDir))
                {
                    var w = PathPlannerUtility.GetWeightFactor(t + 1, dir, node, finish);
                    bool b; //Logical value to check if the path is already trodden or has a lower weight than the ones already trodden
                    b = !pathDict.ContainsKey((node, dir));
                    b = b ? true : (pathDict[(node, dir)].Item4 > w && !b);
                    switch (inst)
                    {
                        case RobotDoing.Forward:

                            if (b) //Never move forward to an already trod path
                            {

                                if (_map.GetTileAt(node) == TileType.Wall)
                                {
                                    break; // Don't move into a wall
                                }
                                else
                                {
                                    pathDict[(node, dir)] = ((currentNode, currentDir), inst,t+1,w);
                                    aStarQueue.Add(new Tuple<int,(Vector2Int, Direction)>(w, (node, dir)));
                                }
                                
                            }
                            
                            break;
                        default:
                            
                            if (b) //Never turn more than it's needed AKA 4 times
                            {
                                
                                pathDict[(node, dir)] = ((currentNode, currentDir), inst,t+1,w);
                                aStarQueue.Add(new Tuple<int,(Vector2Int, Direction)>(w,(node, dir)));
                            }

                            break;
                    }
                }
            }
            
            if (!is_finish_found)
            {
                //Debug.Log("Couldn't find finish for robot.");
                return instructions; //We return an empty stack if we couldn't find the finish (which means it wasn't reachable for our robot)
                
            }

            currentNode = finish;
            
            //Traceback path
            while ((currentNode,currentDir) != (start,facing))
            {
                    
                instructions.Push(pathDict[(currentNode, currentDir)].Item2);
                (currentNode, currentDir) = pathDict[(currentNode, currentDir)].Item1;
            }
            
            return instructions;

        }
        
        #endregion
        
    }
}