namespace WarehouseSimulator.Model.Structs
{
    public struct EventInfo
    {
        /// <summary>
        /// The tasks id
        /// </summary>
        public int Task { private set; get; }
        /// <summary>
        /// The step in which the event happened
        /// </summary>
        public int Step { private set; get; }
        /// <summary>
        /// The description of the event
        /// </summary>
        public string WhatHappened { private set; get; }

        /// <summary>
        /// The most advanced constructor you have ever seen
        /// </summary>
        /// <param name="task">The task id</param>
        /// <param name="step">The simulation step</param>
        /// <param name="whatHappened">The description of the event</param>
        public EventInfo(int task, int step, string whatHappened)
        {
            this.Task = task;
            this.Step = step;
            this.WhatHappened = whatHappened;
        }
    }
}