using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.PB;
using WarehouseSimulator.View;
using WarehouseSimulator.View.MainMenu;

public class UnityPlaybackManager : MonoBehaviour
{
    [SerializeField] private GameObject robie;
    [SerializeField] private GameObject golie;
    
    [SerializeField]
    private UnityMap unityMap;
    
    private PlaybackManager playbackManager;
    
    public bool DebugMode = false;
    public PbInputArgs debugPbInputArgs = new PbInputArgs();

    private void Start()
    {
        playbackManager = new PlaybackManager();
        playbackManager.PbRobotManager.RobotAddedEvent += AddUnityPbRobot;//TODO
        playbackManager.PbGoalManager.GoalAssignedEvent += AddUnityGoal;//TODO
        
            if (DebugMode)
            {
                DebugSetup();
                playbackManager.Setup(debugPbInputArgs);
            }
            else
                playbackManager.Setup(MainMenuManager.pbInputArgs);
        try
        {
        }
        catch (Exception)
        {
            Debug.Log("Some ex");
            UIMessageManager.GetInstance().MessageBox("Error during setup", response =>
            {
                SceneHandler.GetInstance().SetCurrentScene(0);
                SceneManager.LoadScene(SceneHandler.GetInstance().CurrentScene);
            }, new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
            return;
        }
        
        unityMap.AssignMap(playbackManager.Map);
        unityMap.GenerateMap();
        
        //TODO: binding magic
    }

    void DebugSetup()
    {
        debugPbInputArgs.MapFilePath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/maps/warehouse.map";
        debugPbInputArgs.EventLogPath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_log.json";
    }
    
    
    private void AddUnityPbRobot(object sender, RobotCreatedEventArgs e)
    {
        if (e.Robot is PbRobot pbRobie)
        {
            GameObject rob  = Instantiate(robie);
            UnityRobot robieManager  = rob.GetComponent<UnityRobot>();
            robieManager.MyThingies(pbRobie, unityMap, playbackManager.PlaybackData.m_currentPlayBackSpeed);
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
        if (e.Goal is PbGoal pbGolie)
        {
            GameObject gooo = Instantiate(golie);
            UnityGoal golieMan = gooo.GetComponent<UnityGoal>();
            golieMan.GiveGoalModel(pbGolie,unityMap);
        }
    }
}
