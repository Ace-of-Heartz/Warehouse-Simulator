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
        private GoalLike _simGoalModel;
        [SerializeField]
        private TextMeshPro roboId;

        private UnityMap _mapie;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = _mapie.GetWorldPosition(_simGoalModel.GridPosition);
            roboId.text = _simGoalModel.RoboId;
        }

        public void GiveGoalModel(SimGoal g, UnityMap dis)
        {
            _simGoalModel = g;
            _mapie = dis;
            g.GoalFinishedEvent += (_,_) => Destroy(gameObject);
        }
    }
}    