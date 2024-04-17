using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Sim;
using TMPro;

namespace WarehouseSimulator.View
{
    public class UnityGoal : MonoBehaviour
    {
        private GoalLike _goalModel;
        [SerializeField]
        private TextMeshPro roboId;

        private UnityMap _mapie;

        // Start is called before the first frame update
        void Start()
        {
            //Do we even need this blaaa?
        }

        public void GiveGoalModel(SimGoal g, UnityMap dis)
        {
            _goalModel = g;
            _mapie = dis;
            g.GoalFinishedEvent += (_,_) => Destroy(gameObject);
            transform.position = _mapie.GetWorldPosition(_goalModel.GridPosition);
            roboId.text = _goalModel.RoboId;
        }
    }
}    