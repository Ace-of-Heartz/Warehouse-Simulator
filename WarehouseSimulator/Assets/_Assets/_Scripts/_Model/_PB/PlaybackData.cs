using UnityEngine;
using UnityEngine.Serialization;

namespace WarehouseSimulator.Model.PB
{
    [CreateAssetMenu(fileName = "PlaybackData", menuName = "Blaaa", order = 0)]
    public class PlaybackData : ScriptableObject
    {
        public const int DEFAULT_PLAYBACK_TIME_MS = 1000;
        
        //TODO: these are state indecies, not steps
        [SerializeField]
        private int _currentStep;
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
        [SerializeField]
        private float _playbackSpeed;
        public float PlaybackSpeed
        {
            get { return _playbackSpeed; }
            set { _playbackSpeed = value; }
        }
        [SerializeField]
        private int _maxStepAmount;

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

        [SerializeField]
        private bool _isPaused = false;

        public bool IsPaused
        {
            get => _isPaused;
        }
        
        public bool ChangePauseState()
        {
            _isPaused = !_isPaused;
            return _isPaused;
        }
        
        /// <summary>
        /// Increases the current step by 1
        /// Will fail if the current step is already at the maximum step amount
        /// </summary>
        public void IncrementStep()
        {
            if (_currentStep < _maxStepAmount) _currentStep += 1; 
        }

        /// <summary>
        /// Decreases the current step by 1
        /// Will fail if the current step is already at 0
        /// </summary>
        public void DecrementStep()
        {
            if (_currentStep > 0) _currentStep -= 1;
        }
    }
}