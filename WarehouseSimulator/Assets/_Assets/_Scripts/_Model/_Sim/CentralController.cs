#nullable enable
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
        private HashSet<SimRobot> _failedRobot;

        private IPathPlanner _pathPlanner;

        private Task? _taskBeforeNextStep;
        
        
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
            _failedRobot = new();
        }
        
        public void AddPathPlanner(IPathPlanner pathPlanner)
        {
            _pathPlanner = pathPlanner;
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
            _isPreprocessDone = true;
        }
        
        /// <summary>
        /// Tries moving all robots according to their precalculated instructions.
        /// </summary>
        /// <param name="map">Map loaded in from config file</param>
        /// <param name="robieMan">SimRobotManager</param>
        public async void TimeToMove(Map map,SimRobotManager robieMan)
        {
            if (!(IsPathPlanningDone || IsPreprocessDone))
            {
                Debug.Log("Processes not finished until next step. Timeout sent.");
                
                foreach (var (_,actions) in _plannedActions)
                {
                    actions.Push(RobotDoing.Timeout);
                }
            }
            else
            {
                foreach (var (_, actions) in _plannedActions)
                {
                    if (actions.Count == 0) actions.Push(RobotDoing.Wait);
                }
            }

            (bool success, SimRobot? robieTheFirst, SimRobot? robieTheSecond) results = await robieMan.CheckValidSteps(_plannedActions,map);
            if (!results.success)
            {
                foreach (var e in _plannedActions)
                {
                    CustomLog.Instance.AddRobotAction(e.Key.Id,RobotDoing.Wait);
                }
                
                _failedRobot.Add(results.robieTheFirst!);
                if (results.robieTheSecond is not null) _failedRobot.Add(results.robieTheSecond);
                //TODO: Add Wait to one of the robots
            }
            else
            {
                 foreach (SimRobot robie in _plannedActions.Keys)
                 {
                     robie.MakeStep(map);
                 }
            }
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
                _taskBeforeNextStep!.Wait();
            }
            
            IsPathPlanningDone = false;
            
            var robots = _plannedActions.Keys.ToList();
            var tasks = new List<Task>();

            foreach (var robot in robots)
            {
                if(_plannedActions[robot].Count == 0)
                    tasks.Add(PlanNextMoves(robot));
                else if (_failedRobot.Contains(robot))
                {
                    ++i;
                    tasks.Add(PlanNextMoves(robot,true));
                    _failedRobot.Remove(robot);
                    
                }
            }

            _taskBeforeNextStep = Task.WhenAll(tasks);
            await _taskBeforeNextStep;
            
            IsPathPlanningDone = true;

        }
        /// <summary>
        /// Plans a list of instructions for an individual robot with(out) taking the position of other robots into consideration. How rebellious.
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="avoidRobots">Whether to take the position of other robots into consideration</param>
        private async Task PlanNextMoves(SimRobot robot, bool avoidRobots = false)
        {
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
                _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading, avoidRobots);
            }
        }
        
    }
}