using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Sim;
using TMPro;

namespace WarehouseSimulator.View.Sim
{
    public class UnityGoal : MonoBehaviour
    {

        private Goal _goalModel;
        [SerializeField]
        private TextMeshPro roboId;

        private UnityMap _mapie;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = _mapie.GetWorldPosition(_goalModel.GridPosition);
            roboId.text = _goalModel.RoboId;
        }

        private void DestroyMe(object sender, EventArgs e)
        {
            Destroy(gameObject);
        }

        public void GiveGoalModel(Goal g, UnityMap dis)
        {
            _goalModel = g;
            _mapie = dis;
            _goalModel.GoalFinishedEvent += DestroyMe;
        }
    }
}    