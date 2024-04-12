using System;
using System.Collections.Generic;

namespace WarehouseSimulator.Model.PB
{
    public class PbGoalManager
    {
        private List<PbGoal> _allGoals;

        public PbGoalManager()
        {
            _allGoals = new();
        }

        public void SetUpAllGoals(List<List<Boolean>> tasks,List<List<List<Boolean>>> events) //TODO => Blaaa: Ez itt nagyon nem Bool
        {
            
        }

        public void SetTimeTo(int step)
        {
            foreach (PbGoal gboy in _allGoals)
            {
                gboy.SetTimeTo(step);
            }
        }
    }
}