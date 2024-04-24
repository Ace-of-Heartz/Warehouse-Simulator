using UnityEngine;

namespace WarehouseSimulator.Model.PB
{
    [CreateAssetMenu(fileName = "PlaybackData", menuName = "Blaaa", order = 0)]
    public class PlaybackData : ScriptableObject
    {
        public int m_currentStep;
        public float m_currentPlayBackSpeed;
        public const int DEFAULT_PLAYBACK_TIME_MS = 1000;
    }
}