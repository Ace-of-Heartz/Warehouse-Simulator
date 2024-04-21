﻿using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;


namespace WarehouseSimulator.Model
{
    [CreateAssetMenu(fileName = "ROBOT_DATA", menuName = "ROBOT_DATA", order = 0)]
    public class RobotData : ScriptableObject
    {
        public int m_id;
        public int m_shownId;
        public Vector2Int m_gridPosition;
        public Direction m_heading;
        [CanBeNull] public GoalLike m_goal;
        public RobotBeing m_state;
        
    }
}