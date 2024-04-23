#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model.Sim
{
    public class CentralController
    {
        
        #region Fields
        private Dictionary<SimRobot, Stack<RobotDoing>> _plannedActions;
        private Dictionary<SimRobot,Vector2Int> _foolMeOnce;

        private IPathPlanner _pathPlanner;

        private Task? _taskBeforeNextStep;
        
        
        private bool _isPreprocessDone;
        private bool _isPathPlanningDone;
        #endregion

        /// <summary>
        /// Get returns true if all paths have been calculated for the robots, else false
        /// Set is private
        /// </summary>
        public bool IsPathPlanningDone
        {
            get => _isPathPlanningDone;
            private set => _isPathPlanningDone = value;
        }
        public bool IsPreprocessDone => _isPreprocessDone;
        
        /// <summary>
        /// Constructor of CentralController 
        /// </summary>
        /// <param name="map">Map loaded in from config file</param>
        public CentralController(Map map)
        {
            _pathPlanner = new AStar_PathPlanner(map);
            _plannedActions = new();
            _isPreprocessDone = false;
            _foolMeOnce = new();
        }
        
        public void AddPathPlanner(IPathPlanner pathPlanner)
        {
            _pathPlanner = pathPlanner;
        }
        
        /// <summary>
        /// Adds robot to dictionary of CentralController.
        /// Initializes the robot's planned moves with one Wait instruction.
        /// </summary>
        /// <param name="simRobot">Simulation robot model</param>
        public void AddRobotToPlanner(SimRobot simRobot)
        {
            var q = new Stack<RobotDoing>();
            q.Push(RobotDoing.Wait);
            _plannedActions.Add(simRobot, q);
        }
        
        /// <summary>
        /// Preprocess
        /// </summary>
        /// <param name="map"></param>
        public void Preprocess(Map map)
        {
            _isPreprocessDone = true;
        }
        
        /// <summary>
        /// Tries moving all robots according to their precalculated instructions.
        /// </summary>
        /// <param name="map">Map loaded in from config file</param>
        /// <param name="robieMan">SimRobotManager</param>
        public async void TimeToMove(Map map,SimRobotManager robieMan)
        {
            if (!(IsPathPlanningDone || IsPreprocessDone))
            {
                //TODO => Blaaa: If these commented logs are not needed anymore, let them fly freely into the afterlife
                //Debug.Log("Processes not finished until next step. Timeout sent.");
                
                foreach (var (_,actions) in _plannedActions)
                {
                    actions.Push(RobotDoing.Timeout);
                }
            }
            else
            {
                foreach (var (_, actions) in _plannedActions)
                {
                    if (actions.Count == 0) actions.Push(RobotDoing.Wait);
                }
            }

            (Error happened, SimRobot? robieTheFirst, SimRobot? robieTheSecond) results = await robieMan.CheckValidSteps(_plannedActions,map);
            //TODO => Blaaa: If these commented logs are not needed anymore, let them fly freely into the afterlife
            //Debug.Log($"Checking step result: {results.happened}");
            if (results.happened != Error.None)
            {
                foreach (var e in _plannedActions)
                {
                    CustomLog.Instance.AddRobotAction(e.Key.Id,RobotDoing.Wait);
                }
                //TODO => Blaaa: If these commented logs are not needed anymore, let them fly freely into the afterlife
                //Debug.Log($"RobFirst: {results.robieTheFirst.Id}, planner action: {_plannedActions[results.robieTheFirst].Peek()}");
                //Debug.Log($"RobSecond: {results.robieTheSecond.Id}, planner action: {_plannedActions[results.robieTheSecond].Peek()}");

                switch (results.happened)
                {
                    case Error.RunIntoWall:
                        if (_foolMeOnce.Keys.Contains(results.robieTheFirst))
                        {
                            _foolMeOnce[results.robieTheFirst!] = Vector2Int.one - 2 * Vector2Int.one;
                        }
                        else
                        {
                            _foolMeOnce.Add(results.robieTheFirst!,Vector2Int.one - 2 * Vector2Int.one);
                        }
                        break;
                    case Error.WantedToCrashIntoSomeoneNotMoving:
                    case Error.WantedToJumpOver:
                        if (!PokeIntoFoolMeOnce((results.robieTheFirst!, results.robieTheSecond!),
                                results.robieTheSecond!.GridPosition, 
                                results.robieTheFirst!.GridPosition))
                        {
                            if (!PokeIntoFoolMeOnce((results.robieTheSecond, results.robieTheFirst),
                                    results.robieTheFirst.GridPosition,
                                    results.robieTheSecond.GridPosition))
                            {
                                _foolMeOnce.Add(results.robieTheFirst,results.robieTheSecond.GridPosition);
                                _plannedActions[results.robieTheSecond] = new Stack<RobotDoing>
                                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                            }
                        }

                        break;
                    case Error.WantedToGoToTheSameField:
                        if (!PokeIntoFoolMeOnce((results.robieTheFirst!, results.robieTheSecond!),
                                results.robieTheSecond!.NextPos, 
                                results.robieTheFirst!.NextPos))
                        {
                            if (!PokeIntoFoolMeOnce((results.robieTheSecond, results.robieTheFirst),
                                    results.robieTheFirst.NextPos,
                                    results.robieTheSecond.NextPos))
                            {
                                _foolMeOnce.Add(results.robieTheFirst,results.robieTheSecond.NextPos);
                                _plannedActions[results.robieTheSecond] = new Stack<RobotDoing>
                                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                            }
                        }
                        break;
                }
            }
            else
            {
                 foreach (SimRobot robie in _plannedActions.Keys)
                 {
                     robie.MakeStep(map);
                 }

                 _foolMeOnce.Clear();
            }
        }
        
        /// <summary>
        /// Plan the move instructions for all robots present in the simulation.
        /// Async method.
        /// </summary>
        public async void PlanNextMovesForAllAsync()
        {
            if ((_taskBeforeNextStep == null ? TaskStatus.WaitingToRun : _taskBeforeNextStep.Status) == TaskStatus.Running)
            {
                _taskBeforeNextStep!.Wait();
            }
            
            IsPathPlanningDone = false;
            
            var robots = _plannedActions.Keys.ToList();
            var tasks = new List<Task>();

            foreach (var robot in robots)
            {
                if(_plannedActions[robot].Count == 0)
                    tasks.Add(PlanNextMoves(robot));
                else if (_foolMeOnce.Keys.Contains(robot))
                {
                    tasks.Add(PlanNextMoves(robot,_foolMeOnce[robot].x,_foolMeOnce[robot].y));
                }
            }

            _taskBeforeNextStep = Task.WhenAll(tasks);
            await _taskBeforeNextStep;
            

        }
        /// <summary>
        /// Plans a list of instructions for an individual robot with(out) taking the position of other robots into consideration. How rebellious.
        /// </summary>
        /// <param name="robot">The robot we plan the route for</param>
        /// <param name="x">The x coordinate of the DO NOT STEP HERE position</param>
        /// <param name="y">The y coordinate of the DO NOT STEP HERE position</param>
        private async Task PlanNextMoves(SimRobot robot, int x = -1, int y = -1)
        {
            
            if (_plannedActions[robot] == null)
            {
                _plannedActions[robot] = new Stack<RobotDoing>();
            }
            
            if (robot.RobotData.m_state == RobotBeing.Free)
            {
                _plannedActions[robot].Push(RobotDoing.Wait);
            }
            else
            {
                _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading, x, y);
            }
        }

        private bool PokeIntoFoolMeOnce((SimRobot robieTheFirst, SimRobot robieTheSecond) robies,Vector2Int whereNoToStepFirst,Vector2Int whereNoToStepSecond)
        {
            bool beenHere = false;
            if (_foolMeOnce.Keys.Contains(robies.robieTheFirst) && _foolMeOnce[robies.robieTheFirst] == whereNoToStepFirst) //if these robots previously tried to step already, but couldn't
            {
                _foolMeOnce.Remove(robies.robieTheFirst); //we shift who is waiting and who is trying to replan the route
                if (_foolMeOnce.Keys.Contains(robies.robieTheSecond))
                {
                    _foolMeOnce[robies.robieTheSecond] = whereNoToStepSecond;
                }
                else
                {
                    _foolMeOnce.Add(robies.robieTheSecond, whereNoToStepSecond);
                }
                _plannedActions[robies.robieTheFirst] = new Stack<RobotDoing>
                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                beenHere = true;
            } 
            else if (_foolMeOnce.Keys.Contains(robies.robieTheFirst)) //if the dictionary already contains our key with but not our value, we overwrite it
            {
                _foolMeOnce[robies.robieTheFirst] = whereNoToStepFirst;
                _plannedActions[robies.robieTheSecond] = new Stack<RobotDoing>
                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                beenHere = true;
            }

            return beenHere;
        }
        
    }
}