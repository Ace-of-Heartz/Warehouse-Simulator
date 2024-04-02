using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class CentralController
    {
        private Dictionary<Robot, RobotDoing> plannedActions;
        
        private bool isPreprocessDone;
        public bool IsPreprocessDone => isPreprocessDone;
        
        public CentralController()
        {
            plannedActions = new Dictionary<Robot, RobotDoing>();
            isPreprocessDone = false;
        }
        
        public void AddRobotToPlanner(Robot robot)
        {
            plannedActions.Add(robot, RobotDoing.Wait);
        }
        

        public void Preprocess(Map map)
        {
            //TODO: async?
            Debug.Log("Preprocessing in central controller");
            isPreprocessDone = true;
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
            Debug.Log("Planning next moves");
            var robots = plannedActions.Keys.ToList();
            foreach (var robot in robots)
            {
                plannedActions[robot] = RobotDoing.Timeout;
            }
            
            //TODO: make async
            // random moves for now
            foreach (var robot in robots)
            {
                plannedActions[robot] = (RobotDoing) Random.Range(0, 4);
            }
        }
    }
}