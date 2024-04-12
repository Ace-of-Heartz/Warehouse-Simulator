using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class GoalManager
    {
        private Queue<Goal> _goalsRemaining;

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

        public void ReadGoals(string from, Map mapie)
        {
            using StreamReader riiid = new(from);
            if (!int.TryParse(riiid.ReadLine(), out int goaln))
            {
                throw new InvalidDataException("Invalid file format: First line not a number");
            }

            int runningGoaln = 0;
            for (int i = 0; i < goaln; i++)
            {
                string line = riiid.ReadLine();
                if (line == null)
                {
                    throw new InvalidDataException("Invalid file format: there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidDataException($"Invalid file format: {runningGoaln + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidDataException($"Invalid file format: {runningGoaln + 2}. line does not provide a valid position");
                }

                Vector2Int nextPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                _goalsRemaining.Enqueue(new Goal(nextPos));
                runningGoaln++;
            }
        }
    }
}