using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class CentralController
    {
        private Dictionary<Robot, RobotDoing> plannedActions;
        
        public CentralController()
        {
            plannedActions = new Dictionary<Robot, RobotDoing>();
        }
        
        public void AddRobotToPlanner(Robot robot)
        {
            plannedActions.Add(robot, RobotDoing.Wait);
        }
        

        public void Preprocess(Map map)
        {
            //TODO: async?
            Debug.Log("Preprocessing in central controller");
        }

        public void TimeToMove(Map map)
        {
            //TODO: abort planNextMoves if still in progress
            Debug.Log("Submitting moves");
            foreach (var (robot, action) in plannedActions)
            {
                robot.PerformActionRequested(action, map);
            }
        }

        public void PlanNextMoves(Map map)
        {
            foreach (var (robot, action) in plannedActions)
            {
                plannedActions[robot] = RobotDoing.Timeout;
            }
            
            //TODO: make async
            // random moves for now
            foreach (var (robot, _) in plannedActions)
            {
                plannedActions[robot] = (RobotDoing) Random.Range(0, 4);
            }
        }
    }
}