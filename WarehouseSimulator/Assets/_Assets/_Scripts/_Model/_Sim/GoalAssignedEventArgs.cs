using System;

namespace WarehouseSimulator.Model.Sim
{
    public class GoalAssignedEventArgs : EventArgs
    {
        public SimGoal SimGoal;
        
        public GoalAssignedEventArgs(SimGoal g)
        {
            SimGoal = g;
        }
    }
}