using UnityEngine;

namespace WarehouseSimulator.Model.Sim
{
    [CreateAssetMenu(fileName = "SIMULATION_DATA", menuName = "SIMULATION_DATA", order = 0)]
    public class SimulationData : ScriptableObject
    {
        public int m_maxStepAmount;
        public int m_currentStep;
        public int m_robotAmount;
        public int m_goalAmount;
        public int m_goalsRemaining;
        public int m_stepTime;
        public int m_preprocessTime;
        public bool m_isFinished;
    }
}