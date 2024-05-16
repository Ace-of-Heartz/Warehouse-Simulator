using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Model base class for goals.
    /// </summary>
    public abstract class GoalLike
    {
        /// <summary>
        /// The goal data for the goal
        /// </summary>
        private GoalData m_goalData;
        
        #region Properties

        /// <summary>
        /// Property for the goal data
        /// </summary>
        public GoalData GoalData => m_goalData;
        
        /// <summary>
        /// The ID of the goal.
        /// </summary>
        public int GoalID => m_goalData.m_id;
        
        /// <summary>
        /// The shown ID of the robot corresponding to the goal.
        /// </summary>
        public string RoboId => m_goalData.m_robot?.ShownId.ToString() ?? "unknown";
        
        /// <summary>
        /// The grid position of the goal.
        /// </summary>
        public Vector2Int GridPosition => m_goalData.m_gridPosition;

        /// <summary>
        /// The robot corresponding to the goal.
        /// </summary>
        public RobotLike Robot => m_goalData.m_robot;

        #endregion

        /// <summary>
        /// Yes it is a constructor.
        /// </summary>
        /// <param name="id">The id of the goal</param>
        /// <param name="gridPosition">The position of the goal</param>
        /// <param name="robo">The robot assigned to the goal</param>
        public GoalLike(int id, Vector2Int gridPosition, [CanBeNull] RobotLike robo = null)
        {
            m_goalData = ScriptableObject.CreateInstance<GoalData>();
            m_goalData.m_id = id;
            m_goalData.m_gridPosition = gridPosition;
            m_goalData.m_robot = robo;
        }
    }
}