using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    public abstract class RobotLike
    {
        #region Fields
        private readonly int _id;
        private Vector2Int _currentGridPosition;
        private Direction _currentHeading;
        private RobotBeing _currentState;
        #endregion
        
        #region Properties
        public int Id
        {
            get => _id;
        }
        public Vector2Int GridPosition
        {
            get => _currentGridPosition;
        }
        public Direction Heading
        {
            get => _currentHeading;
        }
        
        public RobotBeing State
        {
            get => _currentState;
        }
        #endregion
        
        protected RobotLike(int i, Vector2Int gPos, Direction h = Direction.North, RobotBeing s = RobotBeing.Free)
        {
            _id = i;
            _currentGridPosition = gPos;
            _currentHeading = h;
            _currentState = s;
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