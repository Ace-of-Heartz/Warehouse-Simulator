using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class Goal
    {
        private Vector2Int _gridPosition;
        [CanBeNull] private Robot _robot;
        private int _selfId;

        #region Properties

        public string RoboId
        {
            get => _robot?.Id.ToString() ?? "unknown";
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
        
        public Goal(Vector2Int gPos,int id)
        {
            _gridPosition = gPos;
            _selfId = id;
        }

        public void AssignedTo(Robot thisOne)
        {
            _robot = thisOne;
            //TODO => Blaaa: Log later (ehhez kell a selfId)
        }

        public void FinishTask()
        {
            GoalFinishedEvent?.Invoke(this, EventArgs.Empty);
            //TODO => Blaaa: Log later (ehhez kell a selfId)
        }
    }
}

