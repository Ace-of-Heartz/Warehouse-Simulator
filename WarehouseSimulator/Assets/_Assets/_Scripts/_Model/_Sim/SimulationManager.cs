using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        #region Config Fields
        private SimulationConfig config;
        private SimulationData _simulationData;
        private string logFilePath;
        #endregion
        
        private Map map;
        private SimGoalManager _simGoalManager;
        private SimRobotManager _simRobotManager;
        private CentralController _centralController;

        
        #region Properties
        public Map Map => map;
        public SimGoalManager SimGoalManager => _simGoalManager;
        public SimRobotManager SimRobotManager => _simRobotManager;
        public SimulationData SimulationData => _simulationData;
        public bool IsPreprocessDone => _centralController.IsPreprocessDone;
        #endregion
        
        public SimulationManager()
        {
            map = new Map();
            _simGoalManager = new SimGoalManager();
            _simRobotManager = new SimRobotManager();
            _centralController = new CentralController(map);
            _simulationData = ScriptableObject.CreateInstance<SimulationData>();
            
            //event for adding robot to path planning
            _simRobotManager.RobotAddedEvent += (sender, args) =>
            {
                if (args.Robot is SimRobot robie)
                {
                    _centralController.AddRobotToPlanner(robie);
                }
            };
        }
        
        public void Setup(SimInputArgs simulationArgs)
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
            
            logFilePath = simulationArgs.EventLogPath;
            
            config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));//todo: error handling
            config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            map.LoadMap(config.basePath + config.mapFile);
            _simGoalManager.ReadGoals(config.basePath + config.taskFile, map);
            _simRobotManager.RoboRead(config.basePath + config.agentFile, map,config.teamSize);
            
            _simulationData.m_robotAmount = _simRobotManager.RobotCount;
            _simulationData.m_goalAmount = _simGoalManager.GoalCount;
            _simulationData.m_goalsRemaining = _simulationData.m_goalAmount;
            
            _centralController.Preprocess(map);
            _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);

            IPathPlanner pathPlanner;
            switch (simulationArgs.SearchAlgorithm)
            {
                case SEARCH_ALGORITHM.BFS:
                    pathPlanner = new BFS_PathPlanner(map);
                    break;
                case SEARCH_ALGORITHM.A_STAR:
                    pathPlanner = new AStar_PathPlanner(map);
                    break;
                case SEARCH_ALGORITHM.COOP_A_STAR:
                    pathPlanner = new CoopAStar_PathPlanner(map);
                    break;
                default:
                    throw new System.ArgumentException("Invalid search algorithm");
            }
            _centralController.AddPathPlanner(pathPlanner);
            _centralController.Preprocess(map);
            _centralController.PlanNextMovesForAllAsync(map);
        }
        
        public void Tick()
        {
            if (_simulationData.m_currentStep <= _simulationData.m_maxStepAmount)
            {
                _centralController.TimeToMove(map,_simRobotManager);
                _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);
                _centralController.PlanNextMovesForAllAsync(map);
                
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
            CustomLog.Instance.SaveLog(logFilePath);
        }

        // info: exit simultaion before completion
        public void Abort()
        {
            //TODO => Blaaa
        }
    }
}