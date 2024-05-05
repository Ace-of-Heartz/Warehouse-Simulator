using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    [CreateAssetMenu(fileName = "SIMULATION_DATA", menuName = "SIMULATION_DATA", order = 0)]
    public class SimulationData : ScriptableObject
    {
        /// <summary>
        /// The number of steps the simulation will run for
        /// </summary>
        public int m_maxStepAmount;
        /// <summary>
        /// The current step the simulation is on
        /// </summary>
        public int m_currentStep;
        /// <summary>
        /// The number of robots in the simulation
        /// </summary>
        public int m_robotAmount;
        /// <summary>
        /// The number of goals in total
        /// <remarks>
        /// This number is set after the goals are loaded from the file. Any goals added interactively are not counted.
        /// </remarks>
        /// </summary>
        public int m_goalAmount;
        /// <summary>
        /// The number of goals remaining in the simulation
        /// </summary>
        public int m_goalsRemaining;
        // TODO: public int GoalsRemaining
        // {
        //     get => m_goalsRemaining;
        //     set => m_goalsRemaining = value;
        // }
        /// <summary>
        /// The time of a single simulation step in milliseconds
        /// </summary>
        public int m_stepTime;
        /// <summary>
        /// The time for path preprocessing in milliseconds
        /// </summary>
        public int m_preprocessTime;
        /// <summary>
        /// Whether the simulation is completed
        /// </summary>
        public bool m_isFinished;
    }
}