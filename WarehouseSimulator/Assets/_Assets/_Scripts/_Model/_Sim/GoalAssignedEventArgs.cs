using System;

namespace WarehouseSimulator.Model.Sim
{
    public class GoalAssignedEventArgs : EventArgs
    {
        public Goal goal;
        
        public GoalAssignedEventArgs(Goal g)
        {
            goal = g;
        }
    }
}