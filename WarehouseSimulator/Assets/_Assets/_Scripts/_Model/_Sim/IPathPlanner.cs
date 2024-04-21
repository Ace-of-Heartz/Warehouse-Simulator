using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using Vector2 = System.Numerics.Vector2;

namespace WarehouseSimulator.Model.Sim
{
    public interface IPathPlanner
    {
        // ReSharper restore Unity.ExpensiveCode
        Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish,Direction dir, bool checkForRobots);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="facing"></param>
        /// <returns></returns>
        IEnumerable<(Vector2Int, Direction,RobotDoing)> GetNeighbouringNodes(Vector2Int currentNode, Direction facing)
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
        (Vector2Int,Direction, Direction) GetNextNodes(Vector2Int currentNode, Direction facing)
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