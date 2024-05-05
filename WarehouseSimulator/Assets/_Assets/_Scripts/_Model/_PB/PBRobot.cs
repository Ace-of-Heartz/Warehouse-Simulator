using System;
using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.PB
{
    public class PbRobot : RobotLike
    {
        /// <summary>
        /// The history of the robot's position on the grid.
        /// The 0th element is the initial position.
        /// The ith element is the position after the i number of action.
        /// </summary>
        private Vector2Int[] _gridPositionHistory;
        /// <summary>
        /// The history of the robot's heading.
        /// The 0th element is the initial heading.
        /// The ith element is the heading after the i number of action.
        /// </summary>
        private Direction[] _headings;

        /// <summary>
        /// Constructor for the robot.
        /// </summary>
        /// <param name="i">The (0 based) id of the robot</param>
        /// <param name="position">The start position</param>
        /// <param name="stepNumber">The number of simulation steps</param>
        /// <param name="heading">The starting heading</param>
        public PbRobot(int i, Vector2Int position, int stepNumber,Direction heading = Direction.North, RobotBeing state = RobotBeing.Free) 
            : base (i,position,heading,state)
        {
            _gridPositionHistory = new Vector2Int[stepNumber + 1];
            _headings = new Direction[stepNumber + 1];
        }

        /// <summary>
        /// Sets the robot's position and heading tat the give moment.
        /// </summary>
        /// <param name="stateIndex">The current state's index</param>
        /// <exception cref="ArgumentException">Thrown when stateIndex is out of bound</exception>
        public void SetTimeTo(int stateIndex)
        {
            if (stateIndex > _gridPositionHistory.Length)
            {
                throw new ArgumentException($"Argument {nameof(stateIndex)}: stateIndex too high");
            }
            
            if (stateIndex < 0)
            {
                throw new ArgumentException($"Argument {nameof(stateIndex)}: stateIndex too low");
            }
            RobotData.m_gridPosition = _gridPositionHistory[stateIndex];
            RobotData.m_heading = _headings[stateIndex];
        }

        /// <summary>
        /// Calculates the robot's position and heading at each moment.
        /// </summary>
        /// <param name="actions">The actions taken by the robot at each step</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="actions"/> length is incorrect</exception>
        public void CalcTimeLine(List<RobotDoing> actions)
        {
            if (actions.Count + 1 != _gridPositionHistory.Length)
            {
                throw new ArgumentException($"Invalid number of actions ({actions.Count} instead of {_gridPositionHistory.Length}) in argument {nameof(actions)}");
            }
            int i = 0;
            _gridPositionHistory[i] = RobotData.m_gridPosition;
            _headings[i] = RobotData.m_heading;
            foreach (RobotDoing wattodo in actions)
            {
                ++i;
                switch (wattodo)
                {
                    case RobotDoing.Wait:
                    case RobotDoing.Timeout:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = _headings[i - 1];
                        break;
                    case RobotDoing.Forward:
                        _gridPositionHistory[i] = WhereToMove(_gridPositionHistory[i - 1], _headings[i - 1]);
                        _headings[i] = _headings[i - 1];
                        break;
                    case RobotDoing.Rotate90:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = (Direction)( (((int)_headings[i - 1] - 1)+4) % 4);
                        break;
                    case RobotDoing.RotateNeg90:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = (Direction)(((int)_headings[i - 1] + 1) % 4);
                        break;
                }
            }
        }
    }
}