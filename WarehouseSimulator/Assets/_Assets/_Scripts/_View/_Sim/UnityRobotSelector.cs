using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View.Sim
{
    public class UnityRobotSelector : MonoBehaviour
    {
        public UnityRobot m_unityRobot;


        public void SelectRobot()
        {
            Debug.Log("Oh no, someone pressed me!");
            var sceneHandler = SceneHandler.GetInstance();
            if (sceneHandler.CurrentSceneID == 0)
            {
                throw new IncorrectSceneException();
            }

            //Binding properties . . . .
            var robotPanel = sceneHandler.CurrentDoc.rootVisualElement.Q("RobotPanel");
            SerializedObject so = new SerializedObject(m_unityRobot.RobotData);
            robotPanel.Bind(so);
            
            SerializedProperty sp = so.FindProperty("m_id");
            robotPanel.Q("IDField").Q<IntegerField>().BindProperty(sp);

            sp = so.FindProperty("m_heading");
            robotPanel.Q("DirectionField").Q<EnumField>().BindProperty(sp);

            sp = so.FindProperty("m_state");
            robotPanel.Q("StateField").Q<EnumField>().BindProperty(sp);

            sp = so.FindProperty("m_gridPosition");
            robotPanel.Q("PositionField").Q<Vector2IntField>().BindProperty(sp);



        }
    }
}