using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class GoalManager
    {
        private List<Goal> _goalsRemaining; //kérdés: ha ezek csak a még ki nem osztott célok, akkor már aktív célokat mi fogja tartalmazni?

        public GoalManager(List<Goal> gs)
        {
            _goalsRemaining = gs;
        }

        public void AddNewGoal(Vector2Int hir)
        {
            _goalsRemaining.Add(new Goal(hir));
        }

        public Goal GetNext()
        {
            var next = _goalsRemaining.First();
            _goalsRemaining.Remove(next);
            return next;
        }
    }
}