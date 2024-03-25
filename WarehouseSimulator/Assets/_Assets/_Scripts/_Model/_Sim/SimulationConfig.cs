using System;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;

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
    }
    
}