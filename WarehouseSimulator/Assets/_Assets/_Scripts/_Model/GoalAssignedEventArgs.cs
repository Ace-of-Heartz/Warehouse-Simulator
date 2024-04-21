using System;

namespace WarehouseSimulator.Model
{
    public class GoalAssignedEventArgs : EventArgs
    {
        public GoalLike Goal;
        
        public GoalAssignedEventArgs(GoalLike g)
        {
            Goal = g;
        }
    }
}