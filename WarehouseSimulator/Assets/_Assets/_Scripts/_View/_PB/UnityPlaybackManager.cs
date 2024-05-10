using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.PB;
using WarehouseSimulator.View;
using WarehouseSimulator.View.MainMenu;

public class UnityPlaybackManager : MonoBehaviour
{
    /// <summary>
    /// The prefab for the robot
    /// </summary>
    [SerializeField] private GameObject robie;
    /// <summary>
    /// The prefab for the goal
    /// </summary>
    [SerializeField] private GameObject golie;
    
    /// <summary>
    /// The reference to the map
    /// </summary>
    [SerializeField]
    private UnityMap unityMap;
    
    /// <summary>
    /// The model part of the playback manager
    /// </summary>
    private PlaybackManager playbackManager;
    
    /// <summary>
    /// Debug Mode. Kind of broken cause of UI.
    /// </summary>
    public bool DebugMode = false;
    /// <summary>
    /// Debug arguments for the simulation
    /// </summary>
    public PbInputArgs debugPbInputArgs = new PbInputArgs();
    /// <summary>
    /// Holds the playback data
    /// </summary>
    public PlaybackData playbackData;
    
    /// <summary>
    /// The time in seconds until the next tick
    /// </summary>
    private float timeToNextTickCountdown = 0;

    private void Start()
    {
        playbackManager = new PlaybackManager();
        playbackManager.PbRobotManager.RobotAddedEvent += AddUnityPbRobot;//TODO
        playbackManager.PbGoalManager.GoalAssignedEvent += AddUnityGoal;//TODO
        playbackData = playbackManager.PlaybackData;
        
        try
        {
            if (DebugMode)
            {
                DebugSetup();
                playbackManager.Setup(debugPbInputArgs);
            }
            else
                playbackManager.Setup(MainMenuManager.pbInputArgs);
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
        
        unityMap.AssignMap(playbackManager.Map);
        unityMap.GenerateMap();
        
        timeToNextTickCountdown = PlaybackData.DEFAULT_PLAYBACK_TIME_MS / 1000.0f;
        
        GameObject.Find("UIGlobalManager")?.GetComponent<BindingSetupManager>().SetupPlaybackBinding(playbackManager);
    }
    /// <summary>
    /// Setup <see cref="debugPbInputArgs"/> data
    /// </summary>
    void DebugSetup()
    {
        debugPbInputArgs.MapFilePath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/maps/warehouse.map";
        debugPbInputArgs.EventLogPath = "/Users/gergogalig/Library/CloudStorage/OneDrive-EotvosLorandTudomanyegyetem/FourthSemester/Szofttech/sample_files/warehouse_100_log.json";
    }
    
    /// <summary>
    /// Add a new Unity robot to the playback
    /// </summary>
    /// <param name="sender">unused</param>
    /// <param name="e">Contains the model part of the robot</param>
    /// <exception cref="ArgumentException">Thrown if we try to add an incorrect robot</exception>
    private void AddUnityPbRobot(object sender, RobotCreatedEventArgs e)
    {
        if (e.Robot is PbRobot pbRobie)
        {
            GameObject rob  = Instantiate(robie);
            UnityRobot robieManager  = rob.GetComponent<UnityRobot>();
            robieManager.MyThingies(pbRobie, unityMap, playbackManager.PlaybackData.PlaybackSpeed);
        }
        else
        {
#if DEBUG
            throw new ArgumentException("Nagyon rossz robotot adtunk át a UnitySimulationManager-nek");
#endif
        }
    }
    
    /// <summary>
    /// Add a new Unity goal to the playback
    /// </summary>
    /// <param name="sender">unused</param>
    /// <param name="e">Contains the model part of the goal</param>
    private void AddUnityGoal(object sender, GoalAssignedEventArgs e)
    {
        if (e.Goal is PbGoal pbGolie)
        {
            GameObject gooo = Instantiate(golie);
            UnityGoal golieMan = gooo.GetComponent<UnityGoal>();
            golieMan.GiveGoalModel(pbGolie,unityMap);
        }
    }
    
    private void Update()
    {
        //keyboard override
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playbackManager.SetTimeTo(playbackManager.PlaybackData.CurrentStep - 1);
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playbackManager.NextState();
        }
        
        //automatic playback
        if(playbackManager.PlaybackData.IsPaused)
            return;
        
        timeToNextTickCountdown -= Time.deltaTime * playbackManager.PlaybackData.PlaybackSpeed;
        if (timeToNextTickCountdown <= 0)
        {
            playbackManager.NextState();
            timeToNextTickCountdown = PlaybackData.DEFAULT_PLAYBACK_TIME_MS / 1000.0f;
        }
    }
}
