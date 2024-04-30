using UnityEngine;

namespace WarehouseSimulator.Model.PB
{
    public class PlaybackManager
    {
        private Map _map;
        private PbRobotManager _pbRobotManager;
        private PbGoalManager _pbGoalManager;
        private PlaybackData _playbackData;

        public PlaybackData PlaybackData => _playbackData;
        public PbRobotManager PbRobotManager => _pbRobotManager;
        public PbGoalManager PbGoalManager => _pbGoalManager;
        public Map Map => _map;

        public PlaybackManager()
        {
            _map = new();
            _pbRobotManager = new();
            _pbGoalManager = new();
            _playbackData = ScriptableObject.CreateInstance<PlaybackData>();
        }

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
        
        public void NextState()
        {
            SetTimeTo(_playbackData.CurrentStep + 1);
        }
        
        public void PreviousState()
        {
            SetTimeTo(_playbackData.CurrentStep - 1);
        }
    }
}