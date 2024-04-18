using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;
using WarehouseSimulator.Model;
using WarehouseSimulator.View.MainMenu;

namespace WarehouseSimulator.View.Sim
{
    public class UnitySimulationManager : MonoBehaviour
    {
        [SerializeField] private GameObject robie;
        [SerializeField] private GameObject golie;


        
        [SerializeField]
        private UnityMap unityMap;
    
        private SimulationManager simulationManager;

        public bool DebugMode = false;
        public SimInputArgs debugSimInputArgs = new SimInputArgs();

        private float timeSinceLastTick = 0;
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.SimRobotManager.RobotAddedEvent += AddUnitySimRobot;
            simulationManager.SimRobotManager.GoalAssignedEvent += AddUnityGoal;
            if (DebugMode)
            {
                simulationManager.Setup(MainMenuManager.simInputArgs); 

                // DebugSetup();
                // simulationManager.Setup(debugSimInputArgs);
            }
            else
                simulationManager.Setup(MainMenuManager.simInputArgs);

            unityMap.AssignMap(simulationManager.Map);
            unityMap.GenerateMap();
        }

        void Update()
        {
            //while preprocessing is in progress, we wait
            if(!simulationManager.IsPreprocessDone)
                return;
            
            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= simulationManager.StepTime)
            {
                simulationManager.Tick();
                timeSinceLastTick = 0;
            }
        }

        void DebugSetup()
        {
            debugSimInputArgs.ConfigFilePath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_config.json";
            debugSimInputArgs.PreparationTime = 1;
            debugSimInputArgs.IntervalOfSteps = 3;
            debugSimInputArgs.NumberOfSteps = 100;
            debugSimInputArgs.EventLogPath = "/Users/gergogalig/log.log";
            debugSimInputArgs.SearchAlgorithm = SEARCH_ALGORITHM.BFS;
        }

        private void AddUnitySimRobot(object sender, RobotCreatedEventArgs e)
        {
            if (e.Robot is SimRobot simRobie)
            {
                Debug.Log("Robot added to UnitySimulationManager. ID:" + simRobie.Id);
                GameObject rob  = Instantiate(robie);
                UnityRobot robieManager  = rob.GetComponent<UnityRobot>();
                robieManager.MyThingies(simRobie,unityMap,simulationManager.StepTime);
            }
            else
            {
                #if DEBUG
                throw new ArgumentException("Nagyon rossz robotot adtunk át a UnitySimulationManager-nek");
                #endif
            }
        }
        
        private void AddUnityGoal(object sender, GoalAssignedEventArgs e)
        {
            if (e.Goal is SimGoal simGolie)
            {
                Debug.Log("Robot added to UnitySimulationManager. ID:" + simGolie.SimRobot.Id);
                GameObject gooo = Instantiate(golie);
                UnityGoal golieMan = gooo.GetComponent<UnityGoal>();
                golieMan.GiveGoalModel(simGolie,unityMap);
            }
        }
    }   
}
