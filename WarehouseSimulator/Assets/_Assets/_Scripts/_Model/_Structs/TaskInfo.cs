namespace WarehouseSimulator.Model.Structs
{
    /// <summary>
    /// Represents a task. Used for logging
    /// </summary>
    public struct TaskInfo
    {
        /// <summary>
        /// The task's id
        /// </summary>
        public int Task { private set; get; }
        /// <summary>
        /// The x coordinate of the task
        /// </summary>
        public int X { private set; get; }
        /// <summary>
        /// The y coordinate of the task
        /// </summary>
        public int Y { private set; get; }

        /// <summary>
        /// Fancy constructor
        /// </summary>
        /// <param name="task">The task id</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public TaskInfo(int task, int x, int y)
        {
            this.Task = task;
            this.X = x;
            this.Y = y;
        }
    }
}