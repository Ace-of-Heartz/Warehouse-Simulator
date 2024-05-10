using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Simulation user configuration
    /// </summary>
    public struct SimInputArgs
    {
        /// <summary>
        /// The path to the configuration file
        /// </summary>
        public string ConfigFilePath;
        /// <summary>
        /// The number of steps the simulation should run for
        /// </summary>
        public int NumberOfSteps;
        /// <summary>
        /// The time between each step in milliseconds
        /// </summary>
        public int IntervalOfSteps;
        /// <summary>
        /// The time the simulation can prepare before starting
        /// </summary>
        public int PreparationTime;
        /// <summary>
        /// The path of the event log file
        /// </summary>
        public string EventLogPath;
        /// <summary>
        /// The search algorithm to use
        /// </summary>
        public SEARCH_ALGORITHM SearchAlgorithm;
        
        /// <summary>
        /// Checks if the input arguments are complete
        /// </summary>
        /// <returns>True if this all fields contain semi-valid values</returns>
        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(ConfigFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
        
    }
}