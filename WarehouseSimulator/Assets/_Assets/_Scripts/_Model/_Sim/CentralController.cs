using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class CentralController
    {
        private Dictionary<Robot, List<RobotDoing>> plannedActions;

        private IPathPlanner m_pathPlanner;
        
        private bool isPreprocessDone;
        public bool IsPreprocessDone => isPreprocessDone;
        
        public CentralController(Map map)
        {
            m_pathPlanner = new BFS_PathPlanner(map);
            plannedActions = new();
            isPreprocessDone = false;
        }
        
        public void AddRobotToPlanner(Robot robot)
        {
            plannedActions.Add(robot, new List<RobotDoing>(){RobotDoing.Wait});
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
                foreach (var action in actions)
                {
                    robot.PerformActionRequested(action, map);
                }
            }
        }

        public void PlanNextMoves(Map map)
        {
            var robots = plannedActions.Keys.ToList();
            foreach (var robot in robots)
            {
                plannedActions[robot] = new List<RobotDoing>{RobotDoing.Timeout};
                
                
            }

            List<RobotDoing> plannedActionsForRobot;
            foreach (var robot in robots)
            {
                Debug.Log(robot.Heading);
                //Robot starting point
                Debug.Log(robot.GridPosition);
                //Robot ending point
                Debug.Log(robot.Goal.GridPosition);
                //Robot planned actions
                plannedActionsForRobot = m_pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading).Result;
                plannedActions[robot] = plannedActionsForRobot;

                foreach (var a in plannedActionsForRobot)
                {
                    switch (a)
                    {
                        case RobotDoing.Forward:
                            Debug.Log("Moving forward.");
                            break;
                        case RobotDoing.Rotate90:
                            Debug.Log("Rotating clockwise.");
                            break;
                        case RobotDoing.RotateNeg90:
                            Debug.Log("Rotating counter-clockwise.");
                            break;
                        default:
                            Debug.Log("Doing something sus.");
                            break;
                    }
                }
                
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