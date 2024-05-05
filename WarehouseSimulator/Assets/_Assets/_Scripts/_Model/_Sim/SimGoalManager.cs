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
        /// <summary>
        /// The queue of goals that are yet to be assigned.
        /// </summary>
        private Queue<SimGoal> _goalsRemaining;
        /// <summary>
        /// The ID of the next goal to be added.
        /// </summary>
        private int _nextid;
        
        /// <summary>
        /// The number of goals remaining.
        /// </summary>
        public int GoalCount => _goalsRemaining.Count;
        
        /// <summary>
        /// Constructor for the SimGoalManager class. Yes it is redundant. Yes it works. Yes this summary is necessary. And yes, have a good day.
        /// </summary>
        public SimGoalManager()
        {
            _goalsRemaining = new Queue<SimGoal>();
            _nextid = 0;
        }

        /// <summary>
        /// Adds a new goal to be completed.
        /// </summary>
        /// <remarks>
        /// Logs the goal to the CustomLog.
        /// </remarks>>
        /// <param name="position">The proposed position of the goal</param>
        /// <param name="mapie">Best pie is the mapie</param>
        /// <exception cref="ArgumentException">Throws this if argument position is not empty.</exception>
        public void AddNewGoal(Vector2Int position, Map mapie)
        {
            if (mapie.GetTileAt(position) == TileType.Wall)
            {
                throw new ArgumentException($"Invalid position given: map tile at {position} is not empty.");
            }
            SimGoal newGoal = new(position,_nextid);
            _nextid++;
            _goalsRemaining.Enqueue(newGoal);
            CustomLog.Instance.AddTaskData(newGoal.GoalID, newGoal.GridPosition.x, newGoal.GridPosition.y);
        }

        /// <summary>
        /// Gets the next goal from the queue if there is one.
        /// </summary>
        /// <returns>The next goal in the queue or null if the queue is empty. </returns>
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

        /// <summary>
        /// Loads the goals from a file.
        /// </summary>
        /// <param name="from">The path to the file</param>
        /// <param name="mapie">The map that the goal is added to</param>
        /// <exception cref="InvalidFileException">Thrown if the file is not up to standard</exception>
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

                Vector2Int nextPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                AddNewGoal(nextPos,mapie);
            }
        }
    }
}