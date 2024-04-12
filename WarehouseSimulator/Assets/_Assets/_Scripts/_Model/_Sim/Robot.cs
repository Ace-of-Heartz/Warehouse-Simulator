using System;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class Robot
    {
        private readonly int _id;
        private Vector2Int _gridPosition;
        private Direction _heading;
        [CanBeNull] private Goal _goal;
        private RobotBeing _state;

        #region Properties
        public int Id
        {
            get => _id;
        }
        public Vector2Int GridPosition
        {
            get => _gridPosition;
        }
        public Direction Heading
        {
            get => _heading;
        }
        public Goal Goal
        {
            get => _goal;
            private set
            {
                //TODO => Blaaa: Log later
                _state = value == null ? RobotBeing.Free : RobotBeing.InTask;
                _goal = value;
            }
        }

        public RobotBeing State
        {
            get => _state;
        }
        #endregion

        public Robot(int i, Vector2Int gPos, Direction h = Direction.North, Goal g = null, RobotBeing s = RobotBeing.Free)
        {
            _id = i;
            _gridPosition = gPos;
            _heading = h;
            _goal = g;
            _state = s;
        }

        public void AssignGoal(Goal goTo)
        {
            goTo.AssignedTo(this);
            Goal = goTo;
            _state = RobotBeing.InTask;
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
                        mapie.DeoccupyTile(_gridPosition);
                        _gridPosition = nextPos;
                        mapie.OccupyTile(_gridPosition);
                        if (nextPos == _goal?.GridPosition)
                        {
                            GoalCompleted();
                        }
                    }
                    break;
                case(RobotDoing.Rotate90):
                    _heading = (Direction)( ((int)_heading + 1) % 4 );
                    break;
                case(RobotDoing.RotateNeg90):
                    _heading = (Direction)( ((int)_heading - 1) % 4 );
                    break;
            }
        }
        
        private void GoalCompleted()
        {
            Goal?.FinishTask();
            Goal = null;
            _state = RobotBeing.Free;
        }

        private Vector2Int WhereToMove()
        {
            switch (_heading)
            {
                case(Direction.North):
                    return _gridPosition + Vector2Int.down; 
                case(Direction.West):
                    return _gridPosition + Vector2Int.left; 
                case(Direction.South):
                    return _gridPosition + Vector2Int.up;
                case(Direction.East):
                    return _gridPosition + Vector2Int.right;
                default: return new Vector2Int(-1, -1);
            }
        }
    }
}
