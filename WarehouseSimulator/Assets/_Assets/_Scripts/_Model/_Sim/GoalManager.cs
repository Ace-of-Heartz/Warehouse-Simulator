using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class GoalManager
    {
        private Queue<Goal> _goalsRemaining; //kérdés: ha ezek csak a még ki nem osztott célok, akkor már aktív célokat mi fogja tartalmazni?

        public GoalManager()
        {
            _goalsRemaining = new Queue<Goal>();
        }

        public void AddNewGoal(Vector2Int hir)
        {
            _goalsRemaining.Enqueue(new Goal(hir));
        }

        [CanBeNull]
        public Goal GetNext()
        {
            Goal next = null;
            try
            {
                next = _goalsRemaining.Dequeue();
            }
            catch (Exception)
            { }
            return next;
        }

        public void ReadGoals(string from, Vector2Int mapSize)
        {
            using StreamReader riiid = new(from);
            if (!int.TryParse(riiid.ReadLine(),out int goaln)) 
            {throw new InvalidDataException("Invalid file format: First line not a number"); }

            int runningGoaln = 0;
            string line = riiid.ReadLine();
            while (line != null)
            {
                if (runningGoaln >= goaln)
                {
                    throw new InvalidDataException("Invalid file format: there were too many lines");
                }

                if (!int.TryParse(riiid.ReadLine(), out int linPos)) 
                {
                    throw new InvalidDataException($"Invalid file format: {runningGoaln + 2}. line not a number");
                }

                int quot = linPos / mapSize.y;
                Vector2Int newGoalPos = new(linPos - mapSize.y * quot,quot);
                _goalsRemaining.Enqueue(new Goal(newGoalPos));

                runningGoaln++;
                line = riiid.ReadLine();
            }

        }
    }
}