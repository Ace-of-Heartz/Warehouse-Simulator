using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model
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
        private int stepsCompleted = 0;
        private Dictionary<int, String> robotActions = null;
        private Dictionary<int, String> plannerActions = null;
        private List<double> plannerTimes = null;
        private List<LogError> errors = null;
        private Dictionary<int, List<EventInfo>> taskEvents = null;
        private List<TaskInfo> taskData = null;

        #region Property

        public int TeamSize => teamSize;
        public List<RobotStartPos> StartPos => startPos;
        public int StepsCompleted => stepsCompleted;
        public Dictionary<int, String> RobotActions => robotActions;
        public Dictionary<int, List<EventInfo>> TaskEvents => taskEvents;
        public List<TaskInfo> TaskData => taskData;

        #endregion

        public void Init()
        {
            actionModel = null;
            allValid = true;
            teamSize = 0;
            startPos = new List<RobotStartPos>();
            taskCompletedCount = 0;
            sumOfCost = 0;
            stepsCompleted = 0;
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
            stepsCompleted = 10;
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
        
        /// <summary>
        /// Saves logged data to a file
        /// </summary>
        /// <param name="path"></param>
        public void SaveLog(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            //actionModel
            sb.Append($"\"actionModel\":\"{actionModel}\",");
            //allValid
            string valid = allValid ? "Yes" : "No";
            sb.Append($"\"AllValid\":{valid},");
            //teamSize
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
                sb.Append($"[{startPos[i].y},{startPos[i].x},\"{heading}\"]");
                if (i != startPos.Count - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //numTaskFinished
            sb.Append($"\"numTaskFinished\":{taskCompletedCount},");
            //sumOfCost
            sb.Append($"\"sumOfCost\":{sumOfCost},");
            //makespan
            sb.Append($"\"makespan\":{stepsCompleted},");
            //actualPaths
            sb.Append("\"actualPaths\":[");
            for (int i = 0; i < teamSize; i++)
            {
                sb.Append($"\"{ActionStringAddCommas(robotActions[i])}\"");
                if (i != teamSize - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //plannerPaths
            sb.Append("\"plannerPaths\":[");
            for (int i = 0; i < teamSize; i++)
            {
                sb.Append($"\"{ActionStringAddCommas(plannerActions[i])}\"");
                if (i != teamSize - 1)
                    sb.Append(",");
            }
            sb.Append("],");
            //plannerTimes
            sb.Append("\"plannerTimes\":[");
            for (int i = 0; i < plannerTimes.Count; i++)
            {
                string s = plannerTimes[i].ToString(CultureInfo.InvariantCulture);
                sb.Append(s);
                if (i != plannerTimes.Count - 1)
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
                sb.Append($"[{taskData[i].Task},{taskData[i].Y},{taskData[i].X}]");
                if (i != taskData.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]");
            sb.Append("}");

            String json = sb.ToString();
            using StreamWriter writer = new StreamWriter(path);
            writer.Write(json);
        }
        
        /// <summary>
        /// Load log file. Throws Exception if the file is not in the correct format
        /// </summary>
        /// <param name="path"></param>
        public void LoadLog(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string jsonString = reader.ReadToEnd();
            
            //preprocess json
            jsonString = jsonString.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            jsonString = jsonString.TrimStart('{').TrimEnd('}');
            
            string[] keyValCandidates = jsonString.Split(',');
            //split properties
            List<string> keyValuePairsList = new List<string>();
            foreach (string candidate in keyValCandidates)
            {
                if (candidate.Contains(":"))
                    keyValuePairsList.Add(candidate);
                else
                    keyValuePairsList[^1] += "," + candidate;
            }
            
            //make key-value dictionary
            Dictionary<string, string> keyValueDict = new Dictionary<string, string>();
            foreach (string keyValuePair in keyValuePairsList)
            {
                string[] parts = keyValuePair.Split(':');
                string key = parts[0].Trim('"');
                string value = parts[1];
                keyValueDict.Add(key, value);
            }
            
            //parse values
            Init();
            actionModel = keyValueDict["actionModel"].Trim('"');
            allValid = keyValueDict["AllValid"].Trim('"') == "Yes";
            teamSize = int.Parse(keyValueDict["teamSize"]);
            string[] start = keyValueDict["start"].Trim('[').Trim(']').Split("],[");
            foreach (string s in start)
            {
                string[] parts = s.Split(',');
                int y = int.Parse(parts[0]);
                int x = int.Parse(parts[1]);
                Direction heading = Direction.North;
                switch (parts[2].Trim('"'))
                {
                    case "N":
                        heading = Direction.North;
                        break;
                    case "E":
                        heading = Direction.East;
                        break;
                    case "S":
                        heading = Direction.South;
                        break;
                    case "W":
                        heading = Direction.West;
                        break;
                }
                startPos.Add(new RobotStartPos(x, y, heading));
            }
            taskCompletedCount = int.Parse(keyValueDict["numTaskFinished"]);
            sumOfCost = int.Parse(keyValueDict["sumOfCost"]);
            stepsCompleted = int.Parse(keyValueDict["makespan"]);
            string[] actualPaths = keyValueDict["actualPaths"].Trim('[').Trim(']').Split("\",");
            for (int i = 0; i < teamSize; i++)
            {
                robotActions.Add(i, CommafiedActionStringToActionString(actualPaths[i].Trim('\"')));
            }
            string[] plannerPaths = keyValueDict["plannerPaths"].Trim('[').Trim(']').Split("\",");
            for (int i = 0; i < teamSize; i++)
            {
                plannerActions.Add(i, CommafiedActionStringToActionString(plannerPaths[i].Trim('\"')));
            }
            string[] plannerTimesStr = keyValueDict["plannerTimes"].Trim('[').Trim(']').Split(",");
            foreach (string s in plannerTimesStr)
            {
                if (s == "") break;
                plannerTimes.Add(double.Parse(s, CultureInfo.InvariantCulture));
            }
            string[] errorsStr = keyValueDict["errors"].Trim('[').Trim(']').Split("],[");
            foreach (string s in errorsStr)
            {
                if (s == "") break;//empty list
                string[] parts = s.Split(',');
                int robot1 = int.Parse(parts[0]);
                int robot2 = int.Parse(parts[1]);
                int step = int.Parse(parts[2]);
                string action = parts[3].Trim('"');
                errors.Add(new LogError(robot1, robot2, step, action));
            }
            string eventS = keyValueDict["events"];
            if (eventS[0] == '[' && eventS[^1] == ']')
                eventS = eventS.Substring(1, eventS.Length - 2);
            List<String> eventInfosPerRobot = new List<String>();
            int level = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < eventS.Length; i++)
            {
                sb.Append(eventS[i]);
                if (eventS[i] == '[')
                {
                    level++;
                }
                else if (eventS[i] == ']')
                {
                    level--;
                    if (level == 0)
                    {
                        eventInfosPerRobot.Add(sb.ToString().Trim(new []{',', '[', ']'}));
                        sb.Clear();
                    }
                }
            }
            foreach(var robotEvents in eventInfosPerRobot)
            {
                string[] events = robotEvents.Split("],[");
                
                List<EventInfo> eventInfos = new List<EventInfo>();
                foreach (string e in events)
                {
                    if (e == "") break;//empty list
                    string[] parts1 = e.Split(',');
                    int task = int.Parse(parts1[0]);
                    int step = int.Parse(parts1[1]);
                    string action = parts1[2].Trim('"');
                    eventInfos.Add(new EventInfo(task, step, action));
                }
                taskEvents.Add(taskEvents.Count, eventInfos);
            }
            string[] tasksStr = keyValueDict["tasks"].Trim('[').Trim(']').Split("],[");
            foreach (string s in tasksStr)
            {
                if (s == "") break;//empty list
                string[] parts = s.Split(',');
                int task = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);
                int x = int.Parse(parts[2]);
                taskData.Add(new TaskInfo(task, x, y));
            }
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
        
        public void AddTaskEvent(int robotId, int taskId, string action)
        {
            if(action == "finished")
                taskCompletedCount++;
            taskEvents[robotId].Add(new EventInfo(taskId, stepsCompleted + 1, action));//assume current step to be the next step
        }
        
        public void AddTaskData(int taskId, int x, int y)
        {
            taskData.Add(new TaskInfo(taskId, x, y));
        }
        
        public void SimulationStepCompleted()
        {
            stepsCompleted++;
        }
        
        public void AddError(int robot1, int robot2)
        {
            errors.Add(new LogError(robot1, robot2, stepsCompleted + 1, "Invalid move"));
            allValid = false;
        }
        
        public void AddRobotAction(int robot, RobotDoing action)
        {
            sumOfCost++;
            robotActions[robot] += RobotActionToString(action);
        }
        
        public void AddPlannerAction(int robot, RobotDoing action)
        {
            plannerActions[robot] += RobotActionToString(action);
        }
        
        public void AddPlannerTime(double time)
        {
            plannerTimes.Add(time);
        }

        public List<RobotDoing> GetAllActions(int roboId)
        { 
            List<RobotDoing> res = new();
            string all = robotActions[roboId];
            char[] charactions = all.ToCharArray();
            foreach (char action in charactions)
            {
                res.Add(CharToRobotAction(action));
            }

            return res;
        }

        private RobotDoing CharToRobotAction(char action)
        {
            switch (action)
            {
                case 'F':
                    return RobotDoing.Forward;
                case 'C':
                    return RobotDoing.Rotate90;
                case 'R':
                    return RobotDoing.RotateNeg90;
                case 'W':
                    return RobotDoing.Wait;
                case 'T':
                    return RobotDoing.Timeout;
                default:
                    throw new InvalidFileException("The format of the log file was in an incorrect format: the robotactions must be one of the following letters:" +
                                                   "\n\"F\",\"C\",\"R\",\"W\",\"T\"");
            }
        }

        private string RobotActionToString(RobotDoing action)
        {
            switch (action)
            {
                case RobotDoing.Forward:
                    return "F";
                case RobotDoing.Rotate90:
                    return "C";
                case RobotDoing.RotateNeg90:
                    return "R";
                case RobotDoing.Wait:
                    return "W";
                case RobotDoing.Timeout:
                    return "T";
                default:
                    return "";
            }
        }

        private string ActionStringAddCommas(string s)
        {
            StringBuilder sb = new();
            for(int i = 0; i < s.Length; i++)
            {
                sb.Append(s[i]);
                if (i != s.Length - 1)
                    sb.Append(",");
            }
            return sb.ToString();
        }

        private string CommafiedActionStringToActionString(string s)
        {
            return s.Replace(",", "");
        }
    }
}