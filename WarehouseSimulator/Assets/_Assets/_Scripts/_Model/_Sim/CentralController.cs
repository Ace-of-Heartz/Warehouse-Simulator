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
        
        #region Fields
        private Dictionary<SimRobot, Stack<RobotDoing>> _plannedActions;

        private IPathPlanner _pathPlanner;

        [CanBeNull] private Task _taskBeforeNextStep;
        
        
        private bool _isPreprocessDone;
        private bool _isPathPlanningDone;
        #endregion

        /// <summary>
        /// Get returns true if all paths have been calculated for the robots, else false
        /// Set is private
        /// </summary>
        public bool IsPathPlanningDone
        {
            get => _isPathPlanningDone;
            private set => _isPathPlanningDone = value;
        }
        public bool IsPreprocessDone => _isPreprocessDone;
        
        /// <summary>
        /// Constructor of CentralController 
        /// </summary>
        /// <param name="map">Map loaded in from config file</param>
        public CentralController(Map map)
        {
            _pathPlanner = new AStar_PathPlanner(map);
            _plannedActions = new();
            _isPreprocessDone = false;
        }
        
        /// <summary>
        /// Adds robot to dictionary of CentralController.
        /// Initializes the robot's planned moves with one Wait instruction.
        /// </summary>
        /// <param name="simRobot">Simulation robot model</param>
        public void AddRobotToPlanner(SimRobot simRobot)
        {
            var q = new Stack<RobotDoing>();
            q.Push(RobotDoing.Wait);
            _plannedActions.Add(simRobot, q);
        }
        
        /// <summary>
        /// Preprocess
        /// </summary>
        /// <param name="map"></param>
        public void Preprocess(Map map)
        {
            //TODO: async?
            _isPreprocessDone = true;
        }

        /// <summary>
        /// Tries moving all robots according to their precalculated instructions.
        /// </summary>
        /// <param name="map">Map loaded in from config file</param>
        public void TimeToMove(Map map)
        {
            if (!(IsPathPlanningDone || IsPreprocessDone))
            {
                Debug.Log("Processes not finished until next step. Timeout sent.");
                //_taskBeforeNextStep.Dispose();
                //TODO: Send Timeout
            }
            foreach (var (robot, actions) in _plannedActions)
            {
                if(actions.Count == 0) continue;
                var a = actions.Pop();
                robot.TryPerformActionRequested(a, map);
                robot.MakeStep(map);
            }
        }
        
        /// <summary>
        /// Plan the move instruction for a single robot.
        /// Async method.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="robot"></param>
        public async void PlanNextMovesForRobotAsync(Map map, SimRobot robot)
        {
            if ((_taskBeforeNextStep == null ? TaskStatus.WaitingToRun :  _taskBeforeNextStep.Status) == TaskStatus.Running)
            {
                _taskBeforeNextStep.Wait();
            }
            
            IsPathPlanningDone = false;
            _taskBeforeNextStep = PlanNextMoves(map, robot);
            IsPathPlanningDone = true;
        }
        
        /// <summary>
        /// Plan the move instructions for all robots present in the simulation.
        /// Async method.
        /// </summary>
        /// <param name="map">Map loaded from config file</param>
        public async void PlanNextMovesForAllAsync(Map map)
        {
            if ((_taskBeforeNextStep == null ? TaskStatus.WaitingToRun :  _taskBeforeNextStep.Status) == TaskStatus.Running)
            {
                _taskBeforeNextStep.Wait();
            }
            
            
            IsPathPlanningDone = false;
            
            var robots = _plannedActions.Keys.ToList();
            var tasks = new List<Task>();
            
            
            //TODO: Make async
            foreach (var robot in robots)
            {
                tasks.Add(PlanNextMoves(map,robot));
            }

            _taskBeforeNextStep = Task.WhenAll(tasks);
            await _taskBeforeNextStep;
            
            IsPathPlanningDone = true;

        }
        /// <summary>
        /// Plans a list of instructions for an individual robot without taking the position of other robots into consideration. How rebellious.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="robot"></param>
        private async Task PlanNextMoves(Map map,SimRobot robot)
        {
            //TODO: Calc how many waits we need for one request

            if (_plannedActions[robot] == null)
            {
                _plannedActions[robot] = new Stack<RobotDoing>();
            }
            
            if (robot.Goal == null)
            {
                _plannedActions[robot].Push(RobotDoing.Wait);
            }
            else
            {
                _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading,false);
            }
        }
        /// <summary>
        /// Plans a list of instructions for an individual robot while also considering the positions of other robots. How polite.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="robot"></param>
        private async Task PlanReroute(Map map, SimRobot robot)
        {
            if (_plannedActions[robot] == null)
            {
                _plannedActions[robot] = new Stack<RobotDoing>();
            }
            
            if (robot.Goal == null)
            {
                _plannedActions[robot].Push(RobotDoing.Wait);
            }
            else
            {
                _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading,true);
            }
        }

        
    }
}