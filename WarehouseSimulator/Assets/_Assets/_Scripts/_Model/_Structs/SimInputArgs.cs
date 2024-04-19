using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    public struct SimInputArgs
    {
        public string ConfigFilePath;
        public int NumberOfSteps;
        public int IntervalOfSteps;
        public float PreparationTime;
        public string EventLogPath;
        public SEARCH_ALGORITHM SearchAlgorithm;
        
        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(ConfigFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
        
    }
}