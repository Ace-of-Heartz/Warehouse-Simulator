using System.IO;

namespace WarehouseSimulator.Model.Sim
{
    public class SimulationManager
    {
        //config part
        private SimulationConfig config;
        private int maxSteps;
        private float stepTime;

        private Map map;
        // private GoalManager goalManager;
        // private RobotManager robotManager;
        // private CentralController centralController;

        public void Setup(string configFilePath)
        {
            ConfigIO configIO = new ();
            config = configIO.ParseFromJson(configIO.GetJsonContent(configFilePath).Result).Result;
            config.basePath = Path.GetDirectoryName(configFilePath) + Path.PathSeparator;
            
            map = new Map();
            map.LoadMap(config.basePath + config.mapFile);
            
            // TODO: goalManager(load)
            // TODO: robotManager(load)
            // TODO: centralController (preprocess)
        }
        
        // simulation tick
        public void Tick()
        {
            //TODO: centralController timeToMove and planNextSteps
        }

        // simulation completed
        public void Finished()
        {
            
        }

        // exit simultaion before completion
        public void Abort()
        {
            
        }
    }
}