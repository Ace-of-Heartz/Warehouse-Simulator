namespace WarehouseSimulator.Model
{
    public struct PbInputArgs
    {
        public string MapFilePath;

        public string EventLogPath;

        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(MapFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
    }
}