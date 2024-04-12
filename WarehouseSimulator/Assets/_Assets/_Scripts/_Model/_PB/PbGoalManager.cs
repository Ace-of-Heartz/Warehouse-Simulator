using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.PB
{
    public class PbGoalManager
    {
        private List<PbGoal> _allGoals;

        public PbGoalManager()
        {
            _allGoals = new();
        }

        public void SetUpAllGoals(List<TaskInfo> tasks,List<List<EventInfo>> events) //TODO => Blaaa: Ez itt nagyon nem Bool
        {
            int nextid = 0;
            foreach (TaskInfo task in tasks)
            {
                _allGoals.Add(new PbGoal(nextid,new Vector2Int(task.X,task.Y)));
                nextid++;
            }

            foreach (List<EventInfo> roboevent in events)
            {
                
            }
        }

        public void SetTimeTo(int step)
        {
            foreach (PbGoal gboy in _allGoals)
            {
                gboy.SetTimeTo(step);
            }
        }
    }
}