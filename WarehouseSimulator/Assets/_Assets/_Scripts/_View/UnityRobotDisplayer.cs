using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class UnityRobotDisplayer : MonoBehaviour
    {
        #region Fields
        private UnityRobot _selectedRobot;
        private VisualElement _robotDisplay;
        #endregion 
        
        #region Unity Methods
        /// <summary>
        ///     Initializes the robot display visual element.
        /// </summary>
        private void Start()
        {
            _robotDisplay = SceneHandler.GetInstance().CurrentDoc.rootVisualElement.Q("RobotPanel");
        }
        
        /// <summary>
        ///     Updates the robot display visual element.
        /// </summary>
        private void Update()
        {
            //Updating properties . . . .

            if (_selectedRobot == null)
            {
                return;
            }
      
            _robotDisplay.Q<EnumField>("DirectionField").value = _selectedRobot.RobotData.m_heading;
            
            if (_selectedRobot.IsSimRobot)
            {
                
                _robotDisplay.Q<EnumField>("StateField").value = _selectedRobot.RobotData.m_state;
            }
            
            
            _robotDisplay.Q("PositionField").Q<Vector2IntField>().value = _selectedRobot.RobotData.m_gridPosition;
            
            if (_selectedRobot.RobotData.m_goal == null)
            {
                _robotDisplay.Q("GoalDisplay").style.display = DisplayStyle.None;
            }
            else 
            {
                _robotDisplay.Q("GoalDisplay").style.display = DisplayStyle.Flex ;
            
                _robotDisplay.Q("GoalDisplay").Q("GoalPanel").Q<IntegerField>("IDField").value = _selectedRobot.RobotData.m_goal.GoalID;

                
                _robotDisplay.Q("GoalDisplay").Q("GoalPanel").Q<Vector2IntField>().value = _selectedRobot.RobotData.m_goal.GridPosition;
            }
        }
        #endregion
        
        #region C# Methods
        /// <summary>
        ///     Gets the currently selected robot.
        /// </summary>
        public UnityRobot SelectedRobot
        {
            get => _selectedRobot;
        }

        /// <summary>
        ///     Sets the robot to be displayed and sets the one time info.
        /// </summary>
        /// <param name="robot"></param>
        public void SetRobot(UnityRobot robot)
        {
            _selectedRobot = robot;
            
            SetOneTimeInfo();
        }

        /// <summary>
        ///     Sets the one time info for the robot and settings for the display.
        /// </summary>
        private void SetOneTimeInfo()
        {
            _robotDisplay.Q<IntegerField>("IDField").value = _selectedRobot.RobotData.m_shownId;
            
            if (_selectedRobot.IsSimRobot)
            {
                _robotDisplay.Q("StateField").style.display = DisplayStyle.Flex;
            }
            else
            {
                _robotDisplay.Q("StateField").style.display = DisplayStyle.None;   
            }
        }
        #endregion
        
        
    }
}