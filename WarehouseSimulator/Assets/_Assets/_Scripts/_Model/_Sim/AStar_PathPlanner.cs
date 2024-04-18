using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class AStar_PathPlanner : IPathPlanner
    {
        private Map m_map;
        
        public AStar_PathPlanner(Map map)
        {
            map = m_map;
        }
        
        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish,Direction direction)
        {
            Stack<RobotDoing> instructions = new();

            return instructions;
            
        }

        private Stack<RobotDoing> GetInstructions()
        {
            Dictionary<
                    (Vector2Int,Direction)
                    ,
                    ((Vector2Int,Direction),RobotDoing)> 
                pathDict;
            
            Queue<(Vector2Int,Direction)> dfsQueue;
            
            Stack<RobotDoing> instructions = new();
            pathDict = new();
            dfsQueue = new();

            return instructions;
        }

        private int GetWeightFactor(int costOfPath,Direction dir, Vector2Int inbetween, Vector2Int end)
        {
            return GetHeuristicEstimation(dir, inbetween, end) + costOfPath;
        }

        private int GetHeuristicEstimation(Direction dir,Vector2Int node, Vector2Int end)
        {
            int est = Mathf.Abs(node.x - end.x) - Mathf.Abs(node.y - end.y);
            est += GetRotationCost(dir,node,end);

            return est;
        }

        private int GetRotationCost(Direction currentDir, Vector2Int node, Vector2Int end)
        {
            int cost = 0;
            
            Vector2Int dirV = end - node;
            if (dirV.x != 0)
            {
                cost += 1;
            }

            if (dirV.y != 0)
            {
                cost += 1;
            }

            return cost;
        }
    }
}