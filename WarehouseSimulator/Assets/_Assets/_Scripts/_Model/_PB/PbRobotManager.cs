using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

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

        public void SetUpAllRobots(int stepNumber,List<(Vector2Int,Direction)> whoWhere,List<List<RobotDoing>> robiesDoing)
        {
            if (stepNumber < robiesDoing.Count)
            {
                throw new ArgumentException($"Argument {nameof(stepNumber)} with value {stepNumber} too low");
            }

            if (stepNumber > robiesDoing.Count)
            {
                throw new ArgumentException($"Argument {nameof(stepNumber)} with value {stepNumber} too high");
            }
            
            int i = 1;
            foreach ((var coordin, var dirr) in whoWhere)
            {
                var robie = new PbRobot(i, coordin, stepNumber, dirr);
                robie.CalcTimeLine(robiesDoing[i-1]);
                i++;
                _allRobots.Add(robie);
                RobotAddedEvent?.Invoke(this, new RobotCreatedEventArgs(robie));
            }
        }

        public void SetTimeTo(int step)
        {
            foreach (PbRobot robie in _allRobots)
            {
                robie.SetTimeTo(step); //TODO => Blaaa: async?
            }
        }
    }
}