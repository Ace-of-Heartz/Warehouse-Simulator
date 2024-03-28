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

        
        // Properties
        public Map Map => map;
        
        public void Setup(string configFilePath)
        {
            config = ConfigIO.ParseFromJson(ConfigIO.GetJsonContent(configFilePath));//todo: error handling
            config.basePath = Path.GetDirectoryName(configFilePath) + Path.DirectorySeparatorChar;
            
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