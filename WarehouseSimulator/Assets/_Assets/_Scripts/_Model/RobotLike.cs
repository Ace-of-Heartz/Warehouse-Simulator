using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Model base class for robots.
    /// </summary>
    public abstract class RobotLike
    {
        #region Fields

        /// <summary>
        /// The robot data for the robot
        /// </summary>
        private RobotData m_robotData;
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Property for the robot data
        /// </summary>
        public RobotData RobotData => m_robotData;
        
        /// <summary>
        /// The id of the robot
        /// </summary>
        public int Id => m_robotData.m_id;
        
        /// <summary>
        /// The id of the robot shown in the UI.
        /// </summary>
        public int ShownId => m_robotData.m_shownId;

        /// <summary>
        /// The position of the robot
        /// </summary>
        public Vector2Int GridPosition => m_robotData.m_gridPosition;
    
        /// <summary>
        /// The heading of the robot
        /// </summary>
        public Direction Heading => m_robotData.m_heading;
        
        /// <summary>
        /// The goal of the robot if it has one.
        /// </summary>
        public GoalLike Goal
        {
            get => m_robotData.m_goal;
            protected set
            {
                m_robotData.m_state = value == null ? RobotBeing.Free : RobotBeing.InTask;
                m_robotData.m_goal = value;
            }
        }
        /// <summary>
        /// The state of the robot. (Free or InTask)
        /// </summary>
        public RobotBeing State => m_robotData.m_state;
        
        #endregion
        
        /// <summary>
        /// The constructor of the robot.
        /// </summary>
        /// <param name="i">The id</param>
        /// <param name="gPos">The initial position</param>
        /// <param name="heading">The initial heading</param>
        /// <param name="state">The initial state</param>
        /// <param name="goal">The goal assigned to the robot</param>
        protected RobotLike(int i, 
            Vector2Int gPos, 
            Direction heading = Direction.North, 
            RobotBeing state = RobotBeing.Free, 
            [CanBeNull] GoalLike goal = null)
        {
            m_robotData = ScriptableObject.CreateInstance<RobotData>();
            m_robotData.m_id = i;
            m_robotData.m_shownId = i + 1;
            m_robotData.m_gridPosition = gPos;
            m_robotData.m_heading = heading;
            m_robotData.m_state = state;
            m_robotData.m_goal = goal;
        }
        
        /// <summary>
        /// Get the position that is one step away from a position.
        /// </summary>
        /// <param name="pos">The position from which we do the calculation</param>
        /// <param name="heading">The heading in which direction we 'move'</param>
        /// <returns>The position one step away</returns>
        protected Vector2Int WhereToMove(Vector2Int pos, Direction heading)
        {
            switch (heading)
            {
                case(Direction.North):
                    return pos + Vector2Int.down; 
                case(Direction.West):
                    return pos + Vector2Int.left; 
                case(Direction.South):
                    return pos + Vector2Int.up;
                case(Direction.East):
                    return pos + Vector2Int.right;
                default: return pos;
            }
        }
    }
}