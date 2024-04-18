using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.Sim
{
    public class CustomLog
    {
        private static CustomLog instance;
        public static CustomLog Instance => instance ??= new CustomLog();
        private CustomLog() { Init(); }
        
        
        private string actionModel = null;
        private bool allValid = true;
        private int teamSize = 0;
        private List<RobotStartPos> startPos = null;
        private int taskCompletedCount = 0;
        private int sumOfCost = 0;
        private int makespan = 0;
        private Dictionary<int, String> robotActions = null;
        private Dictionary<int, String> plannerActions = null;
        private List<double> plannerTimes = null;
        private List<LogError> errors = null;
        private Dictionary<int, List<EventInfo>> taskEvents = null;
        private List<TaskInfo> taskData = null;

        public void Init()
        {
            actionModel = null;
            allValid = true;
            teamSize = 0;
            startPos = new List<RobotStartPos>();
            taskCompletedCount = 0;
            sumOfCost = 0;
            makespan = 0;
            robotActions = new Dictionary<int, String>();
            plannerActions = new Dictionary<int, String>();
            plannerTimes = new List<double>();
            errors = new List<LogError>();
            taskEvents = new Dictionary<int, List<EventInfo>>();
            taskData = new List<TaskInfo>();
        }

        public void DummyData()
        {
            actionModel = "cica";
            allValid = false;
            teamSize = 2;
            startPos = new List<RobotStartPos>();
            startPos.Add(new RobotStartPos(5, 5, Direction.North));
            startPos.Add(new RobotStartPos(1, 0, Direction.West));
            taskCompletedCount = 2;
            sumOfCost = 10;
            makespan = 10;
            robotActions = new Dictionary<int, String>();
            robotActions.Add(0, "FFRRFFLLFF");
            robotActions.Add(1, "FFRRFFLLFF");
            plannerActions = new Dictionary<int, String>();
            plannerActions.Add(0, "FFRRFFLLFF");
            plannerActions.Add(1, "FFRRFFLLFF");
            plannerTimes = new List<double>();
            plannerTimes.Add(0.1);
            plannerTimes.Add(0.1);
            errors = new List<LogError>();
            errors.Add(new LogError(0, 1, 5, "F"));
            taskEvents = new Dictionary<int, List<EventInfo>>();
            taskEvents.Add(0, new List<EventInfo>());
            taskEvents[0].Add(new EventInfo(0, 0, "spawned"));
            taskEvents[0].Add(new EventInfo(0, 1, "finished"));
            taskEvents.Add(1, new List<EventInfo>());
            taskEvents[1].Add(new EventInfo(1, 0, "spawned"));
            taskEvents[1].Add(new EventInfo(1, 1, "finished"));
            taskData = new List<TaskInfo>();
            taskData.Add(new TaskInfo(0, 0, 0));
            taskData.Add(new TaskInfo(1, 15, 8));
        }
        
        //CAUTION: NOT YET FINISHED
        public void SaveLog(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            //actionModel
            sb.Append($"\"actionModel\":\"{actionModel}\",");
            //allValid
            string valid = allValid ? "true" : "false";
            sb.Append($"\"allValid\":{valid},");
            //teamSIze
            sb.Append($"\"teamSize\":{teamSize},");
            //start(robot start positions)
            sb.Append("\"start\":[");
            for (int i = 0; i < startPos.Count; i++)
            {
                string heading = "";
                switch (startPos[i].heading)
                {
                    case Direction.North:
                        heading = "N";
                        break;
                    case Direction.East:
                        heading = "E";
                        break;
                    case Direction.South:
                        heading = "S";
                        break;
                    case Direction.West:
                        heading = "W";
                        break;
                }
                sb.Append($"[{startPos[i].x},{startPos[i].y},\"{heading}\"]");
                if (i != startPos.Count - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //numTasksFinished
            sb.Append($"\"numTasksFinished\":{taskCompletedCount},");
            //sumOfCosts
            sb.Append($"\"sumOfCosts\":{sumOfCost},");
            //makespan
            sb.Append($"\"makespan\":{makespan},");
            //actualPaths
            sb.Append("\"actualPaths\":[");
            for (int i = 0; i < teamSize; i++)
            {
                sb.Append($"\"{robotActions[i]}\"");
                if (i != teamSize - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //plannerPaths
            sb.Append("\"plannerPaths\":[");
            for (int i = 0; i < teamSize; i++)
            {
                sb.Append($"\"{plannerActions[i]}\"");
                if (i != teamSize - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //errors
            sb.Append("\"errors\":[");
            for (int i = 0; i < errors.Count; i++)
            {
                sb.Append($"[{errors[i].robot1},{errors[i].robot2},{errors[i].step},\"{errors[i].action}\"]");
                if (i != errors.Count - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //events
            sb.Append("\"events\":[");
            for (int i = 0; i < teamSize; i++)
            {
                sb.Append("[");
                for (int j = 0; j < taskEvents[i].Count; j++)
                {
                    sb.Append($"[{taskEvents[i][j].Task},{taskEvents[i][j].Step},\"{taskEvents[i][j].WhatHappened}\"]");
                    if (j != taskEvents[i].Count - 1)
                        sb.Append(",");
                }
                sb.Append("]");
                if (i != teamSize - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //tasks(task spawn and id)
            sb.Append("\"tasks\":[");
            for (int i = 0; i < taskData.Count; i++)
            {
                sb.Append($"[{taskData[i].Task},{taskData[i].X},{taskData[i].Y}]");
                if (i != taskData.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]");
            sb.Append("}");

            String json = sb.ToString();
            using StreamWriter writer = new StreamWriter(path);
            writer.Write(json);
        }
        
        public void SetActionModel(string actionModel)
        {
            this.actionModel = actionModel;
        }
        
        public void AddRobotStart(int id, int x, int y, Direction heading)
        {
            startPos.Add(new RobotStartPos(x, y, heading));
            robotActions.Add(id, "");
            plannerActions.Add(id, "");
            taskEvents.Add(id, new List<EventInfo>());
            teamSize++;
        }
        
        public void TaskEvents(int robotId, int taskId, int step, string action)
        {
            if(action == "finished")
                taskCompletedCount++;
            taskEvents[robotId].Add(new EventInfo(taskId, step, action));
        }
        
        public void AddTaskData(int taskId, int x, int y)
        {
            taskData.Add(new TaskInfo(taskId, x, y));
        }
        
        public void AddSimulationStep()
        {
            makespan++;
        }
        
        public void AddError(int robot1, int robot2, int step, string action)
        {
            errors.Add(new LogError(robot1, robot2, step, action));
            allValid = false;
        }
        
        public void AddRobotAction(int robot, string action)
        {
            sumOfCost++;
            robotActions[robot] += action;
        }
        
        public void AddPlannerAction(int robot, string action)
        {
            plannerActions[robot] += action;
        }
        
        public void AddPlannerTime(double time)
        {
            plannerTimes.Add(time);
        }
    }
}