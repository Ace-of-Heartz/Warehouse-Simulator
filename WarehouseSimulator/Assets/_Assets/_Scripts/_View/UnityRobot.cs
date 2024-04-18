using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Sim;
using TMPro;
using Unity.Properties;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;

namespace WarehouseSimulator.View
{
    public class UnityRobot : MonoBehaviour
    {
        #region Fields
        
        private RobotData m_robotData;
        
        private RobotLike _roboModel;
        
        [SerializeField]
        private TextMeshPro id;

        private float _speed;

        [SerializeField] private GameObject _texture;
        private UnityMap _mapie;

        #endregion
        
        #region Properties

        public RobotData RobotData
        {
            get => m_robotData;
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

        public void MyThingies(SimRobot dis, UnityMap dat, float speedMultiplier)
        {
            _roboModel = dis;
            _mapie = dat;
            _speed = speedMultiplier;
            transform.position = _mapie.GetWorldPosition(_roboModel.GridPosition);
            id.text = _roboModel.Id.ToString();
            m_robotData = _roboModel.RobotData;
        }
    }
}    