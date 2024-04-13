using System;
using JetBrains.Annotations;
using Unity.Properties;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class Robot
    {

        
        
        // private readonly int _id;
        // private Vector2Int _gridPosition;
        // private Direction _heading;
        // [CanBeNull] private Goal _goal;
        // private RobotBeing _state;

        private RobotData m_robotData;
        
        

        #region Properties

        public RobotData RobotData
        {
            get => m_robotData;
        }
            
        public int Id
        {
            get => m_robotData.m_id;
        }
        public Vector2Int GridPosition
        {
            get => m_robotData.m_gridPosition;
        }
        public Direction Heading
        {
            get => m_robotData.m_heading;
        }
        public Goal Goal
        {
            get => m_robotData.m_goal;
            private set
            {
                //TODO => Blaaa: Log later
                m_robotData.m_state = value == null ? RobotBeing.Free : RobotBeing.InTask;
                m_robotData.m_goal = value;
            }
        }
        public RobotBeing State
        {
            get => m_robotData.m_state;
        }
        #endregion

        public Robot(int i, Vector2Int gPos, Direction h = Direction.North, Goal g = null, RobotBeing s = RobotBeing.Free)
        {
            m_robotData = ScriptableObject.CreateInstance<RobotData>();
            m_robotData.m_id = i;
            m_robotData.m_gridPosition = gPos;
            m_robotData.m_heading = h;
            m_robotData.m_goal = g;
            m_robotData.m_state = s;
            
            // _id = i;
            // _gridPosition = gPos;
            // _heading = h;
            // _goal = g;
            // _state = s;
        }

        public void AssignGoal(Goal goTo)
        {
            goTo.AssignedTo(this);
            Goal = goTo;
            // _state = RobotBeing.InTask;
            m_robotData.m_state = RobotBeing.InTask;
        }


        public void PerformActionRequested(RobotDoing watt, Map mapie)
        {
            if (mapie == null) { throw new ArgumentNullException($"The argument: {nameof(mapie)} as the map does not exist"); }
            switch (watt)
            {
                case(RobotDoing.Timeout):
                case(RobotDoing.Wait):
                    break;
                case(RobotDoing.Forward):
                    Vector2Int nextPos = WhereToMove();
                    if (mapie.GetTileAt(nextPos) == TileType.Wall)
                    {
                        //TODO => Blaaa: CC react and LOG
                    } 
                    else if (mapie.GetTileAt(nextPos) == TileType.RoboOccupied)
                    {
                        //TODO => Blaaa: CC react and LOG
                    }
                    else
                    {
                        //TODO => Unity react
                        mapie.DeoccupyTile(m_robotData.m_gridPosition);
                        m_robotData.m_gridPosition = nextPos;
                        mapie.OccupyTile(m_robotData.m_gridPosition);
                        if (nextPos == m_robotData.m_goal?.GridPosition)
                        {
                            GoalCompleted();
                        }
                    }
                    break;
                case(RobotDoing.Rotate90):
                    m_robotData.m_heading = (Direction)( ((int)m_robotData.m_heading + 1) % 4 );
                    break;
                case(RobotDoing.RotateNeg90):
                    m_robotData.m_heading = (Direction)( ((int)m_robotData.m_heading - 1) % 4 );
                    break;
            }

            
            
        }
        
        private void GoalCompleted()
        {
            Goal?.FinishTask();
            Goal = null;
            m_robotData.m_state = RobotBeing.Free;
        }

        private Vector2Int WhereToMove()
        {
            switch (m_robotData.m_heading)
            {
                case(Direction.North):
                    return m_robotData.m_gridPosition + Vector2Int.down; 
                case(Direction.West):
                    return m_robotData.m_gridPosition + Vector2Int.left; 
                case(Direction.South):
                    return m_robotData.m_gridPosition + Vector2Int.up;
                case(Direction.East):
                    return m_robotData.m_gridPosition + Vector2Int.right;
                default: return new Vector2Int(0, 0);
            }
        }
    }
}
