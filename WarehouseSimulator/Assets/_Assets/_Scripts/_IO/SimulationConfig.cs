using System;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Persistence
{
    [Serializable]
    public struct AceSimulationConfig
    {
        [SerializeField] public string mapFile;
        [SerializeField] public int robotCount;
        [SerializeField] public string robotFile;
        [SerializeField] public string tasksFile;
        [SerializeField] public int tasksReveal;
        [SerializeField] public TASK_ASSIGNMENT_STRATEGY taskAssignmentStrategy;
    }
}