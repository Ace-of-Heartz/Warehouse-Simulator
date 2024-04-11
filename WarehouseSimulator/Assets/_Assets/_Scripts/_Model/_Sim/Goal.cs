using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class Goal
    {
        private Vector2Int _gridPosition;
        [CanBeNull] private Robot _robot;

        #region Properties

        public string RoboId
        {
            get => _robot.Id.ToString();
        }
        
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

