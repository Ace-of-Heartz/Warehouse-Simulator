using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;

namespace WarehouseSimulator.View.Sim
{
    public class UnityRobotSelector : MonoBehaviour
    {
        public UnityRobot m_unityRobot;


        public void SelectRobot()
        {
            // Debug.Log($"Kerfus-{m_unityRobot.RobotData.m_id} reporting for duty!");
            var sceneHandler = SceneHandler.GetInstance();
            if (sceneHandler.CurrentSceneID == 0)
            {
                throw new IncorrectSceneException();
            }

            //Binding properties . . . .
            var robotPanel = sceneHandler.CurrentDoc.rootVisualElement.Q("RobotPanel");
            SerializedObject so = new SerializedObject(m_unityRobot.RobotData);
            robotPanel.Bind(so);
            
            SerializedProperty sp = so.FindProperty("m_shownId");
            robotPanel.Q("IDField").Q<IntegerField>("IDField").BindProperty(sp);

            sp = so.FindProperty("m_heading");
            robotPanel.Q("DirectionField").Q<EnumField>().BindProperty(sp);

            if (m_unityRobot.IsSimRobot)
            {
                sp = so.FindProperty("m_state");
                robotPanel.Q("StateField").Q<EnumField>().BindProperty(sp);
            }
            else
            {
                robotPanel.Q("StateField").style.display = DisplayStyle.None;   
            }
            

            sp = so.FindProperty("m_gridPosition");
            robotPanel.Q("PositionField").Q<Vector2IntField>().BindProperty(sp);

            if (m_unityRobot.RobotData.m_goal == null)
            {
                robotPanel.Q("GoalDisplay").style.display = DisplayStyle.None;
            }
            else
            {
                robotPanel.Q("GoalDisplay").style.display = DisplayStyle.Flex ;
                so = new SerializedObject(m_unityRobot.RobotData.m_goal.GoalData);

                sp = so.FindProperty("m_id");
                robotPanel.Q("GoalDisplay").Q("GoalPanel").Q<IntegerField>("IDField").BindProperty(sp);

                sp = so.FindProperty("m_gridPosition");
                robotPanel.Q("GoalDisplay").Q("GoalPanel").Q<Vector2IntField>().BindProperty(sp);
            }
            



        }
    }
}