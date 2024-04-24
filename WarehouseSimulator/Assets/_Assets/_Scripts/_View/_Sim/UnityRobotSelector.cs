using System;

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

            GameObject.Find("RobotDisplayManager").GetComponent<UnityRobotDisplayer>().SetRobot(m_unityRobot);
            
            
            
            
            



        }
    }
}