using UnityEngine;

namespace WarehouseSimulator.Model.PB
{
    /// <summary>
    /// Manages the playback
    /// </summary>
    public class PlaybackManager
    {
        /// <summary>
        /// The map on which the playback is running
        /// </summary>
        private Map _map;
        /// <summary>
        /// The manager for the robots in the playback
        /// </summary>
        private PbRobotManager _pbRobotManager;
        /// <summary>
        /// The manager for the goals in the playback
        /// </summary>
        private PbGoalManager _pbGoalManager;
        /// <summary>
        /// TODO
        /// </summary>
        private PlaybackData _playbackData;

        /// <summary>
        /// TODO:
        /// </summary>
        public PlaybackData PlaybackData => _playbackData;
        /// <summary>
        /// The manager for the robots in the playback. Get only
        /// </summary>
        public PbRobotManager PbRobotManager => _pbRobotManager;
        /// <summary>
        /// The manager for the goals in the playback. Get only
        /// </summary>
        public PbGoalManager PbGoalManager => _pbGoalManager;
        /// <summary>
        /// The map on which the playback is running. Get only
        /// </summary>
        public Map Map => _map;

        /// <summary>
        /// Constructor for the PlaybackManager class. Yes it is redundant. Yes it works. Yes this summary is necessary. And yes, have a good day.
        /// </summary>
        public PlaybackManager()
        {
            _map = new();
            _pbRobotManager = new();
            _pbGoalManager = new();
            _playbackData = ScriptableObject.CreateInstance<PlaybackData>();
        }

        /// <summary>
        /// Sets up the playback with the given arguments
        /// </summary>
        /// <param name="pbInputArgs">The arguments containing the map and logfile path</param>
        public void Setup(PbInputArgs pbInputArgs)
        {   
            _map.LoadMap(pbInputArgs.MapFilePath);
            CustomLog.Instance.LoadLog(pbInputArgs.EventLogPath);
            
            _playbackData.CurrentStep = 0;
            _playbackData.PlaybackSpeed = 1;
            _playbackData.IsPaused = false;
            _playbackData.MaxStepAmount = CustomLog.Instance.StepsCompleted;
            
            _pbGoalManager.SetUpAllGoals(CustomLog.Instance.TaskData,CustomLog.Instance.TaskEvents);
            _pbRobotManager.SetUpAllRobots(CustomLog.Instance.StepsCompleted,CustomLog.Instance.StartPos);
            
            SetTimeTo(0);
        }

        
        /// <summary>
        /// Seeks to the specified time.
        /// </summary>
        /// <param name="stateIndex">The current stateIndex</param>
        public void SetTimeTo(int stateIndex)
        {
            if (stateIndex < 0 || stateIndex > _playbackData.MaxStepAmount)
            {
                if (!PlaybackData.IsPaused)
                    PlaybackData.ChangePauseState();//pause if out of bounds
                
                return;
            }
            _playbackData.CurrentStep = stateIndex;
            _pbGoalManager.SetTimeTo(stateIndex);
            _pbRobotManager.SetTimeTo(stateIndex);
        }
        
        /// <summary>
        /// Advances the playback by one step.
        /// </summary>
        public void NextState()
        {
            SetTimeTo(_playbackData.CurrentStep + 1);
        }
        
        /// <summary>
        /// Un-advances the playback by one step.
        /// </summary>
        public void PreviousState()
        {
            SetTimeTo(_playbackData.CurrentStep - 1);
        }
    }
}