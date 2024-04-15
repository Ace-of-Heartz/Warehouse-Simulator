using System.IO;
using WarehouseSimulator.View.MainMenu;

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
        private GoalManager goalManager;
        private RobotManager robotManager;
        private CentralController centralController;

        
        #region Properties
        public Map Map => map;
        public GoalManager GoalManager => goalManager;
        public RobotManager RobotManager => robotManager;
        public float StepTime => stepTime;
        public bool IsPreprocessDone => centralController.IsPreprocessDone;
        #endregion
        
        public SimulationManager()
        {
            map = new Map();
            goalManager = new GoalManager();
            robotManager = new RobotManager();
            centralController = new CentralController();
            
            //event for adding robot to path planning
            robotManager.RobotAddedEvent += (sender, args) => centralController.AddRobotToPlanner(args.SimRobot);
        }
        
        public void Setup(SimInputArgs simulationArgs)
        {
            maxSteps = simulationArgs.NumberOfSteps;
            stepTime = simulationArgs.IntervalOfSteps;
            preparationTime = simulationArgs.PreparationTime;
            
            config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));//todo: error handling
            config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            map.LoadMap(config.basePath + config.mapFile);
            goalManager.ReadGoals(config.basePath + config.taskFile, map);
            robotManager.RoboRead(config.basePath + config.agentFile, map);
            
            robotManager.AssignTasksToFreeRobots(goalManager);
            centralController.Preprocess(map);
            centralController.PlanNextMoves(map);
        }
        
        public void Tick()
        {
            centralController.TimeToMove(map);
            centralController.PlanNextMoves(map);
            robotManager.AssignTasksToFreeRobots(goalManager);
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