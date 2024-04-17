#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimRobot : RobotLike
    {
        private Vector2Int _nextPos;

        public SimRobot(int i,
            Vector2Int gridPos,
            Direction heading = Direction.North,
            SimGoal? goal = null,
            RobotBeing state = RobotBeing.Free)
                : base(i, gridPos, heading, state, goal)
        {
            _nextPos = new(-1, -1);
        }

        public Vector2Int NextPos => _nextPos;
        
        public void AssignGoal(SimGoal goTo)
        {
            goTo.AssignedTo(this);
            RobotData.m_goal = goTo;
            RobotData.m_state = RobotBeing.InTask;
        }

        public (bool,SimRobot?) TryPerformActionRequestedAsync(RobotDoing watt, Map mapie)
        {
            _nextPos = RobotData.m_gridPosition;
            if (mapie == null)
            {
                throw new ArgumentNullException($"The argument: {nameof(mapie)} as the map does not exist");
            }

            switch (watt)
            {
                case (RobotDoing.Timeout):
                case (RobotDoing.Wait):
                    break;
                case (RobotDoing.Forward):
                    _nextPos = WhereToMove(RobotData.m_gridPosition);
                    if (mapie.GetTileAt(_nextPos) == TileType.Wall)
                    {
                        //TODO => Blaaa: CC react and LOG
                        return (false, this);
                        //return ((false, this));
                    }
                    // else if (mapie.GetTileAt(nextPos) == TileType.RoboOccupied)
                    // {
                    //     //TODO => Blaaa: CC react and LOG
                    // }
                    break;
                case (RobotDoing.Rotate90):
                    RobotData.m_heading = (Direction)(((int)RobotData.m_heading + 1) % 4);
                    break;
                case (RobotDoing.RotateNeg90):
                    RobotData.m_heading = (Direction)(((int)RobotData.m_heading - 1) % 4);
                    break;
            }

            return (true,null);
        }

        public void MakeStep(Map mipieMap)
        {
            mipieMap.DeoccupyTile(RobotData.m_gridPosition);
            RobotData.m_gridPosition = _nextPos;
            mipieMap.OccupyTile(RobotData.m_gridPosition);
            if (_nextPos == RobotData.m_goal?.GridPosition)
            {
                GoalCompleted();
            }
        }
        
        private void GoalCompleted()
        {
            if (RobotData.m_goal is SimGoal)
            {
                var bla = (SimGoal)RobotData.m_goal;
                bla.FinishTask();
            }
            RobotData.m_goal = null;
            RobotData.m_state = RobotBeing.Free;
        }
    }
}
