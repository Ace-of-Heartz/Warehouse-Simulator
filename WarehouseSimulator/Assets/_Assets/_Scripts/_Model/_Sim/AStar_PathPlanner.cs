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
    }
}