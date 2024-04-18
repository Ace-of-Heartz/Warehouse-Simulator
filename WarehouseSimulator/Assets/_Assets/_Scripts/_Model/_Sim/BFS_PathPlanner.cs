using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model.Sim
{
    public class BFS_PathPlanner : IPathPlanner
    {
        #region Fields
        private Map m_map;
        #endregion
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public BFS_PathPlanner(Map map)
        {
            m_map = map;
        } 
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish, Direction dir)
        {
            var instructions = GetInstructions(start, finish, dir);
            return instructions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="facing"></param>
        /// <returns></returns>
        private Stack<RobotDoing> GetInstructions(Vector2Int start, Vector2Int finish, Direction facing)
        {
            Dictionary<
                (Vector2Int,Direction)
                ,
                ((Vector2Int,Direction),RobotDoing)> 
                pathDict;
            
            Queue<(Vector2Int,Direction)> dfsQueue;
            
            bool is_finish_found = false;
            
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
                    is_finish_found = true;
                    break;
                }
                foreach((var node,var dir,var inst) in GetNeighbouringNodes(currentNode,currentDir))
                {

                    switch (inst)
                    {
                        case RobotDoing.Forward:
                            if (!pathDict.Keys.ToList().Exists(p => p.Item1 == node )) //Never move forward to an already trod path
                            {

                                if (m_map.GetTileAt(node) == TileType.Wall)
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

            if (!is_finish_found)
            {
                Debug.Log("Couldn't find finish for robot.");
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
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="facing"></param>
        /// <returns></returns>
        private IEnumerable<(Vector2Int, Direction,RobotDoing)> GetNeighbouringNodes(Vector2Int currentNode, Direction facing)
        {
            (Vector2Int forwardNode,Direction leftNode,Direction rightNode)  = GetNextNodes(currentNode, facing);
            yield return (forwardNode, facing,RobotDoing.Forward);
            yield return (currentNode, leftNode,RobotDoing.Rotate90);
            yield return (currentNode, rightNode,RobotDoing.RotateNeg90);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="facing"></param>
        /// <returns></returns>
        private (Vector2Int,Direction, Direction) GetNextNodes(Vector2Int currentNode, Direction facing)
        {
            Vector2Int forwardNode;
            Direction leftNode, rightNode;
            
            switch (facing)
            {
                case Direction.North:
                    forwardNode = currentNode + Vector2Int.down;
                    leftNode    = Direction.West;
                    rightNode   = Direction.East;
                    break;
                case Direction.South:
                    forwardNode = currentNode + Vector2Int.up;
                    leftNode    = Direction.East;
                    rightNode   = Direction.West;
                    break;    
                case Direction.East:
                    forwardNode = currentNode + Vector2Int.right;
                    leftNode    = Direction.North;
                    rightNode   = Direction.South;
                    break;
                case Direction.West:
                    forwardNode = currentNode + Vector2Int.left;
                    leftNode    = Direction.South;
                    rightNode   = Direction.North;
                    break;
                default:
                    forwardNode = currentNode;
                    leftNode    = facing;
                    rightNode   = facing;
                    break;
            }

            return (forwardNode,leftNode,rightNode);
        }
        




    }
}