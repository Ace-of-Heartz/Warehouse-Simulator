using System.IO;
using WarehouseSimulator.View.MainMenu;

namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        //config part
        private SimulationConfig config;
        private int maxSteps;
        private float stepTime;
        private float preparationTime;

        private Map map;
        // private GoalManager goalManager;
        // private RobotManager robotManager;
        // private CentralController centralController;

        
        // Properties
        public Map Map => map;
        
        public void Setup(SimInputArgs simulationArgs)
        {
            maxSteps = simulationArgs.NumberOfSteps;
            stepTime = simulationArgs.IntervalOfSteps;
            preparationTime = simulationArgs.PreparationTime;
            
            config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(simulationArgs.ConfigFilePath));//todo: error handling
            config.basePath = Path.GetDirectoryName(simulationArgs.ConfigFilePath) + Path.DirectorySeparatorChar;
            
            map = new Map();
            map.LoadMap(config.basePath + config.mapFile);
            
            // TODO: goalManager(load)
            // TODO: robotManager(load)
            // TODO: centralController (preprocess)
        }
        
        // info: simulation tick
        public void Tick()
        {
            //TODO: centralController timeToMove and planNextSteps
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