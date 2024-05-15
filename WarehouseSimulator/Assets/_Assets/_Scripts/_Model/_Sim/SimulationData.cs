using UnityEngine;
using UnityEngine.Serialization;

namespace WarehouseSimulator.Model.Sim
{
    /// <summary>
    /// Holds the data for the simulation
    /// </summary>
    [CreateAssetMenu(fileName = "SIMULATION_DATA", menuName = "SIMULATION_DATA", order = 0)]
    public class SimulationData : ScriptableObject
    {
        /// <summary>
        /// The number of steps the simulation will run for
        /// </summary>
        public int _maxStepAmount;
        /// <summary>
        /// The current step the simulation is on
        /// </summary>
        public int _currentStep;
        /// <summary>
        /// The number of robots in the simulation
        /// </summary>
        public int _robotAmount;
        /// <summary>
        /// The number of goals in total
        /// <remarks>
        /// This number is set after the goals are loaded from the file. Any goals added interactively are not counted.
        /// </remarks>
        /// </summary>
        public int _goalAmount;
        /// <summary>
        /// The number of goals remaining in the simulation
        /// </summary>
        public int _goalsRemaining;
        /// <summary>
        /// The time of a single simulation step in milliseconds
        /// </summary>
        public int _stepTime;
        /// <summary>
        /// The time for path preprocessing in milliseconds
        /// </summary>
        public int _preprocessTime;
        /// <summary>
        /// Whether the simulation is completed
        /// </summary>
        public bool _isFinished;
    }
}