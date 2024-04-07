using System;

namespace WarehouseSimulator.Model.Sim
{
    public class RobotCreatedEventArgs : EventArgs
    {
        public Robot robot;
        
        public RobotCreatedEventArgs(Robot r)
        {
            robot = r;
        }
    }
}