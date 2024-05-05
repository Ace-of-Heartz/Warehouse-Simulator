using System;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
        
    [Serializable]
    public struct SimulationConfig
    {
        /// <summary>
        /// The path of the folder which contains the config file
        /// No path separator at the end.
        /// </summary>
        [NonSerialized] public string basePath;

        /// <summary>
        /// The path of the map file
        /// </summary>
        public string mapFile;
        /// <summary>
        /// The path of the robots' file
        /// </summary>
        public string agentFile;
        /// <summary>
        /// The number of robots in the simulation requested
        /// </summary>
        public int teamSize;
        /// <summary>
        /// The path of the goals' file
        /// </summary>
        public string taskFile;
        /// <summary>
        /// The number of tasks to reveal to the pathPlanner. UNUSED
        /// </summary>
        public int numTasksReveal;
        /// <summary>
        /// The task assignment strategy to distribute the goals among the robots. IGNORED
        /// </summary>
        public string taskAssignmentStrategy;
        
        /// <summary>
        /// <see cref="taskAssignmentStrategy"/>, but as an enum
        /// </summary>
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