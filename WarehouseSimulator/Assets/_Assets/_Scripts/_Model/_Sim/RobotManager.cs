using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class RobotManager
    {
        private Dictionary<Robot, RobotDoing> AllRobots;

        [CanBeNull] public event EventHandler<RobotCreatedEventArgs> RobotAddedEvent;
        [CanBeNull] public event EventHandler<GoalAssignedEventArgs> GoalAssignedEvent;
        
        private void AddRobot(int i, Vector2Int pos)
        {
            Robot newR = new(i, pos);
            AllRobots.Add(newR,RobotDoing.Wait);
            newR.CallRobotPosEvent(this);
            RobotAddedEvent?.Invoke(this, new(newR));
        }
        
        public void PerformRobotAction(Map mapie)
        {
            foreach (var (robie, task) in AllRobots)
            {
                robie.PerformActionRequested(task, mapie);
            }
        }
    
        public void AssignTasksToFreeRobots(GoalManager from) //calling this is the responsibility of the SimulationManager
        {
            foreach (var (robie,_) in AllRobots)
            {
                if (robie.State == RobotBeing.Free)
                {
                    Goal next = from.GetNext();
                    if (next == null) { break; }
                    robie.AssignGoal(next);
                    GoalAssignedEvent?.Invoke(this, new(next));
                }
            }
        }
        
        public void RoboRead(string from, Map mapie)
        {
            using StreamReader rid = new(from);
            if (!int.TryParse(rid.ReadLine(), out int robn)) //
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
