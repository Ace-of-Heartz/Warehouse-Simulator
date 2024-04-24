using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class AStar_PathPlanner : IPathPlanner
    {
        private Map m_map;

        public AStar_PathPlanner(Map map)
        {
            m_map = map;
        }

        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish, Direction direction, int x, int y)
        {
            Stack<RobotDoing> instructions = GetInstructions(start,finish,direction,x,y);
        
            return instructions;

        }

        private Stack<RobotDoing> GetInstructions(Vector2Int start, Vector2Int finish, Direction facing,
            int x, int y)
        {
            Dictionary<
                    (Vector2Int, Direction)
                    ,
                    ((Vector2Int, Direction), RobotDoing, int,int)>
                pathDict = new();

            bool is_finish_found = false;

            MinHeap<int, (Vector2Int, Direction)> aStarQueue = new();
            Stack<RobotDoing> instructions = new();
            pathDict[(start, facing)] = ((start,facing), RobotDoing.Wait, 0, GetWeightFactor(0, facing, start, finish)); //Arbitrary value
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
                foreach ((var node, var dir, var inst) in (this as IPathPlanner).GetNeighbouringNodes(currentNode,
                             currentDir))
                {
                    var w = GetWeightFactor(t + 1, dir, node, finish);
                    bool b; //Logical value to check if the path is already trodden or has a lower weight than the ones already trodden
                    b = !pathDict.ContainsKey((node, dir));
                    b = b ? true : (pathDict[(node, dir)].Item4 > w && !b);
                    switch (inst)
                    {
                        case RobotDoing.Forward:

                            if (b) //Never move forward to an already trod path
                            {

                                if (m_map.GetTileAt(node) == TileType.Wall || (x != -1 && y != -1 && node == new Vector2Int(x,y)))
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
                    
                instructions.Push(pathDict[(currentNode, currentDir)].Item2);
                (currentNode, currentDir) = pathDict[(currentNode, currentDir)].Item1;
            }
            
            return instructions;
        }

        /// <summary>
        /// Calculates the weight of a ndoe depending on the cost of the path already and a heuristic estimation of the upcoming path. 
        /// </summary>
        /// <param name="costOfPath"></param>
        /// <param name="dir"></param>
        /// <param name="inbetween"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int GetWeightFactor(int costOfPath, Direction dir, Vector2Int inbetween, Vector2Int end)
        {
            return GetHeuristicEstimation(dir, inbetween, end) + costOfPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="node"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int GetHeuristicEstimation(Direction dir, Vector2Int node, Vector2Int end)
        {
            int est = Mathf.Abs(node.x - end.x) + Mathf.Abs(node.y - end.y);
            est += GetRotationCost(dir, node, end);

            return est;
        }

        /// <summary>
        /// Get the heuristic cost of rotations.
        /// </summary>
        /// <param name="currentDir"></param>
        /// <param name="node"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int GetRotationCost(Direction currentDir, Vector2Int node, Vector2Int end)
        {
            int cost = 0;

            Vector2Int dirV = end - node;
            if (dirV.x > 0)
            {
                cost += GetWeightOfDirection(1, 1, 0, 2, currentDir);
            }
            else if (dirV.x < 0)
            {
                cost += GetWeightOfDirection(1, 1, 2, 0, currentDir);
            }

            if (dirV.y > 0)
            {
                cost += GetWeightOfDirection(2, 0, 1, 1, currentDir);
            } 
            else if (dirV.y < 0)
            {
                cost += GetWeightOfDirection(0, 2, 1, 1, currentDir);
            } 

            return Mathf.Min(cost,2);
        }
        /// <summary>
        /// Calculates the cost of a certain direction depending on the given current direction.
        /// </summary>
        /// <param name="northWeight"></param>
        /// <param name="southWeight"></param>
        /// <param name="eastWeight"></param>
        /// <param name="westWeight"></param>
        /// <param name="currentDir"></param>
        /// <returns></returns>
        private int GetWeightOfDirection(int northWeight, int southWeight, int eastWeight, int westWeight, Direction currentDir)
        {
            int cost = 0;
            switch (currentDir)
            {
                case Direction.North:
                    cost = northWeight;
                    break;
                case Direction.South:
                    cost = southWeight;
                    break;
                case Direction.East:
                    cost = eastWeight;
                    break;
                case Direction.West:
                    cost = westWeight;
                    break;
            }

            return cost;
        }

    }
    

    
    
}