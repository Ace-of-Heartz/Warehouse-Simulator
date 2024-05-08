#nullable enable
using System;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// Model representation of a robot in the simulation
    /// </summary>
    public class SimRobot : RobotLike
    {
        /// <summary>
        /// The next state of the robot proposed by the planner
        /// <remarks>
        /// This tuple is used to store the next state of the robot as proposed by the planner.
        /// - nextPos: The next position of the robot on the grid.
        /// - nextHeading: The next heading direction of the robot.
        /// - what: The next action the robot is supposed to perform.
        /// </remarks>
        /// </summary>
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

        /// <summary>
        /// Gets the next proposed position of the robot
        /// </summary>
        public Vector2Int NextPos => _nexties.nextPos;
        
        /// <summary>
        /// Assigns a goal to the robot
        /// </summary>
        /// <param name="goTo">The goal assigned</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="goTo"/> is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if the robot is not free of tasks</exception>
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
        
        /// <summary>
        /// Tries to perform the action requested by the planner, if successful, the <see cref="_nexties"/> will store this proposed location
        /// The current state is not affected by this method.
        /// We check only for collisions with walls in this method.
        /// </summary>
        /// <param name="watt">The action requested</param>
        /// <param name="mapie">The map on which the simulation is running</param>
        /// <returns>Tuple:
        /// - bool: True if proposed action is OK, false if not
        /// - SimRobot: null if the bool is True. This robot if the proposed action is not OK
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapie"/> is null</exception>
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

        /// <summary>
        /// Transitions the robot to the next state stored in <see cref="_nexties"/>
        /// </summary>
        /// <param name="mipieMap">The map on which the simulation is running</param>
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
        
        /// <summary>
        /// The goal assigned to the robot is completed
        /// </summary>
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
