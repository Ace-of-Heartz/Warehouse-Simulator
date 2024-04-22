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

        public PlaybackManager()
        {
            _map = new();
            _pbRobotManager = new();
            _pbGoalManager = new();
            _playbackData = ScriptableObject.CreateInstance<PlaybackData>();
        }

        public void Setup(string LogFilePath, string MapFilePath)
        {   
            _map.LoadMap(MapFilePath);
            CustomLog.Instance.LoadLog(LogFilePath);
            _playbackData.m_currentStep = 1;
            _playbackData.m_currentPlayBackSpeed = 1;
            _pbGoalManager.SetUpAllGoals(CustomLog.Instance.TaskData,CustomLog.Instance.TaskEvents);
            _pbRobotManager.SetUpAllRobots(CustomLog.Instance.StepsCompleted,CustomLog.Instance.StartPos,CustomLog.Instance.RobotActions);
        }

        public void SetTimeTo(int step)
        {
            _playbackData.m_currentStep = step;
            _pbGoalManager.SetTimeTo(step);
            _pbRobotManager.SetTimeTo(step);
        }
    }
}