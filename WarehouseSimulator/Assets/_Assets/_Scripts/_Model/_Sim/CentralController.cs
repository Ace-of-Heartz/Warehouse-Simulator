#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// The brain of the simulation. It supervises all robot movements
    /// </summary>
    public class CentralController
    {
        
        #region Fields
        private Dictionary<SimRobot, RobotDoing> _plannedActions;

        private IPathPlanner? _pathPlanner;

        private Task<Dictionary<SimRobot, RobotDoing>>? _taskBeforeNextStep;
        
        
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
        public CentralController()
        {
            _plannedActions = new();
            _isPreprocessDone = false;
        }
        
        /// <summary>
        /// Assign a path planner to be used for path planning
        /// </summary>
        /// <param name="pathPlanner">The custom planner</param>
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
            _plannedActions.Add(simRobot, RobotDoing.Wait);
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
        public async void TimeToMove(SimRobotManager robieMan,Map map)
        {
            if (!IsPathPlanningDone || !IsPreprocessDone)
            { 
                foreach (var robot in _plannedActions.Keys.ToList())
                {
                    _plannedActions[robot] = RobotDoing.Timeout;
                }
            }
            bool isNotValidStep = await robieMan.CheckValidSteps(_plannedActions,map);
            
            if (isNotValidStep)
            {
                foreach (var e in _plannedActions)
                {
                    CustomLog.Instance.AddRobotAction(e.Key.Id,RobotDoing.Wait);
                }
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
        public async Task PlanNextMovesForAllAsync()
        {
            if (_taskBeforeNextStep != null)
            {
                if (!_taskBeforeNextStep.IsCompleted)
                {
                    _taskBeforeNextStep.Wait();
                }
            }
            IsPathPlanningDone = false;

            var robots = _plannedActions.Keys.ToList();
            float startTime = UnityEngine.Time.time;
            _taskBeforeNextStep = Task.Run(() => Task.FromResult(_pathPlanner!.GetNextSteps(robots)));
            _plannedActions = await _taskBeforeNextStep;
            float endTime = UnityEngine.Time.time;
            float timeTakenSeconds = endTime - startTime;
            CustomLog.Instance.AddPlannerTime(timeTakenSeconds);
            IsPathPlanningDone = true;
        }
    }
}