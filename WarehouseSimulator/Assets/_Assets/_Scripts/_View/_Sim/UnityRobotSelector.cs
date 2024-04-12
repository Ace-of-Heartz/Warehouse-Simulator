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
            SerializedObject so = new SerializedObject(m_unityRobot);
            robotPanel.Bind(so);
            robotPanel.Q("IDField").Q<IntegerField>().bindingPath = "ID";
        }
    }
}