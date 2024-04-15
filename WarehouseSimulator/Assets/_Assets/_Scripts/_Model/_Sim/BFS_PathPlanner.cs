using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model.Sim
{
    public class BFS_PathPlanner : IPathPlanner
    {
        private Map m_map;

        private Queue<Vector2Int> m_queue;
        
        public BFS_PathPlanner(Map map)
        {
            m_map = map;
        } 
            
        public async Task<List<RobotDoing>> GetPath(Vector2Int start, Vector2Int finish, Direction dir)
        {

            return GetInstructions(start,finish,dir);
        }

        private List<RobotDoing> GetInstructions(Vector2Int start, Vector2Int finish, Direction dir)
        {
            List<RobotDoing> instructions = new();


            return instructions;
        }

        //private Enumerable<Vector2Int> GetNeighbouringNode()
        //{
        //    
        //} 
        
        
        
    }
}