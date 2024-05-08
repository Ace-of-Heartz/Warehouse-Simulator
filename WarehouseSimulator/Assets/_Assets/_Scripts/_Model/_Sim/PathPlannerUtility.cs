using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public static class PathPlannerUtility
    {
         /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="facing"></param>
        /// <returns></returns>
        public static IEnumerable<(Vector2Int, Direction,RobotDoing)> GetNeighbouringNodes(Vector2Int currentNode, Direction facing)
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
        public static (Vector2Int,Direction, Direction) GetNextNodes(Vector2Int currentNode, Direction facing)
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
        
                /// <summary>
        /// Calculates the weight of a ndoe depending on the cost of the path already and a heuristic estimation of the upcoming path. 
        /// </summary>
        /// <param name="costOfPath"></param>
        /// <param name="dir"></param>
        /// <param name="inbetween"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetWeightFactor(int costOfPath, Direction dir, Vector2Int inbetween, Vector2Int end)
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
        private static int GetHeuristicEstimation(Direction dir, Vector2Int node, Vector2Int end)
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
        private static int GetRotationCost(Direction currentDir, Vector2Int node, Vector2Int end)
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
        private static int GetWeightOfDirection(int northWeight, int southWeight, int eastWeight, int westWeight, Direction currentDir)
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