using System.IO;
using WarehouseSimulator.Model.Enums;


namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        #region Config Fields
        private SimulationConfig config;
        private int maxSteps;
        private float stepTime;
        private float preparationTime;

        private SimulationData m_simulationData;
        #endregion
        
        private Map map;
        private SimGoalManager _simGoalManager;
        private SimRobotManager _simRobotManager;
        private CentralController centralController;

        
        #region Properties
        public Map Map => map;
        public SimGoalManager SimGoalManager => _simGoalManager;
        public SimRobotManager SimRobotManager => _simRobotManager;
        public float StepTime => stepTime;
        public bool IsPreprocessDone => centralController.IsPreprocessDone;
        #endregion
        
        public SimulationManager()
        {
            map = new Map();
            _simGoalManager = new SimGoalManager();
            _simRobotManager = new SimRobotManager();
            centralController = new CentralController(map);
            
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
            maxSteps = simulationArgs.NumberOfSteps;
            stepTime = simulationArgs.IntervalOfSteps;
            preparationTime = simulationArgs.PreparationTime;
            
            config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));//todo: error handling
            config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            map.LoadMap(config.basePath + config.mapFile);
            _simGoalManager.ReadGoals(config.basePath + config.taskFile, map);
            _simRobotManager.RoboRead(config.basePath + config.agentFile, map);
            
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
            centralController.TimeToMove(map);
            //centralController.PlanNextMoves(map);
            _simRobotManager.AssignTasksToFreeRobots(_simGoalManager);
        }

        // info: simulation completed
        public void Finished()
        {
            
        }

        // info: exit simultaion before completion
        public void Abort()
        {
            
        }
    }
}