#nullable enable
using System;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;

namespace WarehouseSimulator.Model.Sim
{
    public class SimRobot : RobotLike
    {
        private Vector2Int _nextPos;

        /// <summary>
        /// Constructor for SimRobot
        /// </summary>
        /// <param name="i">ID of robot</param>
        /// <param name="gridPos">Initial position of robot</param>
        /// <param name="heading">Initial direction of robot</param>
        /// <param name="goal">Goal assigned to robot</param>
        /// <param name="state">Initial state of robot</param>
        public SimRobot(int i,
            Vector2Int gridPos,
            Direction heading = Direction.North,
            SimGoal? goal = null,
            RobotBeing state = RobotBeing.Free)
                : base(i, gridPos, heading, state, goal)
        {
            _nextPos = new(-1, -1);
        }

        public Vector2Int NextPos => _nextPos;
        
        public void AssignGoal(SimGoal? goTo)
        {
            if (goTo is null)
            {
                #if DEBUG
                    throw new ArgumentException("The robot assigned cannot be null or PBRobot!");
                #endif
            }
            else
            {
                if (State == RobotBeing.Free)
                {
                    goTo.AssignedTo(this);
                    Goal = goTo;
                }
                else
                {
                    #if DEBUG
                        throw new InvalidOperationException($"The robot {Id} is already doing a task, you cannot assign another goal to it");
                    #endif
                }
            }
            
        }

        public (bool,SimRobot?) TryPerformActionRequested(RobotDoing watt, Map mapie)
        {
            _nextPos = RobotData.m_gridPosition;
            if (mapie == null)
            {
                throw new ArgumentNullException($"The argument: {nameof(mapie)} as the map does not exist");
            }

            switch (watt)
            {
                case (RobotDoing.Timeout):
                case (RobotDoing.Wait):
                    break;
                case (RobotDoing.Forward):
                    _nextPos = WhereToMove(RobotData.m_gridPosition);
                    if (mapie.GetTileAt(_nextPos) == TileType.Wall)
                    {
                        //TODO => Blaaa: CC react and LOG
                        return (false, this);
                    } 
                    break;
                case (RobotDoing.Rotate90):
                    RobotData.m_heading = (Direction)( (((int)RobotData.m_heading - 1)+4) % 4);
                    break;
                case (RobotDoing.RotateNeg90):
                    RobotData.m_heading = (Direction)(((int)RobotData.m_heading + 1) % 4);
                    break;
            }

            return (true,null);
        }

        public void MakeStep(Map mipieMap)
        {
            mipieMap.DeoccupyTile(RobotData.m_gridPosition);
            RobotData.m_gridPosition = _nextPos;
            mipieMap.OccupyTile(RobotData.m_gridPosition);
            if (_nextPos == Goal?.GridPosition)
            {
                GoalCompleted();
            }
        }
        
        private void GoalCompleted()
        {
            if (Goal is SimGoal simgolie)
            { 
                simgolie.FinishTask();
                Goal = null;
            }
        }
    }
}
