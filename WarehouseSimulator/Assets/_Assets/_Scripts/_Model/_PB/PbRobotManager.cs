using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.PB
{
    public class PbRobotManager
    {
        /// <summary>
        /// The list of all the robots in the playback
        /// </summary>
        private List<PbRobot> _allRobots;
        
        /// <summary>
        /// Invoked when a robot is created.
        /// </summary>
        [CanBeNull] public event EventHandler<RobotCreatedEventArgs> RobotAddedEvent;

        /// <summary>
        /// Constructor for the PbRobotManager class. Yes it is redundant. Yes it works. Yes this summary is necessary. And yes, have a good day.
        /// </summary>
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

        
        /// <summary>
        /// Set the time to a specific state for all the robots.
        /// </summary>
        /// <param name="stateIndex">The current stateIndex</param>
        public void SetTimeTo(int stateIndex)
        {
            foreach (PbRobot robie in _allRobots)
            {
                robie.SetTimeTo(stateIndex);
            }
        }
    }
}