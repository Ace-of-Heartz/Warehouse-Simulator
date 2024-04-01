using System;
using UnityEngine;
using WarehouseSimulator.Model.Sim;
using WarehouseSimulator.View.MainMenu;

namespace WarehouseSimulator.View.Sim
{
    public class UnitySimulationManager : MonoBehaviour
    {
        [SerializeField]
        private UnityMap unityMap;
    
        private SimulationManager simulationManager;

        public bool DebugMode = false;
        public SimInputArgs debugSimInputArgs = new SimInputArgs();
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.RobotManager.RobotAddedEvent += AddUnityRobot;
            if (DebugMode)
            {
                DebugSetup();
                simulationManager.Setup(debugSimInputArgs);
            }
            else
                simulationManager.Setup(MainMenuManager.simInputArgs);

            unityMap.AssignMap(simulationManager.Map);
            unityMap.GenerateMap();
        }

        void DebugSetup()
        {
            debugSimInputArgs.ConfigFilePath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_config.json";
            debugSimInputArgs.PreparationTime = 1;
            debugSimInputArgs.IntervalOfSteps = 1;
            debugSimInputArgs.NumberOfSteps = 100;
            debugSimInputArgs.EventLogPath = "/Users/gergogalig/log.log";
            
        }

        private void AddUnityRobot(object sender, RobotCreatedEventArgs e)
        {
            Debug.Log("Robot added to UnitySimulationManager. ID:" + e.robot.Id);
        }
    }   
}
