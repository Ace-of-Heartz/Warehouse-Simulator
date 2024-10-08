﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// Cooperative A* path planner
    /// </summary>
    public class CoopAStar_PathPlanner : IPathPlanner
    {
        #region Fields
        Map _map;
        private Dictionary<int, Stack<RobotDoing>> _cache;
        private Dictionary<uint,(HashSet<uint>,HashSet<Vector2Int>)> _visitedState;
        
        #endregion
        public CoopAStar_PathPlanner()
        {
            _cache = new();
            _visitedState = new();
        }
        
        /// <summary>
        /// Sets the map for the path planner
        /// </summary>
        /// <param name="map"></param>
        public void SetMap(Map map)
        {
            _map = map;
        }
        /// <summary>
        /// Gets the next steps for the list of robots to take. Each robot receives only the next step to take.
        /// </summary>
        /// <param name="robots"></param>
        /// <returns>
        /// A stack of instructions for the robot to take. Top of the stack is the first instruction.
        /// </returns>
        public Dictionary<SimRobot,RobotDoing> GetNextSteps(List<SimRobot> robots)
        {
            Dictionary<SimRobot,RobotDoing> instructions = new();
            foreach(var robot in robots)
            {
                if(robot.Goal != null)
                {
                    if (!_cache.ContainsKey(robot.Id))
                    {
                        _cache.Add(robot.Id, GetPath(robot.GridPosition, robot.Goal.GridPosition, robot.Heading));
                    }
                
                    if (! (_cache[robot.Id].Count > 0))
                    {
                        _cache[robot.Id] = GetPath(robot.GridPosition, robot.Goal.GridPosition, robot.Heading);
                    } 
                    
                    try
                    {
                        instructions.Add(robot,_cache[robot.Id].Pop());
                        
                    } 
                    catch (System.InvalidOperationException) //For when our GetPath can't find a path to the goal
                    {
                        instructions.Add(robot,RobotDoing.Wait);
                    }
                    
                }
                else
                {
                    instructions.Add(robot,RobotDoing.Wait);
                }
            }

            return instructions;
        }
        
        /// <summary>
        /// Actual path planning algorithm. Returns a stack of instructions for the robot to take.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="facing"></param>
        /// <param name="disallowedPosition"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
                return instructions; //Could not find finish -> don't do anything
            }

            currentNode = finish;
            
            //Traceback path
            while ((currentNode,currentDir) != (start,facing))
            {
                //TODO: Make this save the occupied tiles by the robot
                instructions.Push(pathDict[(currentNode, currentDir)].Item2);
                (currentNode, currentDir) = pathDict[(currentNode, currentDir)].Item1;
            }
            
            return instructions;
        }

        private uint GetNextHashset(uint id)
        {
            bool needNewLayer = true;
            uint res = 0;
            
            foreach(var k in _visitedState.Keys)
            {
                if (!_visitedState[k].Item1.Contains(id))
                {
                    needNewLayer = false;
                    res = k;
                    
                }
            }
            
            if (needNewLayer)
            {
                res = _visitedState.Keys.ToList().Max() + 1;
                _visitedState.Add(res,(new(),new()));
            }

            return res;
        }
        
    }
}