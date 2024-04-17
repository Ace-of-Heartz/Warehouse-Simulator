using System;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model;

namespace WarehouseSimulator.Model.Sim
{
    public class Goal
    {
        private GoalData m_goalData;
        
        #region Properties

        public GoalData GoalData
        {
            get => m_goalData;
        }
        public int GoalID
        {
            get => m_goalData.m_id;
        }
        
        public string RoboId
        {
            get => m_goalData.m_robot?.Id.ToString() ?? "unknown";
        }
        
        public Vector2Int GridPosition
        {
            get => m_goalData.m_gridPosition;
        }

        public Robot Robot
        {
            get => m_goalData.m_robot;
        }

        #endregion
        
        [CanBeNull] public event EventHandler GoalFinishedEvent;
        
        public Goal(Vector2Int gPos,int id)
        {
            m_goalData = ScriptableObject.CreateInstance<GoalData>();
            m_goalData.m_id = id;
            m_goalData.m_gridPosition = gPos;
            

        }

        public void AssignedTo(Robot thisOne)
        {
            m_goalData.m_robot = thisOne;
            //TODO => Blaaa: Log later (ehhez kell a selfId)
        }

        public void FinishTask()
        {
            GoalFinishedEvent?.Invoke(this, EventArgs.Empty);
            //TODO => Blaaa: Log later (ehhez kell a selfId)
        }
    }
}

