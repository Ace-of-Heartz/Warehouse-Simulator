using UnityEngine;

namespace WarehouseSimulator.Model.PB
{
    public class PbGoal
    {
        private Vector2Int _gridPosition;
        private int _aliveFrom;
        private int _aliveTo;
        private bool _currentlyAlive;
        private int _roboId;

        #region Properties

        public Vector2Int GridPos
        {
            get => _gridPosition;
        }

        public int AliveF
        {
            get => _aliveFrom;
        }
        
        public int AliveT
        {
            get => _aliveTo;
        }

        public bool IsAlive
        {
            get => _currentlyAlive;
        }

        public int RoboNumber
        {
            get => _roboId;
        }
        
        #endregion

        public PbGoal(Vector2Int gridPos, int aliveFrom, int aliveTo, int roboId, bool currentlyAlive = true)
        {
            _gridPosition = gridPos;
            _aliveFrom = aliveFrom;
            _aliveTo = aliveTo;
            _roboId = roboId;
            _currentlyAlive = currentlyAlive;
        }

        public void SetTimeTo(int step)
        {
            if (step > _aliveTo || step < _aliveFrom)
            {
                _currentlyAlive = false;
            }
        }
    }
}