using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.PB
{
    /// <summary>
    /// Manages all goals during the playback
    /// </summary>
    public class PbGoalManager
    {
        /// <summary>
        /// A dictionary that contains all the goals that are in the playback.
        /// The key is the id of the goal and the value is the goal itself.
        /// The id is zero based and is assigned in the order that the goals are read from the file.
        /// </summary>
        private Dictionary<int,PbGoal> _allGoals;
        
        /// <summary>
        /// Invoked when a goal is assigned to a robot.
        /// </summary>
        [CanBeNull] public event EventHandler<GoalAssignedEventArgs> GoalAssignedEvent;

        
        /// <summary>
        /// Constructor for the PbGoalManager class. Yes it is redundant. Yes it works. Yes this summary is necessary. And yes, have a good day.
        /// </summary>
        public PbGoalManager()
        {
            _allGoals = new();
        }
        
        /// <summary>
        /// Set up all the goals that are in the simulation from a log file.
        /// </summary>
        /// <param name="tasks">The task positions and id's</param>
        /// <param name="events">The goal assigned and finished events</param>
        /// <exception cref="InvalidFileException">Thrown when there is conflicting information in the parameters. Eg one goal is assigned or finished more than once</exception>
        public void SetUpAllGoals(List<TaskInfo> tasks, Dictionary<int, List<EventInfo>> events) 
        {
            int nextid = 0;
            foreach (TaskInfo task in tasks)
            {
                var nextGoal = new PbGoal(nextid, new Vector2Int(task.X, task.Y));
                _allGoals.Add(nextid,nextGoal);
                nextid++;
            }
            
            foreach ((int roboId, List<EventInfo> roboEvent) in events)
            {
                foreach (EventInfo oneEvent in roboEvent)
                {
                    if (_allGoals.TryGetValue(oneEvent.Task ,out PbGoal thisOne))
                    {
                        if (oneEvent.WhatHappened == "assigned")
                        {
                            thisOne.SetAliveFrom(oneEvent.Step,roboId);
                        } 
                        else if (oneEvent.WhatHappened == "finished")
                        {
                            if (thisOne.RoboNumber != roboId)
                            {
                                throw new InvalidFileException("The log file was in an incorrect format:\n" +
                                                               $"The task {thisOne.SelfId} was assigned to robot {thisOne.RoboNumber}" +
                                                               $"but was finished by robot {roboId}");
                            }
                            thisOne.SetAliveTo(oneEvent.Step);
                        }
                        else
                        {
                            throw new InvalidFileException("The log file was in an incorrect format:\n" +
                                                           "In the list of events other keywords were used besides \"assigned\"" +
                                                           "and \"finished\"");
                        }
                    }
                }
            }

            //Invoke the event only when all the goals are set up
            foreach (var (id, g) in _allGoals)
            {
                GoalAssignedEvent?.Invoke(this, new GoalAssignedEventArgs(g));
            }
        }

        /// <summary>
        /// Set the time to a specific state for all the goals.
        /// </summary>
        /// <param name="stateIndex">The current stateIndex</param>
        public void SetTimeTo(int stateIndex)
        {
            foreach ((int _,PbGoal gboy) in _allGoals)
            {
                gboy.SetTimeTo(stateIndex);
            }
        }
    }
}