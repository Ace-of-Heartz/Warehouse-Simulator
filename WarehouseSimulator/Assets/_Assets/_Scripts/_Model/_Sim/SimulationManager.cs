using System;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// Manages the simulation
    /// </summary>
    public class SimulationManager
    {
        #region Config Fields
        /// <summary>
        /// The simulation data for the simulation manager
        /// </summary>
        private SimulationData _simulationData;
        /// <summary>
        /// The path to the future log file
        /// </summary>
        private string _logFilePath;
        #endregion
        
        /// <summary>
        /// The map of the simulation
        /// </summary>
        private Map _map;
        /// <summary>
        /// The goal manager of the simulation
        /// </summary>
        private readonly SimGoalManager _simGoalManager;
        /// <summary>
        /// The robot manager of the simulation
        /// </summary>
        private readonly SimRobotManager _simRobotManager;
        /// <summary>
        /// The brain of the simulation
        /// </summary>
        private readonly CentralController _centralController;
        
        #region Properties
        /// <summary>
        /// See <see cref="_map"/> for docs
        /// </summary>
        public Map Map => _map;
        /// <summary>
        /// See <see cref="_simGoalManager"/> for docs
        /// </summary>
        public SimGoalManager SimGoalManager => _simGoalManager;
        /// <summary>
        /// See <see cref="_simRobotManager"/> for docs
        /// </summary>
        public SimRobotManager SimRobotManager => _simRobotManager;
        /// <summary>
        /// See <see cref="_simulationData"/> for docs
        /// </summary>
        public SimulationData SimulationData => _simulationData;
        /// <summary>
        /// Whether the preprocessing part is finished
        /// </summary>
        public bool IsPreprocessDone => _centralController.IsPreprocessDone;
        #endregion
        
        /// <summary>
        /// Constructor for the SimulationManager, does basic setup
        /// </summary>
        public SimulationManager()
        {
            _map = new Map();
            _simGoalManager = new SimGoalManager();
            _simRobotManager = new SimRobotManager();
            _centralController = new CentralController();
            _simulationData = ScriptableObject.CreateInstance<SimulationData>();
            CustomLog.Instance.Init();
            
            //event for adding robot to path planning
            _simRobotManager.RobotAddedEvent += (sender, args) =>
            {
                if (args.Robot is SimRobot robie)
                {
                    _centralController.AddRobotToPlanner(robie);
                }
            };
        }
        
        /// <summary>
        /// Sets up the simulation.
        /// </summary>
        /// <param name="simulationArgs">The configuration of the simulation </param>
        /// <exception cref="ArgumentException">Thrown if the selected search algorithm is invalid</exception>
        /// <exception cref="Exception">Thrown if any other error occurs during setup. This exception can take many forms, so good luck debugging.</exception>
        public async void Setup(SimInputArgs simulationArgs)
        {
            CustomLog.Instance.SetActionModel("almafa");
            
            _simulationData.m_maxStepAmount = simulationArgs.NumberOfSteps;
            _simulationData.m_currentStep = 0;
            _simulationData.m_robotAmount = 0;
            _simulationData.m_goalAmount = 0;
            _simulationData.m_goalsRemaining = 0;
            _simulationData.m_stepTime = simulationArgs.IntervalOfSteps;
            _simulationData.m_preprocessTime = simulationArgs.PreparationTime;
            _simulationData.m_isFinished = false;
            
            _logFilePath = simulationArgs.EventLogPath;
            
            SimulationConfig config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));
            config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            _map.LoadMap(config.basePath + config.mapFile);
            _simGoalManager.ReadGoals(config.basePath + config.taskFile, _map);
            _simRobotManager.RoboRead(config.basePath + config.agentFile, _map,config.teamSize);
            
            _simulationData.m_robotAmount = _simRobotManager.RobotCount;
            _simulationData.m_goalAmount = _simGoalManager.GoalCount;
            _simulationData.m_goalsRemaining = _simulationData.m_goalAmount;


            IPathPlanner pathPlanner;
            switch (simulationArgs.SearchAlgorithm)
            {
                case SearchAlgorithm.BFS:
                    pathPlanner = new BFS_PathPlanner();
                    break;
                case SearchAlgorithm.AStar:
                    pathPlanner = new AStar_PathPlanner();
                    break;
                case SearchAlgorithm.CoopAStar:
                    pathPlanner = new CoopAStar_PathPlanner();
                    break;
                case SearchAlgorithm.AStarAsync:
                    pathPlanner = new AStarAsync_PathPlanner();
                    break;
                default:
                    throw new ArgumentException("Invalid search algorithm");
            }
            pathPlanner.SetMap(_map);
            _centralController.AddPathPlanner(pathPlanner);
            _centralController.Preprocess(_map);
            _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);
            await _centralController.PlanNextMovesForAllAsync();
        }
        /// <summary>
        /// Performs one step of the simulation
        /// </summary>
        public async void Tick()
        {
            if (_simulationData.m_currentStep < _simulationData.m_maxStepAmount)
            {
                _centralController.TimeToMove(_simRobotManager,_map);
                _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);
                await _centralController.PlanNextMovesForAllAsync();
                
                _simulationData.m_currentStep++;
                _simulationData.m_goalsRemaining = _simGoalManager.GoalCount;
                CustomLog.Instance.SimulationStepCompleted();
            }
            else
            {
                Finished();
            }
        }

        /// <summary>
        /// The simulation is finished
        /// </summary>
        private void Finished()
        {
            _simulationData.m_isFinished = true;
            CustomLog.Instance.SaveLog(_logFilePath);
        }
    }
}