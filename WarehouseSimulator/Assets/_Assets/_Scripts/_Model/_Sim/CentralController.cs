using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class CentralController
    {
        private Dictionary<SimRobot, Stack<RobotDoing>> _plannedActions;

        private IPathPlanner _pathPlanner;
        
        private bool _isPreprocessDone;
        private bool _isPathPlanningDone;


        public bool IsPathPlanningDone
        {
            get => _isPathPlanningDone;
            private set => _isPathPlanningDone = value;
        }
        public bool IsPreprocessDone => _isPreprocessDone;
        
        public CentralController(Map map)
        {
            _pathPlanner = new BFS_PathPlanner(map);
            _plannedActions = new();
            _isPreprocessDone = false;
        }
        
        public void AddRobotToPlanner(SimRobot simRobot)
        {
            var q = new Stack<RobotDoing>();
            q.Push(RobotDoing.Wait);
            _plannedActions.Add(simRobot, q);
        }
        

        public void Preprocess(Map map)
        {
            //TODO: async?
            _isPreprocessDone = true;
        }

        public async void TimeToMove(Map map,SimRobotManager robieMan)
        {
            //TODO: abort planNextMoves if still in progress
            
            foreach (var (robot, actions) in _plannedActions)
            {
                if(actions.Count == 0) continue;
                var a = actions.Pop();
                robot.TryPerformActionRequested(a, map);
                robot.MakeStep(map);
            }
        }

        public async void PlanNextMovesForRobotAsync(Map map, SimRobot robot)
        {
            IsPathPlanningDone = false;
            await PlanNextMoves(map,robot);
            IsPathPlanningDone = true;
        }
        

        public async void PlanNextMovesForAllAsync(Map map)
        {
            IsPathPlanningDone = false;
            
            var robots = _plannedActions.Keys.ToList();
            var tasks = new List<Task>();
            
            
            //TODO: Make async
            foreach (var robot in robots)
            {
                tasks.Add(PlanNextMoves(map,robot));
            }

            Task t = Task.WhenAll(tasks);
            await t;
            
            IsPathPlanningDone = true;

        }

        private async Task PlanNextMoves(Map map,SimRobot robot)
        {
            //TODO: Calc how many waits we need for one request

            if (_plannedActions[robot] == null)
            {
                _plannedActions[robot] = new Stack<RobotDoing>();
            }
            
            if (robot.RobotData.m_state == RobotBeing.Free)
            {
                _plannedActions[robot].Push(RobotDoing.Wait);
            }
            else
            {
                _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading);
            }
        }

        
    }
}