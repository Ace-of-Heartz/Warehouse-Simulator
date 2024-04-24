using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model.Sim
{
    public class CoopAStar_PathPlanner : IPathPlanner
    {
        Map m_map;
        
        public CoopAStar_PathPlanner(Map map)
        {
            m_map = map;
        }
        
        
        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish, Direction dir, Vector2Int? disallowedPosition = null)
        {
            throw new System.NotImplementedException();
        }   
    }
}