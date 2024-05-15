using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.PB
{
    /// <summary>
    /// Model representation of a goal during the playback
    /// </summary>
    public class PbGoal : GoalLike
    {
        /// <summary>
        /// The first step the goal was alive
        /// </summary>
        private int _aliveFrom;
        /// <summary>
        /// The step the goal was destroyed
        /// </summary>
        private int _aliveTo;
        /// <summary>
        /// The robot id that was assigned to this goal
        /// </summary>
        private int _roboId;
        
        /// <summary>
        /// The corresponding robot's shown id
        /// </summary>
        public new string RoboId => (_roboId + 1).ToString();

        #region Properties

        /// <summary>
        /// The assigned robot's zero based id
        /// </summary>
        public int RoboNumber => _roboId;

        /// <summary>
        /// The goal's zero based id
        /// </summary>
        public int SelfId => GoalData.m_id;
        
        #endregion

        /// <summary>
        /// Event signaling the resurrection or imminent death of a goal (and he cometh in light and he cometh in darkness to save thee from damnation)
        /// </summary>
        [CanBeNull] public event EventHandler<bool> JesusEvent;
        
        
        /// <summary>
        /// Constructor for the goal
        /// </summary>
        /// <param name="selfId">The goal's own id</param>
        /// <param name="gridPos">The position on the map</param>
        public PbGoal(int selfId, Vector2Int gridPos) : base(selfId,gridPos)
        {
            _aliveFrom = _aliveTo = _roboId = -1;
        }

        /// <summary>
        /// Controls the goal's state at the given step
        /// </summary>
        /// <param name="stateIndex">The current state's index</param>
        public void SetTimeTo(int stateIndex)
        {
            bool isAlive = _aliveFrom <= stateIndex && stateIndex < _aliveTo;
            JesusEvent?.Invoke(this, isAlive);
        }

        /// <summary>
        /// Sets the simulation step where the goal was added 
        /// </summary>
        /// <param name="from">The simulation step</param>
        /// <param name="roboId">The corresponding robots zero based id</param>
        /// <exception cref="InvalidFileException">Thrown only if this method was more than one time</exception>
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

        /// <summary>
        /// Sets the simulation step where the goal was removed
        /// </summary>
        /// <param name="to">The simulation step</param>
        /// <exception cref="InvalidFileException">Thrown only if this method was more than one time</exception>
        public void SetAliveTo(int to)
        {
            if (_aliveTo == -1)
            {
                _aliveTo = to;
            }
            else
            {
                throw new InvalidFileException($"Goal {GoalData.m_id} was \"finished\" twice");
            }
        }
    }
}