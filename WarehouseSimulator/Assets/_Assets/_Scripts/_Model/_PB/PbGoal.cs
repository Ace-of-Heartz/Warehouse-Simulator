using System;
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

        public int AliveFrom
        {
            get => _aliveFrom;
        }
        
        public int AliveTo
        {
            get => _aliveTo;
            set
            {
                if (_aliveTo == -1)
                {
                    _aliveTo = value;
                }
                else
                {
                    throw new InvalidFileException($"Goal {_selfId} was given \"finished\" twice");
                }
            }
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

        public PbGoal(int selfId,Vector2Int gridPos, bool currentlyAlive = false)
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

        public void SetAliveFrom(int from, int roboId)
        {
            if (_aliveFrom == -1)
            {
                _aliveFrom = from;
                _roboId = roboId;
            }
            else
            {
                throw new InvalidFileException($"Goal {_selfId} was given \"assigned\" twice");
            }
        }
    }
}