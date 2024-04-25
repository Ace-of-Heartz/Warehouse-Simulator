#nullable enable
using System;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model;

namespace WarehouseSimulator.Model.Sim
{
    public class SimRobot : RobotLike
    {
        private (Vector2Int nextPos, Direction nextHeading,RobotDoing what) _nexties;

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
            _nexties = (new(-1, -1),nextHeading: Direction.North,RobotDoing.Wait);
        }

        public Vector2Int NextPos => _nexties.nextPos;
        
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
                    CustomLog.Instance.AddTaskEvent(Id, goTo.GoalID, "assigned");
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
            CustomLog.Instance.AddPlannerAction(Id,watt);
            if (watt == RobotDoing.Timeout)
            {
                watt = RobotDoing.Wait;
            }
            
            _nexties= (GridPosition,RobotData.m_heading,watt);
            if (mapie == null)
            {
                throw new ArgumentNullException($"The argument: {nameof(mapie)} as the map does not exist");
            }

            switch (watt)
            {
                case (RobotDoing.Wait):
                    break;
                case (RobotDoing.Forward):
                    _nexties.nextPos = WhereToMove(RobotData.m_gridPosition, RobotData.m_heading);
                    if (mapie.GetTileAt(_nexties.nextPos) == TileType.Wall)
                    {
                        CustomLog.Instance.AddError(Id,-1);
                        return (false, this);
                    } 
                    break;
                case (RobotDoing.Rotate90):
                    _nexties.nextHeading = (Direction)( (((int)RobotData.m_heading - 1)+4) % 4);
                    break;
                case (RobotDoing.RotateNeg90):
                    _nexties.nextHeading = (Direction)(((int)RobotData.m_heading + 1) % 4);
                    break;
            }

            return (true,null);
        }

        public void MakeStep(Map mipieMap)
        {
            CustomLog.Instance.AddRobotAction(Id,_nexties.what);
            if (_nexties.nextPos != RobotData.m_gridPosition)
            {
                mipieMap.DeoccupyTile(RobotData.m_gridPosition);
                RobotData.m_gridPosition = _nexties.nextPos;
                mipieMap.OccupyTile(RobotData.m_gridPosition);
                if (_nexties.nextPos == Goal?.GridPosition)
                {
                    GoalCompleted();
                }
            }
            RobotData.m_heading = _nexties.nextHeading;
        }
        
        private void GoalCompleted()
        {
            if (Goal is SimGoal simgolie)
            { 
                simgolie.FinishTask();
                CustomLog.Instance.AddTaskEvent(Id, Goal.GoalID, "finished");
                Goal = null;
            }
        }
    }
}
