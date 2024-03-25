using System;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.IO
{
    [Serializable]
    public struct SimulationConfig
    {
        [NonSerialized] public string basePath;// folder which contains the config file
        
        public string mapFile;
        public string agentFile;
        public int teamSize;
        public string taskFile;
        public int numTasksReveal;
        public TASK_ASSIGNMENT_STRATEGY taskAssignmentStrategy;
    }
}