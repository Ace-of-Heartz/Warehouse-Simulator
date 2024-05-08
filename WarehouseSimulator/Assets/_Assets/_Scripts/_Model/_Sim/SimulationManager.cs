using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        #region Config Fields
        private SimulationConfig _config;
        private SimulationData _simulationData;
        private string _logFilePath;
        #endregion
        
        private Map _map;
        private readonly SimGoalManager _simGoalManager;
        private readonly SimRobotManager _simRobotManager;
        private readonly CentralController _centralController;
        
        #region Properties
        public Map Map => _map;
        public SimGoalManager SimGoalManager => _simGoalManager;
        public SimRobotManager SimRobotManager => _simRobotManager;
        public SimulationData SimulationData => _simulationData;
        public bool IsPreprocessDone => _centralController.IsPreprocessDone;
        #endregion
        
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
            
            _config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));//todo: error handling
            _config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            _map.LoadMap(_config.basePath + _config.mapFile);
            _simGoalManager.ReadGoals(_config.basePath + _config.taskFile, _map);
            _simRobotManager.RoboRead(_config.basePath + _config.agentFile, _map,_config.teamSize);
            
            _simulationData.m_robotAmount = _simRobotManager.RobotCount;
            _simulationData.m_goalAmount = _simGoalManager.GoalCount;
            _simulationData.m_goalsRemaining = _simulationData.m_goalAmount;


            IPathPlanner pathPlanner;
            switch (simulationArgs.SearchAlgorithm)
            {
                case SEARCH_ALGORITHM.BFS:
                    pathPlanner = new BFS_PathPlanner();
                    break;
                case SEARCH_ALGORITHM.A_STAR:
                    pathPlanner = new AStar_PathPlanner();
                    break;
                case SEARCH_ALGORITHM.COOP_A_STAR:
                    pathPlanner = new CoopAStar_PathPlanner();
                    break;
                default:
                    throw new System.ArgumentException("Invalid search algorithm");
            }
            pathPlanner.SetMap(_map);
            _centralController.AddPathPlanner(pathPlanner);
            _centralController.Preprocess(_map);
            _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);
            await _centralController.PlanNextMovesForAllAsync();
        }
        
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

        // info: simulation completed
        private void Finished()
        {
            _simulationData.m_isFinished = true;
            CustomLog.Instance.SaveLog(_logFilePath);
        }
    }
}