using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    [CreateAssetMenu(fileName = "ROBOT_DATA", menuName = "ROBOT_DATA", order = 0)]
    public class RobotData : ScriptableObject
    {
        /// <summary>
        /// The id of the robot.
        /// </summary>
        public int m_id;
        /// <summary>
        /// The id of the robot shown in the UI.
        /// </summary>
        public int m_shownId;
        /// <summary>
        /// The position of the robot
        /// </summary>
        public Vector2Int m_gridPosition;
        /// <summary>
        /// The heading of the robot
        /// </summary>
        public Direction m_heading;
        /// <summary>
        /// The goal of the robot if it has one.
        /// </summary>
        [CanBeNull] public GoalLike m_goal;
        /// <summary>
        /// The state of the robot. (Free or InTask)
        /// </summary>
        public RobotBeing m_state;
        
    }
}