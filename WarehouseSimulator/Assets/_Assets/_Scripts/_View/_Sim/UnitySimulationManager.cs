using System.Collections.Generic;
using UnityEngine;
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
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.RobotManager.RobotAddedEvent += AddUnityRobot;
            simulationManager.RobotManager.GoalAssignedEvent += AddUnityGoal;
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
            GameObject rob  = Instantiate(robie);
            UnityRobot robieManager  = rob.GetComponent<UnityRobot>();
            robieManager.MyRoboModel(e.robot);
        }
        
        private void AddUnityGoal(object sender, GoalAssignedEventArgs e)
        {
            Debug.Log("Robot added to UnitySimulationManager. ID:" + e.goal.Robot.Id);
            GameObject gooo = (golie);
            UnityGoal golieManag = gooo.GetComponent<UnityGoal>();
            golieManag.GiveGoalModel(e.goal);
        }
    }   
}
