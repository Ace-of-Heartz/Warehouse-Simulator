using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Sim;
using TMPro;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.PB;

namespace WarehouseSimulator.View
{
    public class UnityRobot : MonoBehaviour
    {
        #region Fields
        
        /// <summary>
        /// Holds the data of the robot
        /// </summary>
        private RobotData m_robotData;
        
        /// <summary>
        /// The model representation of the robot
        /// </summary>
        private RobotLike _roboModel;
        
        /// <summary>
        /// Reference to the text that shows the robot id
        /// </summary>
        [SerializeField]
        private TextMeshPro id;

        /// <summary>
        /// The visuals of the robot
        /// </summary>
        [SerializeField] private GameObject _texture;
        /// <summary>
        /// Reference to the map
        /// </summary>
        private UnityMap _mapie;
        
        /// <summary>
        /// Playback manager reference for animation speed
        /// </summary>
        private PlaybackManager _playbackManager;
        /// <summary>
        /// Simulation manager reference for animation speed
        /// </summary>
        private SimulationManager _simulationManager;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets the robot data
        /// </summary>
        public RobotData RobotData
        {
            get => m_robotData;
        }
        
        /// <summary>
        /// Check if the robot is a simulation robot
        /// </summary>
        public bool IsSimRobot 
        {
            get => _roboModel is SimRobot;
        }
        #endregion

        // Update is called once per frame
        void Update()
        {
            float animTime = GetAnimationFrameTime();
            float lerpProgress = Time.deltaTime * 4 * 1000 / animTime;
            
            //position
            Vector3 oldPos = transform.position; 
            Vector3 newPos = _mapie.GetWorldPosition(_roboModel.GridPosition);
            if (oldPos != newPos)
                transform.position = Vector3.Lerp(oldPos, newPos, lerpProgress);

            //rotation
            Direction newRot = _roboModel.Heading;
            float targetAngle = 0;
            switch(newRot) {
                case Direction.North:
                    targetAngle = 0;
                    break;
                case Direction.East:
                    targetAngle = -90;
                    break;
                case Direction.South:
                    targetAngle = 180;
                    break;
                case Direction.West:
                    targetAngle = 90;
                    break;
            }
            float interpolatedAngle = Mathf.LerpAngle(_texture.transform.eulerAngles.z, targetAngle, lerpProgress);
            _texture.transform.eulerAngles = new Vector3(0, 0, interpolatedAngle);
        }

        /// <summary>
        /// Initializes the fields
        /// </summary>
        /// <param name="dis">The model representation of the robot</param>
        /// <param name="dat">The reference to the map</param>
        /// <param name="pbMan">The playback manager reference, or null if we are not in playback</param>
        /// <param name="simMan">The simulation manager reference, or null if we are not in simulation</param>
        public void MyThingies(RobotLike dis, UnityMap dat, [CanBeNull] PlaybackManager pbMan, [CanBeNull] SimulationManager simMan)
        {
            _roboModel = dis;
            _mapie = dat;
            transform.position = _mapie.GetWorldPosition(_roboModel.GridPosition);
            id.text = _roboModel.ShownId.ToString();
            m_robotData = _roboModel.RobotData;
            _playbackManager = pbMan;
            _simulationManager = simMan;
        }

        /// <summary>
        /// Get the time in milliseconds for the animation
        /// </summary>
        /// <returns>Time that can be taken up by animation</returns>
        float GetAnimationFrameTime()
        {
            if (_simulationManager is not null)
            {
                return _simulationManager.SimulationData.m_stepTime;
            }
            
            if (_playbackManager is not null)
            {
                return PlaybackData.DEFAULT_PLAYBACK_TIME_MS / _playbackManager.PlaybackData.PlaybackSpeed;
            }
            return 1000; // 1 second is default
        }
    }
}    