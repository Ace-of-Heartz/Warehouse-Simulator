namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Playback user configuration
    /// </summary>
    public struct PbInputArgs
    {
        /// <summary>
        /// The path of the map file
        /// </summary>
        public string MapFilePath;

        /// <summary>
        /// The path of the log file
        /// </summary>
        public string EventLogPath;

        /// <summary>
        /// Checks if the input arguments are complete
        /// </summary>
        /// <returns>True if this all fields contain semi-valid values</returns>
        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(MapFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
    }
}