using UnityEngine;

namespace WarehouseSimulator.View.Sim
{
    public class UnityRobotSelector : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The UnityRobot that this selector is responsible for
        /// </summary>
        public UnityRobot m_unityRobot;
        #endregion
        
        #region Methods
        /// <summary>
        /// Selects the robot and displays it in the robot display panel
        /// </summary>
        /// <exception cref="IncorrectSceneException"></exception>
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
        #endregion
    }
}