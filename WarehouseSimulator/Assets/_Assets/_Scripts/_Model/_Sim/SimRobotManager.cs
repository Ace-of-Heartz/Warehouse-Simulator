using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class SimRobotManager
    {
        private List<SimRobot> AllRobots;

        [CanBeNull] public event EventHandler<RobotCreatedEventArgs> RobotAddedEvent;
        [CanBeNull] public event EventHandler<GoalAssignedEventArgs> GoalAssignedEvent;

        public SimRobotManager()
        {
            AllRobots = new();
        }
        
        private void AddRobot(int i, Vector2Int pos)
        {
            SimRobot newR = new(i, pos);
            AllRobots.Add(newR);
            RobotAddedEvent?.Invoke(this, new(newR));
        }
    
        public void AssignTasksToFreeRobots(SimGoalManager from)
        {
            foreach (var robie in AllRobots)
            {
                if (robie.State == RobotBeing.Free)
                {
                    SimGoal next = from.GetNext();
                    if (next == null) { break; }
                    robie.AssignGoal(next);
                    GoalAssignedEvent?.Invoke(this, new(next));
                }
            }
        }

        public bool CheckValidSteps(List<(SimRobot robie, RobotDoing action)> actions,Map mapie)
        {

            // foreach ((SimRobot robie,RobotDoing what) in actions)
            // {
            //     if (!robie.TryPerformActionRequested(what, mapie)) return false; //TODO => Blaaa: Async?
            // }

            if (!actions.All(tuple => tuple.robie.TryPerformActionRequested(tuple.action, mapie))) return false;
            //TODO => Blaaa: async?

            foreach (SimRobot robie in AllRobots)
            {
                foreach (SimRobot compRobie in AllRobots)
                {
                    if (robie.RobotData.m_id == compRobie.RobotData.m_id) continue; //ha ugyanarról a robotról van szó, akkor skip
                    
                    if (compRobie.NextPos == robie.NextPos) return false; 
                    //megnézzük, hogy a jövőbeli pozíciók között van-e bárhol olyan, amikor egymásra akarnának lépni

                    if (compRobie.NextPos == robie.RobotData.m_gridPosition
                        & robie.NextPos == compRobie.RobotData.m_gridPosition) return false;
                    //ha egymás helyére akarnának lépni (át akarnák ugrani egymást), akkor hibás a lépés
                    
                    //TODO => Nincs más, amit meg kéne nézni?
                }
            }
            
            return true;
        }
        
        public void RoboRead(string from, Map mapie)
        {
            using StreamReader rid = new(from);
            if (!int.TryParse(rid.ReadLine(), out int robn))
            {
                throw new InvalidDataException("Invalid file format: First line not a number");
            }

            int nextid = 0;
            for (int i = 0; i < robn; i++)
            {   
                string line = rid.ReadLine();
                if (line == null)
                {
                    throw new InvalidDataException("Invalid file format: there weren't enough lines");
                } 
                if (!int.TryParse(line, out int linPos))
                {
                    throw new InvalidDataException($"Invalid file format: {nextid + 2}. line not a number");
                }
                if (mapie.GetTileAt(linPos) != TileType.Empty)
                {
                    throw new InvalidDataException($"Invalid file format: {nextid + 2}. line does not provide a valid position");
                }

                Vector2Int nextRobPos = new(linPos % mapie.MapSize.x, linPos / mapie.MapSize.x);
                mapie.OccupyTile(nextRobPos);
                AddRobot(nextid,nextRobPos);
                
                nextid++;
            }
        }
    }
}
