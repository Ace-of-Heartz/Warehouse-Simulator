using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class BFS_PathPlanner : IPathPlanner
    {
        #region Fields
        private readonly Map _map;
        #endregion
        
        
        /// <summary>
        /// Constructor of BFS_PathPlanner class
        /// </summary>
        /// <param name="map">Map loaded in from config file</param>
        public BFS_PathPlanner(Map map)
        {
            _map = map;
        } 
            
        /// <summary>
        /// Gets the shortest path from a starting position to a finish position, with the initial direction in mind.
        /// Can check additionally check for occupied tiles instead of only walls.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="dir"></param>
        /// <param name="checkForRobots"></param>
        /// <returns></returns>
        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish, Direction dir, bool checkForRobots = false)
        {
            return GetInstructions(start, finish, dir, checkForRobots);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="facing"></param>
        /// <param name="checkForRobots"></param>
        /// <returns></returns>
        private Stack<RobotDoing> GetInstructions(Vector2Int start, Vector2Int finish, Direction facing,bool checkForRobots)
        {
            Dictionary<
                (Vector2Int,Direction)
                ,
                ((Vector2Int,Direction),RobotDoing)> 
                pathDict;
            
            Queue<(Vector2Int,Direction)> dfsQueue;
            
            bool isFinishFound = false;
            
            Stack<RobotDoing> instructions = new();
            pathDict = new();
            dfsQueue = new();

            pathDict[(start, facing)] = ((Vector2Int.zero, Direction.North), RobotDoing.Wait); //Arbitrary value
            dfsQueue.Enqueue((start,facing));
            

            (var currentNode,var currentDir) = (Vector2Int.zero, Direction.North);
            //Find finish
            while (dfsQueue.Count > 0)
            {
                (currentNode,currentDir) = dfsQueue.Dequeue();
                if (currentNode == finish)
                {
                    isFinishFound = true;
                    break;
                }
                foreach((var node,var dir,var inst) in (this as IPathPlanner).GetNeighbouringNodes(currentNode,currentDir))
                {

                    switch (inst)
                    {
                        case RobotDoing.Forward:
                            if (!pathDict.Keys.ToList().Exists(p => p.Item1 == node )) //Never move forward to an already trod path
                            {

                                if (_map.GetTileAt(node) == TileType.Wall ||  (checkForRobots && _map.GetTileAt(node) == TileType.RoboOccupied)  )
                                {
                                    break; // Don't move into a wall
                                }
                                else
                                {
                                    pathDict[(node, dir)] = ((currentNode, currentDir),inst);
                                    dfsQueue.Enqueue((node, dir));
                                }
                                
                            }

                            break;
                        default:
                            if (!pathDict.ContainsKey((node, dir))) //Never turn more than it's needed AKA 4 times
                            {
                                pathDict[(node, dir)] = ((currentNode, currentDir),inst);
                                dfsQueue.Enqueue((node, dir));
                            }

                            break;
                    }
                    
                    
                }
            }

            if (!isFinishFound)
            {
                return instructions; //Could not find finish -> don't do anything
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


        




    }
}