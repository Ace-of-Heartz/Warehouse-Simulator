using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace WarehouseSimulator.Model.Sim
{
    public class Goal
    {
        private Vector2Int _gridPosition;
        [CanBeNull] private Robot _robot;

        #region Properties

        public Vector2Int GridPosition
        {
            get => _gridPosition;
        }

        public Robot Robot
        {
            get => _robot;
        }

        #endregion
        
        [CanBeNull] public event EventHandler GoalFinishedEvent;
        [CanBeNull] public event EventHandler GoalAssignedEvent;
        
        public Goal(Vector2Int gPos)
        {
            _gridPosition = gPos;
        }

        public void AssignedTo(Robot thisOne)
        {
            _robot = thisOne;
            GoalAssignedEvent?.Invoke(this, EventArgs.Empty);
            //TODO => Blaaa: Log later
        }

        public void FinishTask()
        {
            GoalFinishedEvent?.Invoke(this, EventArgs.Empty);
            //TODO => Blaaa: Log later
        }
    }
}

