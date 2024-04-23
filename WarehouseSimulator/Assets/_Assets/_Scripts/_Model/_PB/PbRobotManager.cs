﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.PB
{
    public class PbRobotManager
    {
        private List<PbRobot> _allRobots;
        
        [CanBeNull] public event EventHandler<RobotCreatedEventArgs> RobotAddedEvent;

        public PbRobotManager()
        {
            _allRobots = new();
        }

        /// <summary>
        /// Sets up all the robots and calculates their future states, positions, headings
        /// </summary>
        /// <param name="stepNumber">The number of steps completed in the simulation</param>
        /// <param name="whoWhere">The starting positions of the robots</param>
        /// <exception cref="InvalidFileException">The exception thrown when we can't calculate the timeline</exception>
        public void SetUpAllRobots(int stepNumber,List<RobotStartPos> whoWhere)
        {
            int i = 0;
            foreach (RobotStartPos startPos in whoWhere)
            {
                var robie = new PbRobot(i, new Vector2Int(startPos.x,startPos.y), stepNumber, startPos.heading);
                try
                {
                    robie.CalcTimeLine(CustomLog.Instance.GetAllActions(i));
                }
                catch (Exception ex)
                {
                    throw new InvalidFileException("Couldn't match the robot id with the number of lists in the log file, the exact error:" +
                                                   $"\n{ex.Message}");
                }
                i++;
                _allRobots.Add(robie);
                RobotAddedEvent?.Invoke(this, new RobotCreatedEventArgs(robie));
            }
        }

        public void SetTimeTo(int stateIndex)
        {
            foreach (PbRobot robie in _allRobots)
            {
                robie.SetTimeTo(stateIndex);
            }
        }
    }
}