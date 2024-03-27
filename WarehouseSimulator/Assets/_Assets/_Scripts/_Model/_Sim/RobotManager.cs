using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class RobotManager
    {
        public Dictionary<Robot, RobotDoing> AllRobots;
        
        private void AddRobot(int i, Vector2Int pos)
        {
            Robot newR = new(i, pos);
            AllRobots.Add(newR,RobotDoing.Wait);
        }
    
        public void PerformRobotAction(Map mapie)
        {
            foreach (var (robie, task) in AllRobots)
            {
                robie.PerformActionRequested(task, mapie);
            }
        }
    
        public void AssignTasksToFreeRobots()
        {
            
        }
        
        public void RoboRead(string from, Vector2Int mapSize)
        {
            using StreamReader rid = new(from);
            if (!int.TryParse(rid.ReadLine(), out int robn)) //
            {
                throw new InvalidDataException("Invalid file format: First line not a number");
            }

            int nextid = 0;
            string line = rid.ReadLine();
            while (line != null)
            {
                if (nextid >= robn)
                {
                    throw new InvalidDataException("Invalid file format: there were too many lines");
                }

                if (!int.TryParse(rid.ReadLine(), out int linPos)) 
                {
                    throw new InvalidDataException($"Invalid file format: {nextid + 2}. line not a number");
                }

                int quot = linPos / mapSize.y;
                Vector2Int newRobPos = new(quot,linPos - mapSize.y * quot);
                AddRobot(nextid,newRobPos);
                
                nextid++;
                line = rid.ReadLine();
            }
        }
    }
}
