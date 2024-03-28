using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.View.Sim
{
    public class UnitySimulationManager : MonoBehaviour
    {
        [SerializeField]
        private UnityMap unityMap;
    
        private SimulationManager simulationManager;
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.Setup("/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_config.json");

            unityMap.AssignMap(simulationManager.Map);
            unityMap.GenerateMap();
        }

        void Update()
        {
        
        }
    }   
}
