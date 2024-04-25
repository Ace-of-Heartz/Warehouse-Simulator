﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.PB
{
    public class PbGoalManager
    {
        private Dictionary<int,PbGoal> _allGoals;
        
        [CanBeNull] public event EventHandler<GoalAssignedEventArgs> GoalAssignedEvent;

        public PbGoalManager()
        {
            _allGoals = new();
        }
        
        public void SetUpAllGoals(List<TaskInfo> tasks,Dictionary<int, List<EventInfo>> events) 
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
                            thisOne.AliveTo = oneEvent.Step;
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

        public void SetTimeTo(int step)
        {
            foreach ((int _,PbGoal gboy) in _allGoals)
            {
                gboy.SetTimeTo(step);
            }
        }
    }
}