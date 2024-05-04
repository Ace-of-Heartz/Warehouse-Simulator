using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model.Sim
{
    public class CoopAStar_PathPlanner : IPathPlanner
    {
        #region Fields
        Map _map;
        private Dictionary<int, Stack<RobotDoing>> _cache;
        #endregion
        public CoopAStar_PathPlanner()
        {
            _cache = new();
        }
        
        public void SetMap(Map map)
        {
            _map = map;
        }
        
        public Dictionary<SimRobot,RobotDoing> GetNextSteps(List<(SimRobot,bool)> robotsNDirt)
        {
            Dictionary<SimRobot,RobotDoing> instructions = new();
            foreach(var (robot,isDirty) in robotsNDirt)
            {
                if (isDirty)
                {
                    _cache[robot.Id] = GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading);
                }
                instructions.Add(robot,_cache[robot.Id].Pop());
            }

            return instructions;
        }
        
        public Stack<RobotDoing> GetPath(Vector2Int start, Vector2Int finish, Direction facing, Vector2Int? disallowedPosition = null)
        {
            throw new System.NotImplementedException();
        } 
        
    }
}