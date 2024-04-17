using System;
using UnityEngine;

namespace WarehouseSimulator.Model.PB
{

    public class PbGoal : GoalLike
    {
        private int _aliveFrom;
        private int _aliveTo;
        private bool _currentlyAlive;
        private int _roboId;

        #region Properties

        public Vector2Int GridPos => GoalData.m_gridPosition;

        public int AliveFrom => _aliveFrom;
    
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
                    throw new InvalidFileException($"Goal {GoalData.m_id} was \"finished\" twice");
                }
            }
        }

        public bool IsAlive => _currentlyAlive;

        public int RoboNumber => _roboId;

        public int SelfId => GoalData.m_id;
        
        #endregion

        public PbGoal(int selfId, Vector2Int gridPos, bool currentlyAlive = false) : base(selfId,gridPos)
        {
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
                throw new InvalidFileException($"Goal {GoalData.m_id} was \"assigned\" twice");
            }
        }
    }
}