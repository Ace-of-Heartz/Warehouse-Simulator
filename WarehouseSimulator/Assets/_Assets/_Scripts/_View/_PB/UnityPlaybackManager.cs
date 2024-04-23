using UnityEngine;
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
    public PbInputArgs debugSimInputArgs = new PbInputArgs();

    private void Start()
    {
        Debug.Log("playback started");
        Debug.Log("ALMA: " + MainMenuManager.pbInputArgs.MapFilePath);
        Debug.Log("KÖRTE: " + MainMenuManager.pbInputArgs.EventLogPath);
    }
}
