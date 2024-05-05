using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Structs
{
    public struct RobotStartPos
    {
        /// <summary>
        /// The x coordinate of the robot's starting position
        /// </summary>
        public int x;
        /// <summary>
        /// The y coordinate of the robot's starting position
        /// </summary>
        public int y;
        /// <summary>
        /// The starting heading of the robot
        /// </summary>
        public Direction heading;
        
        /// <summary>
        /// Fancy constructor
        /// </summary>
        /// <param name="x">See <see cref="x"/> for more information</param>
        /// <param name="y">See <see cref="y"/> for more information</param>
        /// <param name="heading">See <see cref="heading"/> for more information</param>
        public RobotStartPos(int x, int y, Direction heading)
        {
            this.x = x;
            this.y = y;
            this.heading = heading;
        }
    }
}