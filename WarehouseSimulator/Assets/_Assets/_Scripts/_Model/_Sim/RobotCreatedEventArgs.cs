using System;

namespace WarehouseSimulator.Model.Sim
{
    public class RobotCreatedEventArgs : EventArgs
    {
        public SimRobot SimRobot;
        
        public RobotCreatedEventArgs(SimRobot r)
        {
            SimRobot = r;
        }
    }
}