namespace WarehouseSimulator.Model.Structs
{
    public struct EventInfo
    {
        public int Task { private set; get; }
        public int Step { private set; get; }
        public string WhatHappened { private set; get; }

        public EventInfo(int task, int step, string whatHappened)
        {
            this.Task = task;
            this.Step = step;
            this.WhatHappened = whatHappened;
        }
    }
}