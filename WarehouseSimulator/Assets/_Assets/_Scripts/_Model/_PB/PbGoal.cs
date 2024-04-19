using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.PB
{

    public class PbGoal : GoalLike
    {
        private int _aliveFrom;
        private int _aliveTo;
        private int _roboId;
        private bool _currentlyAlive;

        #region Properties

        public Vector2Int GridPos => GoalData.m_gridPosition;
    
        public int AliveTo
        {
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

        public int RoboNumber => _roboId;

        public int SelfId => GoalData.m_id;

        public bool CurrentlyAlive => _currentlyAlive;
        
        #endregion

        [CanBeNull] public event EventHandler jesusEvent;
        
        public PbGoal(int selfId, Vector2Int gridPos, bool currentlyAlive = false) : base(selfId,gridPos)
        {
            _aliveFrom = _aliveTo = _roboId = -1;
            _currentlyAlive = currentlyAlive;
        }

        public void SetTimeTo(int step)
        {
            if (step > _aliveTo || step < _aliveFrom)
            {
                if (_currentlyAlive)
                {
                    _currentlyAlive = false;
                    jesusEvent?.Invoke(this,EventArgs.Empty);
                }
            }
            else
            {
                if (!_currentlyAlive)
                {
                    _currentlyAlive = true;
                    jesusEvent?.Invoke(this, EventArgs.Empty);
                }
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