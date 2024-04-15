using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class SimGoal : GoalLike
    {
        [CanBeNull] public event EventHandler GoalFinishedEvent;
        
        public SimGoal(Vector2Int gridPos,int id) 
                : base (id,gridPos) { }

        public void AssignedTo(SimRobot thisOne)
        {
            GoalData.m_robot = thisOne;
            //TODO => Blaaa: Log later (ehhez kell a selfId)
        }

        public void FinishTask()
        {
            GoalFinishedEvent?.Invoke(this, EventArgs.Empty);
            //TODO => Blaaa: Log later (ehhez kell a selfId)
        }
    }
}

