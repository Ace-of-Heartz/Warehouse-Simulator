using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class AStar_PathPlanner : IPathPlanner
    {
        public async Task<List<RobotDoing>> GetPath(Vector2Int start, Vector2Int finish)
        {
            List<RobotDoing> instructions = new();

            return instructions;
            
        }
    }
}