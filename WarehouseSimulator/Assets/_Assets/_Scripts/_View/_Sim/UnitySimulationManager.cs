using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;
using WarehouseSimulator.View.MainMenu;

namespace WarehouseSimulator.View.Sim
{
    /// <summary>
    /// Unity part of the simulation manager
    /// </summary>
    public class UnitySimulationManager : MonoBehaviour
    {
        /// <summary>
        /// Robot prefab
        /// </summary>
        [SerializeField] private GameObject robie;
        /// <summary>
        /// Goal prefab
        /// </summary>
        [SerializeField] private GameObject golie;

        /// <summary>
        /// Map reference
        /// </summary>
        [SerializeField]
        private UnityMap unityMap;
    
        /// <summary>
        /// Model part of the simulation manager
        /// </summary>
        private SimulationManager simulationManager;
        /// <summary>
        /// Getter for the simulation data
        /// </summary>
        public SimulationData SimulationData => simulationManager.SimulationData; //TODO: To this safer
        /// <summary>
        /// Debug Mode. Kind of broken cause of UI.
        /// </summary>
        public bool DebugMode = false;
        /// <summary>
        /// Debug arguments for the simulation
        /// </summary>
        public SimInputArgs debugSimInputArgs = new SimInputArgs();

        /// <summary>
        /// Time in seconds until the next tick
        /// </summary>
        private float timeToNextTickCountdown = 0;
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.SimRobotManager.RobotAddedEvent += AddUnitySimRobot;
            simulationManager.SimRobotManager.GoalAssignedEvent += AddUnityGoal;
            try
            {
                if (DebugMode)
                {
                    DebugSetup();
                    simulationManager.Setup(debugSimInputArgs);
                }
                else
                    simulationManager.Setup(MainMenuManager.simInputArgs);
            }
            catch (Exception e)
            {
                UIMessageManager.GetInstance().MessageBox("Error during setup:\n" + e.Message, response =>
                {
                    SceneHandler.GetInstance().SetCurrentScene(0);
                    SceneManager.LoadScene(SceneHandler.GetInstance().CurrentScene);
                }, new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
                return;
            }

            unityMap.AssignMap(simulationManager.Map);
            unityMap.GenerateMap();
            
            timeToNextTickCountdown = simulationManager.SimulationData.m_stepTime / 1000.0f;
            
            GameObject.Find("UIGlobalManager").GetComponent<BindingSetupManager>().SetupSimBinding(simulationManager);
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

        /// <summary>
        /// Setup <see cref="debugSimInputArgs"/> data
        /// </summary>
        void DebugSetup()
        {
            debugSimInputArgs.ConfigFilePath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_config.json";
            debugSimInputArgs.PreparationTime = 1000;
            debugSimInputArgs.IntervalOfSteps = 800;
            debugSimInputArgs.NumberOfSteps = 100;
            debugSimInputArgs.EventLogPath = "/Users/gergogalig/Desktop/log.log";
            debugSimInputArgs.SearchAlgorithm = SearchAlgorithm.BFS;
        }

        /// <summary>
        /// Add a new Unity robot to the simulation
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">Contains the model part of the robot</param>
        /// <exception cref="ArgumentException">Thrown if we try to add an incorrect robot</exception>
        private void AddUnitySimRobot(object sender, RobotCreatedEventArgs e)
        {
            if (e.Robot is SimRobot simRobie)
            {
                GameObject rob  = Instantiate(robie);
                UnityRobot robieManager  = rob.GetComponent<UnityRobot>();
                robieManager.MyThingies(simRobie, unityMap, null, simulationManager);
            }
            else
            {
                #if DEBUG
                    throw new ArgumentException("Nagyon rossz robotot adtunk át a UnitySimulationManager-nek");
                #endif
            }
        }
        
        /// <summary>
        /// Add a new Unity goal to the simulation
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">Contains the model part of the goal</param>
        private void AddUnityGoal(object sender, GoalAssignedEventArgs e)
        {
            if (e.Goal is SimGoal simGolie)
            {
                GameObject gooo = Instantiate(golie);
                UnityGoal golieMan = gooo.GetComponent<UnityGoal>();
                golieMan.GiveGoalModel(simGolie,unityMap);
            }
        }
    }   
}
