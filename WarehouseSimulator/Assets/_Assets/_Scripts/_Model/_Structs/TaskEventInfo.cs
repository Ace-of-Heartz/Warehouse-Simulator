namespace WarehouseSimulator.Model.Structs
{
    public struct TaskEventInfo
    {
        public int Task { private set; get; }
        public int Step { private set; get; }
        public string WhatHappened { private set; get; }

        public TaskEventInfo(int task, int step, string whatHappened)
        {
            this.Task = task;
            this.Step = step;
            this.WhatHappened = whatHappened;
        }
    }
}