using System;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Sim;
using TMPro;
using WarehouseSimulator.Model.PB;

namespace WarehouseSimulator.View
{
    public class UnityGoal : MonoBehaviour
    {
        /// <summary>
        /// The goal model that is being represented by this object
        /// </summary>
        private GoalLike _goalModel;
        /// <summary>
        /// The text that shows the goal id
        /// </summary>
        [SerializeField]
        private TextMeshPro goalIdText;

        /// <summary>
        /// Reference to the map
        /// </summary>
        private UnityMap _mapie;

        /// <summary>
        /// Initializes the fields
        /// </summary>
        /// <param name="g">A Goal, either SimGoal or PbGoal</param>
        /// <param name="dis">Map from the view</param>
        public void GiveGoalModel(GoalLike g, UnityMap dis) 
        {
            _goalModel = g;
            _mapie = dis;
            transform.position = _mapie.GetWorldPosition(_goalModel.GridPosition);
            goalIdText.text = _goalModel.RoboId;
            if (g is SimGoal simG)
            {
                simG.GoalFinishedEvent += simGOnGoalFinishedEvent();
            }
            else if (g is PbGoal pbG)
            {
                pbG.JesusEvent += pbGOnJesusEvent();
                goalIdText.text = pbG.RoboId;
            }
        }

        private void OnDestroy()
        {
            if (_goalModel is SimGoal simG)
            {
                simG.GoalFinishedEvent -= simGOnGoalFinishedEvent();
            }
            else if (_goalModel is PbGoal pbG)
            {
                pbG.JesusEvent -= pbGOnJesusEvent();
                goalIdText.text = pbG.RoboId;
            }
        }

        private EventHandler simGOnGoalFinishedEvent()
        {
            return (_,_) => Destroy(gameObject);
        }

        private EventHandler<bool> pbGOnJesusEvent()
        {
            return (_,isAlive) => gameObject.SetActive(isAlive);
        }
    }
}    