using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Goal data for the goals in the warehouse.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "GOAL_DATA", menuName = "GOAL_DATA", order = 1)]
    public class GoalData : ScriptableObject
    {
        /// <summary>
        /// The id of the goal.
        /// </summary>
        public int m_id;
        /// <summary>
        /// The position of the goal.
        /// </summary>
        public Vector2Int m_gridPosition;
        /// <summary>
        /// The robot that is assigned to the goal. Null if not relevant.
        /// </summary>
        [CanBeNull] public RobotLike m_robot;
    }
}