using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimGoalManager
    {
        private Queue<SimGoal> _goalsRemaining;
        private int _nextid;
        
        public int GoalCount => _goalsRemaining.Count;

        public SimGoalManager()
        {
            _goalsRemaining = new Queue<SimGoal>();
            _nextid = 0;
        }

        public void AddNewGoal(Vector2Int here, Map mapie)
        {
            if (mapie.GetTileAt(here) != TileType.Empty)
            {
                throw new InvalidFileException($"Invalid file format: {_nextid + 2}. line does not provide a valid position");
            }
            SimGoal newGoal = new(here,_nextid);
            _nextid++;
            _goalsRemaining.Enqueue(newGoal);
            CustomLog.Instance.AddTaskData(newGoal.GoalID, newGoal.GridPosition.x, newGoal.GridPosition.y);
        }

        [CanBeNull]
        public SimGoal GetNext()
        {
            SimGoal next = null;
            try
            {
                next = _goalsRemaining.Dequeue();
            }
            catch (Exception) { /* ignored */ }

            return next;
        }

        public void ReadGoals(string from, Map mapie)
        {
            using StreamReader riiid = new(from);
            if (!int.TryParse(riiid.ReadLine(), out int goaln))
            {
                throw new InvalidFileException("Invalid file format: First line not a number");
            }
            
            for (int i = 0; i < goaln; i++)
            {
                string line = riiid.ReadLine();
                if (line == null)
                {
                    throw new InvalidFileException("Invalid file format: there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidFileException($"Invalid file format: {_nextid + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidFileException($"Invalid file format: {_nextid + 2}. line does not provide a valid position");
                }

                Vector2Int nextPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                AddNewGoal(nextPos);
            }
        }
    }
}