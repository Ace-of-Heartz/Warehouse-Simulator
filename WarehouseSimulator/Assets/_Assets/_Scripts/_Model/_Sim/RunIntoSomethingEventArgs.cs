using System;
using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    public class RunIntoSomethingEventArgs : EventArgs
    {
        public Vector2Int Where { private set; get; }

        public RunIntoSomethingEventArgs(Vector2Int w)
        {
            Where = w;
        }
    }
}