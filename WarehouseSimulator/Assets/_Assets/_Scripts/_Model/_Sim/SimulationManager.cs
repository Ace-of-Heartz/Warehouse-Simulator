using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        #region Config Fields
        private SimulationConfig config;
        private SimulationData m_simulationData;
        private string logFilePath;
        #endregion
        
        private Map map;
        private SimGoalManager _simGoalManager;
        private SimRobotManager _simRobotManager;
        private CentralController centralController;

        
        #region Properties
        public Map Map => map;
        public SimGoalManager SimGoalManager => _simGoalManager;
        public SimRobotManager SimRobotManager => _simRobotManager;
        public SimulationData SimulationData => m_simulationData;
        public bool IsPreprocessDone => centralController.IsPreprocessDone;
        #endregion
        
        public SimulationManager()
        {
            map = new Map();
            _simGoalManager = new SimGoalManager();
            _simRobotManager = new SimRobotManager();
            centralController = new CentralController(map);
            m_simulationData = ScriptableObject.CreateInstance<SimulationData>();
            
            //event for adding robot to path planning
            _simRobotManager.RobotAddedEvent += (sender, args) =>
            {
                if (args.Robot is SimRobot robie)
                {
                    centralController.AddRobotToPlanner(robie);
                }
            };
        }
        
        public void Setup(SimInputArgs simulationArgs)
        {
            CustomLog.Instance.SetActionModel("almafa");
            
            m_simulationData.m_maxStepAmount = simulationArgs.NumberOfSteps;
            m_simulationData.m_currentStep = 1;
            m_simulationData.m_robotAmount = 0;
            m_simulationData.m_goalAmount = 0;
            m_simulationData.m_goalsRemaining = 0;
            m_simulationData.m_stepTime = simulationArgs.IntervalOfSteps;
            m_simulationData.m_preprocessTime = simulationArgs.PreparationTime;
            m_simulationData.m_isFinished = false;
            
            logFilePath = simulationArgs.EventLogPath;
            
            config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));//todo: error handling
            config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            map.LoadMap(config.basePath + config.mapFile);
            _simGoalManager.ReadGoals(config.basePath + config.taskFile, map);
            _simRobotManager.RoboRead(config.basePath + config.agentFile, map,config.teamSize);
            
            m_simulationData.m_robotAmount = _simRobotManager.RobotCount;
            m_simulationData.m_goalAmount = _simGoalManager.GoalCount;
            m_simulationData.m_goalsRemaining = m_simulationData.m_goalAmount;
            
            centralController.Preprocess(map);
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
            centralController.AddPathPlanner(pathPlanner);
            centralController.Preprocess(map);
            centralController.PlanNextMovesForAllAsync(map);
        }
        
        public void Tick()
        {
            if (m_simulationData.m_currentStep <= m_simulationData.m_maxStepAmount)
            {
                Debug.Log("stepping");
                centralController.TimeToMove(map,_simRobotManager);
                //centralController.PlanNextMoves(map);
                _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);
                
                m_simulationData.m_currentStep++;
                m_simulationData.m_goalsRemaining = _simGoalManager.GoalCount;
                CustomLog.Instance.SimulationStepCompleted();
            }
            else
            {
                Finished();
            }
        }

        // info: simulation completed
        public void Finished()
        {
            m_simulationData.m_isFinished = true;
            //TODO => Blaaa
        }

        // info: exit simultaion before completion
        public void Abort()
        {
            //TODO => Blaaa
        }
    }
}