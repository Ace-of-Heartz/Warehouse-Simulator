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
        private Dictionary<SimRobot,Vector2Int?> _criticalRobots;

        private IPathPlanner _pathPlanner;

        private Task? _taskBeforeNextStep;
        
        
        private bool _isPreprocessDone;
        private bool _isPathPlanningDone;

        private int _criticalWaitingFor = 3;
        private int _robotsWaitingFor;

        private bool _solveDeadlocks;
        public bool SolveDeadlocks
        {
            get => _solveDeadlocks;
            set => _solveDeadlocks = value;
        }
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
            _criticalRobots = new();
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
        public async void TimeToMove(Map map, SimRobotManager robieMan)
        {
            if (!(IsPathPlanningDone || IsPreprocessDone))
            {
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

            (Error happened, SimRobot? firstRobot, SimRobot? secondRobot) results = await robieMan.CheckValidSteps(_plannedActions,map);

            if (results.happened != Error.None)
            {
                foreach (var e in _plannedActions)
                {
                    CustomLog.Instance.AddRobotAction(e.Key.Id,RobotDoing.Wait);
                }
                
                switch (results.happened)
                {
                    case Error.RAN_INTO_WALL:
                        if (_criticalRobots.Keys.Contains(results.firstRobot))
                        {
                            _criticalRobots[results.firstRobot!] = null;
                        }
                        else
                        {
                            _criticalRobots.Add(results.firstRobot!,null);
                        }
                        break;
                    case Error.RAN_INTO_PASSIVE_ROBOT:
                        break;
                    case Error.RAN_INTO_ACTIVE_ROBOT:
                    case Error.TRIED_SWAPPING_PLACES:
                        if (!ShiftCriticalRobots((results.firstRobot!, results.secondRobot!),
                                results.secondRobot!.GridPosition, 
                                results.firstRobot!.GridPosition))
                        {
                            if (!ShiftCriticalRobots((results.secondRobot, results.firstRobot),
                                    results.firstRobot.GridPosition,
                                    results.secondRobot.GridPosition))
                            {
                                _criticalRobots.Add(results.firstRobot,results.secondRobot.GridPosition);
                                _plannedActions[results.secondRobot] = new Stack<RobotDoing>
                                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                            }
                        }

                        break;
                    case Error.RAN_INTO_FIELD_OCCUPATION_CONFLICT:
                        if (!ShiftCriticalRobots((results.firstRobot!, results.secondRobot!),
                                results.secondRobot!.NextPos, 
                                results.firstRobot!.NextPos))
                        {
                            if (!ShiftCriticalRobots((results.secondRobot, results.firstRobot),
                                    results.firstRobot.NextPos,
                                    results.secondRobot.NextPos))
                            {
                                _criticalRobots.Add(results.firstRobot,results.secondRobot.NextPos);
                                _plannedActions[results.secondRobot] = new Stack<RobotDoing>
                                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                            }
                        }
                        break;
                }

                if(SolveDeadlocks)
                    ++_robotsWaitingFor;
            }
            else
            {
                 foreach (SimRobot robie in _plannedActions.Keys)
                 {
                     robie.MakeStep(map);
                 }

                 _robotsWaitingFor = 0;
                 _criticalRobots.Clear();
            }
        }
        
        /// <summary>
        /// Plan the move instructions for all robots present in the simulation.
        /// Async method.
        /// </summary>
        public async void PlanNextMovesForAllAsync(Map map)
        {
            if ((_taskBeforeNextStep == null ? TaskStatus.WaitingToRun : _taskBeforeNextStep.Status) == TaskStatus.Running)
            {
                _taskBeforeNextStep!.Wait();
            }
            
            IsPathPlanningDone = false;
            
            var robots = _plannedActions.Keys.ToList();
            var tasks = new List<Task>();

            
            if( _robotsWaitingFor > _criticalWaitingFor)
            {
                tasks.Add(Task.Run(() => AddNoiseToSteps(map)));
            }
            else
            {
                foreach (var robot in robots)
                {
                    if(_plannedActions[robot].Count == 0)
                        tasks.Add(Task.Run(() => PlanNextMoves(robot,null)));
                    else if (_criticalRobots.Keys.Contains(robot))
                    {
                        tasks.Add(Task.Run(() => PlanNextMoves(robot,_criticalRobots[robot])));
                    }
                }
            }

            _taskBeforeNextStep = Task.WhenAll(tasks);
            await _taskBeforeNextStep;
            

        }
        /// <summary>
        /// Plans a list of instructions for an individual robot with(out) taking the position of other robots into consideration. How rebellious.
        /// </summary>
        /// <param name="robot">The robot we plan the route for</param>
        /// <param name="disallowedPosition"></param>
        private void PlanNextMoves(SimRobot robot, Vector2Int? disallowedPosition)
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
                _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading,disallowedPosition);
            }
        }
        
        // private Stack<RobotDoing> PlanNextMoves(SimRobot robot, Vector2Int? disallowedPosition)
        // {
        //     
        //     if (_plannedActions[robot] == null)
        //     {
        //         return new Stack<RobotDoing>();
        //     }
        //     
        //     if (robot.RobotData.m_state == RobotBeing.Free)
        //     {
        //         var temp = _plannedActions[robot];
        //         temp.Push(RobotDoing.Wait);
        //         return temp;
        //     }
        //     else
        //     {
        //         return _pathPlanner.GetPath(robot.GridPosition,robot.Goal.GridPosition,robot.Heading,disallowedPosition);
        //     }
        // }

        private bool ShiftCriticalRobots((SimRobot robieTheFirst, SimRobot robieTheSecond) robies,Vector2Int whereNoToStepFirst,Vector2Int whereNoToStepSecond)
        {
            bool beenHere = false;
            if (_criticalRobots.Keys.Contains(robies.robieTheFirst) && _criticalRobots[robies.robieTheFirst] == whereNoToStepFirst) //if these robots previously tried to step already, but couldn't
            {
                _criticalRobots.Remove(robies.robieTheFirst); //we shift who is waiting and who is trying to replan the route
                if (_criticalRobots.Keys.Contains(robies.robieTheSecond))
                {
                    _criticalRobots[robies.robieTheSecond] = whereNoToStepSecond;
                }
                else
                {
                    _criticalRobots.Add(robies.robieTheSecond, whereNoToStepSecond);
                }
                _plannedActions[robies.robieTheFirst] = new Stack<RobotDoing>
                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                beenHere = true;
            } 
            else if (_criticalRobots.Keys.Contains(robies.robieTheFirst)) //if the dictionary already contains our key with but not our value, we overwrite it
            {
                _criticalRobots[robies.robieTheFirst] = whereNoToStepFirst;
                _plannedActions[robies.robieTheSecond] = new Stack<RobotDoing>
                    (new []{RobotDoing.Wait,RobotDoing.Wait,RobotDoing.Wait});
                beenHere = true;
            }

            return beenHere;
        }

        private void AddNoiseToSteps(Map map)
        {
            int r = 10;
            foreach (var robot in _plannedActions.Keys.ToList())
            {

                Vector2Int noise;
                do
                {
                    noise = new Vector2Int(Random.Range(-r, r+1), Random.Range(-r, r+1));
                } while (map.GetTileAt(noise) == TileType.Wall);
                
                if (robot.RobotData.m_state == RobotBeing.Free)
                {
                    _plannedActions[robot].Push(RobotDoing.Wait);
                }
                else
                {
                    _plannedActions[robot] = _pathPlanner.GetPath(robot.GridPosition,robot.GridPosition + noise,robot.Heading);
                }
            }
            _robotsWaitingFor = 0;
        }
        
        
        
        
        
    }
}