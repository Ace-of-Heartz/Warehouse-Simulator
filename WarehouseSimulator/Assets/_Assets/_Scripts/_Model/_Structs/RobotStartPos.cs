using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Structs
{
    public struct RobotStartPos
    {
        public int x;
        public int y;
        public Direction heading;
        
        public RobotStartPos(int x, int y, Direction heading)
        {
            this.x = x;
            this.y = y;
            this.heading = heading;
        }
    }
}