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

namespace WarehouseSimulator.View.Sim
{
    public class UnityRobot : MonoBehaviour
    {
        #region Fields
        
        private RobotData m_robotData;
        
        private Robot _roboModel;
        
        [SerializeField]
        private TextMeshPro id;

        private UnityMap _mapie;

        #endregion
        
        #region Properties

        public RobotData RobotData
        {
            get => m_robotData;
        }
        #endregion
        
        // Start is called before the first frame update
        void Start()
        {
            transform.position = _mapie.GetWorldPosition(_roboModel.GridPosition);
            id.text = _roboModel.Id.ToString();
            m_robotData = _roboModel.RobotData;
        }

        // Update is called once per frame
        void Update()
        {
            
            Vector3 oldPos = transform.position; 
            Vector3 newPos = _mapie.GetWorldPosition(_roboModel.GridPosition);
            if (oldPos != newPos)
            {
                transform.position = Vector3.Lerp(oldPos, newPos, Time.deltaTime * 5);
            }

            Direction newRot = _roboModel.Heading;
            switch (newRot)
            {
                case Direction.North:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.East:
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case Direction.South:
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Direction.West:
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }

            id.transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        
        

        public void MyThingies(Robot dis, UnityMap dat)
        {
            _roboModel = dis;
            _mapie = dat;
        }

    
    }
}    