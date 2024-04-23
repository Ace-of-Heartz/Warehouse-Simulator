using System;
using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.PB
{
    public class PbRobot : RobotLike
    {
        private Vector2Int[] _gridPositionHistory;
        private Direction[] _headings;
        private RobotBeing[] _states;

        public PbRobot(int i, Vector2Int position, int stepNumber,Direction heading = Direction.North, RobotBeing state = RobotBeing.Free) 
            : base (i,position,heading,state)
        {
            _gridPositionHistory = new Vector2Int[stepNumber + 1];
            _headings = new Direction[stepNumber + 1];
            _states = new RobotBeing[stepNumber + 1];
        }

        public void SetTimeTo(int stateIndex)
        {
            if (stateIndex > _gridPositionHistory.Length)
            {
                throw new ArgumentException($"Argument {nameof(stateIndex)}: stepnumber too high");
            }
            
            if (stateIndex < 0)
            {
                throw new ArgumentException($"Argument {nameof(stateIndex)}: stepnumber too low");
            }
            RobotData.m_gridPosition = _gridPositionHistory[stateIndex];
            RobotData.m_heading = _headings[stateIndex];
            RobotData.m_state = _states[stateIndex];
        }

        public void CalcTimeLine(List<RobotDoing> actions)
        {
            if (actions.Count + 1 != _gridPositionHistory.Length)
            {
                throw new ArgumentException($"Invalid number of actions ({actions.Count} instead of {_gridPositionHistory.Length}) in argument {nameof(actions)}");
            }
            int i = 0;
            _gridPositionHistory[i] = RobotData.m_gridPosition;
            _headings[i] = RobotData.m_heading;
            _states[i] = RobotData.m_state;
            foreach (RobotDoing wattodo in actions)
            {
                ++i;
                switch (wattodo)
                {
                    case RobotDoing.Wait:
                    case RobotDoing.Timeout:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = _headings[i - 1];
                        _states[i] = _states[i - 1];
                        break;
                    case RobotDoing.Forward:
                        _gridPositionHistory[i] = WhereToMove(_gridPositionHistory[i - 1], _headings[i - 1]);
                        _headings[i] = _headings[i - 1];
                        _states[i] = _states[i - 1];
                        break;
                    case RobotDoing.Rotate90:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = (Direction)( (((int)_headings[i - 1] - 1)+4) % 4);
                        _states[i] = _states[i - 1];
                        break;
                    case RobotDoing.RotateNeg90:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = (Direction)(((int)_headings[i - 1] + 1) % 4);
                        _states[i] = _states[i - 1];
                        break;
                }
            }
        }
    }
}