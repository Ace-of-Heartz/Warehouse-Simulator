using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace WarehouseSimulator.View.MainMenu
{
    public struct SimInputArgs
    {
        [CanBeNull] public string ConfigFilePath { get; set; }
        public int NumberOfSteps { get; set; }
        public int IntervalOfSteps { get; set; }
        public float PreparationTime { get; set; }
        [CanBeNull] public string EventLogPath { get; set; }

        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(ConfigFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
        
    }
}