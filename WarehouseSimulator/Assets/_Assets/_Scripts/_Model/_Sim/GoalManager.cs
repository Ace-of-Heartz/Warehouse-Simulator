﻿using System;
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
        private int _nextid;
        
        

        public GoalManager()
        {
            _goalsRemaining = new Queue<Goal>();
            _nextid = 0;
        }

        public void AddNewGoal(Vector2Int here)
        {
            _goalsRemaining.Enqueue(new Goal(here,_nextid));
            _nextid++;
        }

        [CanBeNull]
        public Goal GetNext()
        {
            Goal next = null;
            try
            {
                next = _goalsRemaining.Dequeue();
            }
            catch (Exception) { }
            return next;
        }

        public void ReadGoals(string from, Map mapie)
        {
            using StreamReader riiid = new(from);
            if (!int.TryParse(riiid.ReadLine(), out int goaln))
            {
                throw new InvalidDataException("Invalid file format: First line not a number");
            }
            
            for (int i = 0; i < goaln; i++)
            {
                string line = riiid.ReadLine();
                if (line == null)
                {
                    throw new InvalidDataException("Invalid file format: there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidDataException($"Invalid file format: {_nextid + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidDataException($"Invalid file format: {_nextid + 2}. line does not provide a valid position, because pos: {linPos % mapie.MapSize.x}, {linPos / mapie.MapSize.x} and tyle was {mapie.GetTileAt(linPos)}");
                }

                Vector2Int nextPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                _goalsRemaining.Enqueue(new Goal(nextPos,_nextid));
                _nextid++;
            }
        }
    }
}