using System;

namespace WarehouseSimulator.Model
{
    public class RobotCreatedEventArgs : EventArgs
    {
        public RobotLike Robot;
        
        public RobotCreatedEventArgs(RobotLike r)
        {
            Robot = r;
        }
    }
}