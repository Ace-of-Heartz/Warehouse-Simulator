using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model
{
    [Serializable]
    [CreateAssetMenu(fileName = "GOAL_DATA", menuName = "GOAL_DATA", order = 1)]
    public class GoalData : ScriptableObject
    {
        public int m_id;
        public Vector2Int m_gridPosition;
        [CanBeNull] public RobotLike m_robot;
    }
}