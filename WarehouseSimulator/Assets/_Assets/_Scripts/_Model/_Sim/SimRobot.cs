using System;
using JetBrains.Annotations;
using Unity.Properties;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimRobot : RobotLike
    {
        public SimRobot(int i, 
            Vector2Int gridPos, 
            Direction heading = Direction.North, 
            SimGoal goal = null, 
            RobotBeing state = RobotBeing.Free) 
                : base (i,gridPos,heading,state,goal) { }

        public void AssignGoal(SimGoal goTo)
        {
            goTo.AssignedTo(this);
            RobotData.m_goal = goTo;
            // _state = RobotBeing.InTask;
            RobotData.m_state = RobotBeing.InTask;
        }


        public void PerformActionRequested(RobotDoing watt, Map mapie)
        {
            if (mapie == null) { throw new ArgumentNullException($"The argument: {nameof(mapie)} as the map does not exist"); }

            switch (watt)
            {
                case (RobotDoing.Timeout):
                case (RobotDoing.Wait):
                    break;
                case (RobotDoing.Forward):
                    Vector2Int nextPos = WhereToMove(RobotData.m_gridPosition);
                    if (mapie.GetTileAt(nextPos) == TileType.Wall)
                    {
                        //TODO => Blaaa: CC react and LOG
                    }
                    else if (mapie.GetTileAt(nextPos) == TileType.RoboOccupied)
                    {
                        //TODO => Blaaa: CC react and LOG
                    }
                    else
                    {
                        //TODO => Unity react
                        mapie.DeoccupyTile(RobotData.m_gridPosition);
                        RobotData.m_gridPosition = nextPos;
                        mapie.OccupyTile(RobotData.m_gridPosition);
                        if (nextPos == RobotData.m_goal?.GridPosition)
                        {
                            GoalCompleted();
                        }
                    }

                    break;
                case (RobotDoing.Rotate90):
                    RobotData.m_heading = (Direction)(((int)RobotData.m_heading + 1) % 4);
                    break;
                case (RobotDoing.RotateNeg90):
                    RobotData.m_heading = (Direction)(((int)RobotData.m_heading - 1) % 4);
                    break;
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
