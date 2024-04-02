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

        // Start is called before the first frame update
        void Start()
        {
            _goalModel.GoalFinishedEvent += DestroyMe;
            transform.position = new(_goalModel.GridPosition.x, _goalModel.GridPosition.y);
            roboId.text = _goalModel.RoboId;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void DestroyMe([CanBeNull] object sender, EventArgs e)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}    