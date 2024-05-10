using UnityEngine;
using WarehouseSimulator.Model.Sim;
using TMPro;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;

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
        /// The animation speed of the robot
        /// </summary>
        private float _speed;

        /// <summary>
        /// The visuals of the robot
        /// </summary>
        [SerializeField] private GameObject _texture;
        /// <summary>
        /// Reference to the map
        /// </summary>
        private UnityMap _mapie;

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
            Vector3 oldPos = transform.position; 
            Vector3 newPos = _mapie.GetWorldPosition(_roboModel.GridPosition);
            //if (oldPos != newPos) transform.position = Vector3.Lerp(oldPos, newPos, Time.deltaTime * _speed);
            if (oldPos != newPos) transform.position = newPos;

            Direction newRot = _roboModel.Heading;
            switch (newRot)
            {
                case Direction.North:
                    _texture.transform.rotation = Quaternion.Euler(0, 0, 0);

                    break;
                case Direction.East:
                    _texture.transform.rotation = Quaternion.Euler(0, 0, -90);

                    break;
                case Direction.South:
                    _texture.transform.rotation = Quaternion.Euler(0, 0, 180);

                    break;
                case Direction.West:
                    _texture.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
        }

        /// <summary>
        /// Initializes the fields
        /// </summary>
        /// <param name="dis">The model representation of the robot</param>
        /// <param name="dat">The reference to the map</param>
        /// <param name="speedMultiplier">The animaiton speed</param>
        public void MyThingies(RobotLike dis, UnityMap dat, float speedMultiplier)
        {
            _roboModel = dis;
            _mapie = dat;
            _speed = speedMultiplier;
            transform.position = _mapie.GetWorldPosition(_roboModel.GridPosition);
            id.text = _roboModel.ShownId.ToString();
            m_robotData = _roboModel.RobotData;
        }
    }
}    