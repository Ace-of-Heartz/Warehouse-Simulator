using System.IO;


namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        #region Config Fields
        private SimulationConfig config;
        private int maxSteps;
        private float stepTime;
        private float preparationTime;
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
            _simRobotManager.RobotAddedEvent += (sender, args) => centralController.AddRobotToPlanner(args.SimRobot);
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
            centralController.Preprocess(map);
            centralController.PlanNextMovesForAll(map);
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