using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model
{
    public abstract class GoalLike
    {
        private GoalData m_goalData;
        
        #region Properties

        public GoalData GoalData => m_goalData;
        
        public int GoalID => m_goalData.m_id;
        
        public string RoboId => m_goalData.m_robot?.ShownId.ToString() ?? "unknown";
        
        public Vector2Int GridPosition => m_goalData.m_gridPosition;

        public RobotLike SimRobot => m_goalData.m_robot;

        #endregion

        public GoalLike(int id, Vector2Int gridPosition, [CanBeNull] RobotLike robo = null)
        {
            m_goalData = ScriptableObject.CreateInstance<GoalData>();
            m_goalData.m_id = id;
            m_goalData.m_gridPosition = gridPosition;
            m_goalData.m_robot = robo;
        }
    }
}