namespace WarehouseSimulator.Model.Structs
{
    public struct TaskInfo
    {
        public int Task { private set; get; }
        public int X { private set; get; }
        public int Y { private set; get; }

        public TaskInfo(int task, int x, int y)
        {
            this.Task = task;
            this.X = x;
            this.Y = y;
        }
    }
}