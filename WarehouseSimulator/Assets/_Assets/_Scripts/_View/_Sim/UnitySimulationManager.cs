using System;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;
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

        private float timeToNextTickCountdown = 0;
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.SimRobotManager.RobotAddedEvent += AddUnitySimRobot;
            simulationManager.SimRobotManager.GoalAssignedEvent += AddUnityGoal;
            if (DebugMode)
            {
                DebugSetup();
                simulationManager.Setup(debugSimInputArgs);
            }
            else
                simulationManager.Setup(MainMenuManager.simInputArgs);

            unityMap.AssignMap(simulationManager.Map);
            unityMap.GenerateMap();
            
            GameObject.Find("UIGlobalManager").GetComponent<BindingSetupManager>().SetupSimBinding(simulationManager.SimulationData);
        }

        void Update()
        {
            //while preprocessing is in progress, we wait
            if(!simulationManager.IsPreprocessDone)
                return;
            
            timeToNextTickCountdown -= Time.deltaTime;
            if (timeToNextTickCountdown <= 0)
            {
                simulationManager.Tick();
                timeToNextTickCountdown= simulationManager.SimulationData.m_stepTime / 1000.0f;
            }
        }

        void DebugSetup()
        {
            debugSimInputArgs.ConfigFilePath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_config.json";
            debugSimInputArgs.PreparationTime = 1000;
            debugSimInputArgs.IntervalOfSteps = 800;
            debugSimInputArgs.NumberOfSteps = 100;
            debugSimInputArgs.EventLogPath = "/Users/gergogalig/Desktop/log.log";
            debugSimInputArgs.SearchAlgorithm = SEARCH_ALGORITHM.BFS;
        }

        private void AddUnitySimRobot(object sender, RobotCreatedEventArgs e)
        {
            if (e.Robot is SimRobot simRobie)
            {
                GameObject rob  = Instantiate(robie);
                UnityRobot robieManager  = rob.GetComponent<UnityRobot>();
                robieManager.MyThingies(simRobie,unityMap,simulationManager.SimulationData.m_stepTime);
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
                GameObject gooo = Instantiate(golie);
                UnityGoal golieMan = gooo.GetComponent<UnityGoal>();
                golieMan.GiveGoalModel(simGolie,unityMap);
            }
        }

        public void AddNewGoal(Vector2Int position)
        {
            simulationManager.SimGoalManager.AddNewGoal(position);
        }
    }   
}
