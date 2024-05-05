using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class SimGoal : GoalLike
    {
        /// <summary>
        /// Invoked when the goal is finished
        /// </summary>
        [CanBeNull] public event EventHandler GoalFinishedEvent;
        
        /// <summary>
        /// Constructor for the SimGoal class
        /// </summary>
        /// <param name="gridPos">The position of the goal on the map</param>
        /// <param name="id">The id of the gaol</param>
        public SimGoal(Vector2Int gridPos,int id) 
                : base (id,gridPos) { }

        /// <summary>
        /// Tells the goal that a robot is assigned to it
        /// </summary>
        /// <param name="thisOne">The robot assigned to this task</param>
        public void AssignedTo(SimRobot thisOne)
        {
            GoalData.m_robot = thisOne;
        }

        /// <summary>
        /// Finishes the goal 
        /// </summary>
        public void FinishTask()
        {
            GoalFinishedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}

