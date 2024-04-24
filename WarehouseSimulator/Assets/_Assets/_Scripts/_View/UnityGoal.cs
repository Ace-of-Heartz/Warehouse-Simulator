using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Sim;
using TMPro;
using WarehouseSimulator.Model.PB;

namespace WarehouseSimulator.View
{
    public class UnityGoal : MonoBehaviour
    {
        private GoalLike _goalModel;
        [SerializeField]
        private TextMeshPro goalIdText;

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
            if (g is SimGoal simG)
            {
                simG.GoalFinishedEvent += (_,_) => Destroy(gameObject);
            }
            else if (g is PbGoal pbG)
            {
                pbG.jesusEvent += (_,_) => gameObject.SetActive(pbG.CurrentlyAlive);
            }
            transform.position = _mapie.GetWorldPosition(_goalModel.GridPosition);
            goalIdText.text = _goalModel.RoboId;
        }
    }
}    