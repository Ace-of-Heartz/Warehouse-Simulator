using System;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class RobotHappenedEventArgs : EventArgs
    {
        public Vector2Int Where { private set; get; }

        public RobotHappenedEventArgs(Vector2Int w)
        {
            Where = w;
        }
    }
}