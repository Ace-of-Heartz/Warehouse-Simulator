using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    public abstract class RobotLike
    {
        #region Fields

        private RobotData m_robotData;
        
        #endregion
        
        #region Properties

        public RobotData RobotData => m_robotData;
        
        public int Id => m_robotData.m_id;
        
        public int ShownId => m_robotData.m_shownId;

        public Vector2Int GridPosition => m_robotData.m_gridPosition;
    
        public Direction Heading => m_robotData.m_heading;
        
        public GoalLike Goal
        {
            get => m_robotData.m_goal;
            protected set
            {
                m_robotData.m_state = value == null ? RobotBeing.Free : RobotBeing.InTask;
                m_robotData.m_goal = value;
            }
        }
        public RobotBeing State => m_robotData.m_state;
        
        #endregion
        
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
        
        protected Vector2Int WhereToMove(Vector2Int pos)
        {
            switch (m_robotData.m_heading)
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