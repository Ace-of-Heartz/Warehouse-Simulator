using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class CentralController
    {
        private Dictionary<SimRobot, Stack<RobotDoing>> plannedActions;

        private IPathPlanner m_pathPlanner;
        
        private bool isPreprocessDone;
        public bool IsPreprocessDone => isPreprocessDone;
        
        public CentralController(Map map)
        {
            m_pathPlanner = new BFS_PathPlanner(map);
            plannedActions = new();
            isPreprocessDone = false;
        }
        
        public void AddRobotToPlanner(SimRobot simRobot)
        {
            var q = new Stack<RobotDoing>();
            q.Push(RobotDoing.Wait);
            plannedActions.Add(simRobot, q);
        }
        

        public void Preprocess(Map map)
        {
            //TODO: async?
            isPreprocessDone = true;
        }

        public void TimeToMove(Map map)
        {
            //TODO: abort planNextMoves if still in progress
            foreach (var (robot, actions) in plannedActions)
            {
                if(actions.Count == 0) continue;
                var a = actions.Pop();
                robot.TryPerformActionRequestedAsync(a, map);
                robot.MakeStep(map);
            }
        }

        public void PlanNextMoves(Map map)
        {
            var robots = plannedActions.Keys.ToList();
            foreach (var robot in robots)
            {
                var q = new Stack<RobotDoing>();
                q.Push(RobotDoing.Timeout);
                plannedActions[robot] = q;
            }

            Stack<RobotDoing> plannedActionsForRobot;
            foreach (var robot in robots)
            {
                if (robot.Goal == null)
                    continue;

                plannedActionsForRobot = m_pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading);
                plannedActions[robot] = plannedActionsForRobot;



            }
            
            
            
            //TODO: make async
            // random moves for now
            // foreach (var robot in robots)
            // {
            //     plannedActions[robot] = (RobotDoing) new Random().Next(0, 4);
            // }
        }
    }
}