using System;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
        
    [Serializable]
    public struct SimulationConfig
    {
        [NonSerialized] public string basePath; // folder which contains the config file

        public string mapFile;
        public string agentFile;
        public int teamSize;
        public string taskFile;
        public int numTasksReveal;
        public string taskAssignmentStrategy;
        
        public TASK_ASSIGNMENT_STRATEGY TaskAssignmentStrategy
        {
            get
            {
                return taskAssignmentStrategy switch
                {
                    "roundrobin" => TASK_ASSIGNMENT_STRATEGY.ROUNDROBIN,
                    _ => TASK_ASSIGNMENT_STRATEGY.ROUNDROBIN // default
                };
            }
        }
    }
    
}