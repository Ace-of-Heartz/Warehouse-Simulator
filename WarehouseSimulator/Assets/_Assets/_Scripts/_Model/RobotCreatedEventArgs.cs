using System;

namespace WarehouseSimulator.Model
{
    public class RobotCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// The robot that was created.
        /// </summary>
        public RobotLike Robot;
        
        /// <summary>
        /// Yes, this is a constructor.
        /// </summary>
        /// <param name="r">The robot that was created</param>
        public RobotCreatedEventArgs(RobotLike r)
        {
            Robot = r;
        }
    }
}