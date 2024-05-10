using UnityEngine;
using UnityEngine.Serialization;

namespace WarehouseSimulator.Model.PB
{
    /// <summary>
    /// Holds the playback data for the <see cref="PlaybackManager"/>
    /// </summary>
    [CreateAssetMenu(fileName = "PlaybackData", menuName = "Blaaa", order = 0)]
    public class PlaybackData : ScriptableObject
    {
        /// <summary>
        /// The default value "playback speed"
        /// </summary>
        public const int DEFAULT_PLAYBACK_TIME_MS = 1000;
        
        //TODO: these are state indecies, not steps
        /// <summary>
        /// The current state index
        /// </summary>
        [SerializeField]
        private int _currentStep;
        /// <summary>
        /// Property for the current state index
        /// </summary>
        public int CurrentStep
        {
            get { return _currentStep; }
            set
            {
                if(value < 0 || value > _maxStepAmount)
                {
                    return;
                }
                _currentStep = value;
            }
        }
        /// <summary>
        /// The playback speed. 1 is the default speed
        /// </summary>
        [SerializeField]
        private float _playbackSpeed;
        /// <summary>
        /// Property for the playback speed
        /// </summary>
        public float PlaybackSpeed
        {
            get { return _playbackSpeed; }
            set { _playbackSpeed = value; }
        }
        /// <summary>
        /// The maximum amount of steps
        /// </summary>
        [SerializeField]
        private int _maxStepAmount;

        /// <summary>
        /// Property for the maximum amount of steps
        /// </summary>
        public int MaxStepAmount
        {
            get => _maxStepAmount;
            
            set {
                if (value > 0)
                {
                    _maxStepAmount = value;
                }
                
            }
        }
        /// <summary>
        /// Playback time pause state
        /// </summary>
        [SerializeField]
        private bool _isPaused = false;

        /// <summary>
        /// Property for the playback pause state
        /// </summary>
        public bool IsPaused
        {
            get => _isPaused;
            set => _isPaused = value;
        }
        /// <summary>
        /// Changes the pause state
        /// </summary>
        /// <returns>The new pause state</returns>
        public bool ChangePauseState()
        {
            _isPaused = !_isPaused;
            return _isPaused;
        }
    }
}