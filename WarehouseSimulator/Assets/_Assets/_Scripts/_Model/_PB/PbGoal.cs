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
        private int _selfId;

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

        public int SelfId
        {
            get => _selfId;
        }
        
        #endregion

        public PbGoal(int selfId,Vector2Int gridPos, bool currentlyAlive = true)
        {
            _selfId = selfId;
            _gridPosition = gridPos;
            _aliveFrom = _aliveTo = _roboId = -1;
            _currentlyAlive = currentlyAlive;
        }

        public void SetTimeTo(int step)
        {
            if (step > _aliveTo || step < _aliveFrom)
            {
                _currentlyAlive = false;
            }
        }

        public void SetAlive(int from, int to,int roboId)
        {
            if (_aliveFrom == -1)
            {
                _aliveFrom = from;
                _aliveTo = to;
                _roboId = roboId;
            }
        }
    }
}