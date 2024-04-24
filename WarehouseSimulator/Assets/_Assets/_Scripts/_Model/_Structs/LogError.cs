namespace WarehouseSimulator.Model.Structs
{
    public struct LogError
    {
        public int robot1;
        public int robot2;
        public int step;
        public string action;
        
        public LogError(int robot1, int robot2, int step, string action)
        {
            this.robot1 = robot1;
            this.robot2 = robot2;
            this.step = step;
            this.action = action;
        }
    }
}