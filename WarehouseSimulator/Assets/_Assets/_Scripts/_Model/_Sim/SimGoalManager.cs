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

        /// <summary>
        ///     Adds a new goal to the queue while incrementing the goal ID.
        /// </summary>
        /// <remarks>
        ///     Logs the goal to the CustomLog.
        /// </remarks>>
        /// <param name="inputPosition"></param>
        /// <param name="mapie">Best pie is the mapie</param>
        /// <exception cref="ArgumentException">Throws this if argument position is not empty.</exception>
        public void AddNewGoal(Vector2Int inputPosition, Map mapie)
        {
            if (mapie.GetTileAt(inputPosition) == TileType.Wall)
            {
                throw new ArgumentException($"Invalid position given: map tile at {inputPosition} is not empty.");
            }
            SimGoal newGoal = new(inputPosition,_nextid);
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
                throw new InvalidFileException("Invalid .tasks file format:\n First line not a number");
            }
            
            for (int i = 0; i < goaln; i++)
            {
                string line = riiid.ReadLine();
                if (line == null)
                {
                    throw new InvalidFileException("Invalid .tasks file format:\n there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidFileException($"Invalid .tasks file format:\n {_nextid + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidFileException($"Invalid .tasks file format:\n {_nextid + 2}. line does not provide a valid position");
                }

                Vector2Int nextPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                AddNewGoal(nextPos,mapie);
            }
        }
    }
}