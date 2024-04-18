using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
                robot.TryPerformActionRequested(a, map);
                robot.MakeStep(map);
            }
        }
        

        public void PlanNextMovesForAll(Map map)
        {
            var robots = plannedActions.Keys.ToList();
            
            //TODO: Make async
            foreach (var robot in robots)
            {
                PlanNextMoves(map,robot);
            }
            
        }

        public void PlanNextMoves(Map map,SimRobot robot)
        {
            //TODO: Calc how many waits we need for one request

            if (plannedActions[robot] == null)
            {
                plannedActions[robot] = new Stack<RobotDoing>();
            }
            
            if (robot.Goal == null)
            {
                plannedActions[robot].Push(RobotDoing.Wait);
            }
            else
            {
                plannedActions[robot] = m_pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading);
            }
        }

        
    }
}