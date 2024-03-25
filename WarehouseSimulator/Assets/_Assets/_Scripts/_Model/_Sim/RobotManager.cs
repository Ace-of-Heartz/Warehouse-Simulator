using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class RobotManager
    {
        public Dictionary<Robot, RobotDoing> AllRobots;
        private int nextId;
    
        public int NextId
        {
            get => nextId;
        }
        
    
        private void AddRobot(Vector2 pos, Direction h, int i)
        {
            Robot newR = new(i, pos, h, null, RobotBeing.Free);
            AllRobots.Add(newR,RobotDoing.Wait);
        }
    
        public void PerformRobotAction()
        {
            
        }
    
        public void AssignTasksToFreeRobots()
        {
            
        }
        
        public void RoboRead(string from)
        {
            using StreamReader rid = new(from);
            if (!int.TryParse(rid.ReadLine(), out int robn)) //
            {
                throw new InvalidDataException("Invalid file format: First line not a number");
            }
    
            string temp = rid.ReadLine();
            while (temp != null)
            {
                if (robn <= 0)
                {
                    throw new InvalidDataException("Invalid file format: there were too many lines");
                }
                
                if (!int.TryParse(rid.ReadLine(), out int newRobPos)) //
                {
                    throw new InvalidDataException("Invalid file format: line not a number");
                }
                robn--;
                temp = rid.ReadLine();
            }
    
        }    
    }
}
