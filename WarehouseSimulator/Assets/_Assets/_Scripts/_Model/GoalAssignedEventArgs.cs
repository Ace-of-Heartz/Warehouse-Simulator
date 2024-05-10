using System;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Event arguments for when a goal is assigned.
    /// </summary>
    public class GoalAssignedEventArgs : EventArgs
    {
        /// <summary>
        /// The goal that was assigned.
        /// </summary>
        public GoalLike Goal;
        
        /// <summary>
        /// Yes this is a constructor.
        /// </summary>
        /// <param name="g">The goal assigned</param>
        public GoalAssignedEventArgs(GoalLike g)
        {
            Goal = g;
        }
    }
}