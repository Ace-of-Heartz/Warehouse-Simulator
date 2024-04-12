using System;
using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.PB
{
    public class PbRobot
    {
        private readonly int _id;
        private Vector2Int[] _gridPositionHistory;
        private Direction[] _headings;
        private RobotBeing[] _states;
        private Vector2Int _currentPos;
        private Direction _currentHeading;
        private RobotBeing _currState;
        
        #region
        public int Id
        {
            get => _id;
        }
        public Vector2Int GridPosition
        {
            get => _currentPos;
        }
        public Direction Heading
        {
            get => _currentHeading;
        }

        public RobotBeing State
        {
            get => _currState;
        }
        
        #endregion

        public PbRobot(int i,Vector2Int cP,int stepNumber,Direction cH = Direction.North,RobotBeing cS = RobotBeing.Free)
        {
            _id = i;
            _currentPos = cP;
            _currentHeading = cH;
            _currState = cS;
            _gridPositionHistory = new Vector2Int[stepNumber];
            _headings = new Direction[stepNumber];
            _states = new RobotBeing[stepNumber];
        }

        public void SetTimeTo(int step)
        {
            if (step > _gridPositionHistory.Length)
            {
                throw new ArgumentException($"Argument {nameof(step)}: stepnumber too high");
            }
            
            if (step < 0)
            {
                throw new ArgumentException($"Argument {nameof(step)}: stepnumber too low");
            }
            _currentPos = _gridPositionHistory[step];
            _currentHeading = _headings[step];
            _currState = _states[step];
        }

        public void CalcTimeLine(List<RobotDoing> actions)
        {
            if (actions.Count + 1 != _gridPositionHistory.Length)
            {
                throw new ArgumentException($"Invalid number of actions ({actions.Count} instead of {_gridPositionHistory.Length}) in argument {nameof(actions)}");
            }
            int i = 0;
            _gridPositionHistory[i] = _currentPos;
            _headings[i] = _currentHeading;
            _states[i] = _currState;
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
                        _gridPositionHistory[i] = WhereToMove(_gridPositionHistory[i - 1]);
                        _headings[i] = _headings[i - 1];
                        _states[i] = _states[i - 1];
                        break;
                    case RobotDoing.Rotate90:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = (Direction)( ((int)_headings[i - 1] + 1) % 4 );
                        _states[i] = _states[i - 1];
                        break;
                    case RobotDoing.RotateNeg90:
                        _gridPositionHistory[i] = _gridPositionHistory[i - 1];
                        _headings[i] = (Direction)( ((int)_headings[i - 1] - 1) % 4 );
                        _states[i] = _states[i - 1];
                        break;
                }
            }
        }
        
        private Vector2Int WhereToMove(Vector2Int pos)
        {
            switch (_currentHeading)
            {
                case(Direction.North):
                    return pos + Vector2Int.down; 
                case(Direction.West):
                    return pos + Vector2Int.left; 
                case(Direction.South):
                    return pos + Vector2Int.up;
                case(Direction.East):
                    return pos + Vector2Int.right;
                default: return new Vector2Int(0, 0);
            }
        }
    }
}