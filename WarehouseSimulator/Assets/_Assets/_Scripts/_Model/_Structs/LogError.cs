namespace WarehouseSimulator.Model.Structs
{
    /// <summary>
    /// Represents an error in the simulation. Used for logging
    /// </summary>
    public struct LogError
    {
        /// <summary>
        /// One of the robots involved in the error
        /// </summary>
        public int robot1;
        /// <summary>
        /// The other robot involved in the error, or -1 if there is only one robot involved
        /// </summary>
        public int robot2;
        /// <summary>
        /// The step in which the error happened
        /// </summary>
        public int step;
        /// <summary>
        /// The description of the error
        /// </summary>
        public string action;
        
        /// <summary>
        /// The most advanced constructor you have ever seen
        /// </summary>
        /// <param name="robot1">See <see cref="robot1"/> for more information</param>
        /// <param name="robot2">See <see cref="robot2"/> for more information</param>
        /// <param name="step">See <see cref="step"/> for more information</param>
        /// <param name="action">See <see cref="action"/> for more information</param>
        public LogError(int robot1, int robot2, int step, string action)
        {
            this.robot1 = robot1;
            this.robot2 = robot2;
            this.step = step;
            this.action = action;
        }
    }
}