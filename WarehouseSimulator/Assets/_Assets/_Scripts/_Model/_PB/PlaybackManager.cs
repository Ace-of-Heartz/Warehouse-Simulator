using UnityEditor;
using UnityEngine;
using WarehouseSimulator.Model;

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
            _playbackData.MaxStepAmount = 69; //Arbitrary value
            
            _pbGoalManager.SetUpAllGoals(CustomLog.Instance.TaskData,CustomLog.Instance.TaskEvents);
            _pbRobotManager.SetUpAllRobots(CustomLog.Instance.StepsCompleted,CustomLog.Instance.StartPos);
            
            SetTimeTo(0);
        }

        public void SetTimeTo(int stateIndex)
        {
            _playbackData.CurrentStep = stateIndex;
            _pbGoalManager.SetTimeTo(stateIndex);
            _pbRobotManager.SetTimeTo(stateIndex);
        }
    }
}