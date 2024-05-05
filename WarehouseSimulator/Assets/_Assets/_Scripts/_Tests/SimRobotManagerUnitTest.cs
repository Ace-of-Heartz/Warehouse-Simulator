using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;
using ArgumentException = System.ArgumentException;

namespace WarehouseSimulator.Model.Sim.Tests
{
    public class TestingRobotManager : SimRobotManager
    {
        public void AddRobot(int i, Vector2Int pos)
        {
            SimRobot newR = new(i, pos);
            _allRobots.Add(newR);
            CustomLog.Instance.AddRobotStart(i, pos.x, pos.y, Direction.North);
        }
    }
    
    [TestFixture]
    public class SimRobotManagerUnitTest
    {
        private Map _emptyMap;
        private Map _33Map;
        private SimRobot _robie;
        private TestingRobotManager _robieMan;
        private SimGoalManager _golieMan;
        [SetUp]
        public void Setup()
        {
            _robie = new(0, Vector2Int.one);
            _robieMan = new();
            _golieMan = new();
            _emptyMap = new Map();
            string[] input = {"h 3","w 3","map","...","...","..."};
            _33Map = new Map();
            _33Map.CreateMap(input);
            CustomLog.Instance.Init();
        }

        [Test]
        public void AssignTasksToFreeRobots_ResultingEventInvoked()
        {
            _robieMan.GoalAssignedEvent += Handler;
            _golieMan.AddNewGoal(Vector2Int.one,_33Map);
            _robieMan.AddRobot(0,Vector2Int.one);
            _robieMan.AddRobot(1,Vector2Int.left);
            _robieMan.AssignTasksToFreeRobots(_golieMan);

            void Handler(object sender, EventArgs e)
            {
                Assert.IsTrue(true);
            }
        }
        
        [Test]
        public void AssignTasksToFreeRobots_ResultingEventNotInvoked()
        {
            _robieMan.GoalAssignedEvent += Handler;
            _robieMan.AddRobot(0,Vector2Int.one);
            _robieMan.AddRobot(1,Vector2Int.left);
            _robieMan.AssignTasksToFreeRobots(_golieMan);

            void Handler(object sender, EventArgs e)
            {
                Assert.IsTrue(false);
            }
            Assert.IsTrue(true);
        }
        
        [UnityTest]
        public IEnumerator CheckValidSteps_ResultingArgumentExceptionThrown()
        { 
            //Magic?
            Dictionary<SimRobot, Stack<RobotDoing>> dicc = 
                new() { { _robie, new Stack<RobotDoing>() } };
        
            var runner = _robieMan.CheckValidSteps(dicc,_emptyMap);
        
            yield return new WaitUntil(() => runner.IsCompleted);
            
            Assert.IsTrue(runner.IsFaulted);
            Assert.IsTrue(runner.Exception?.InnerExceptions[0] is ArgumentException);
        }

    // [TestCase(RobotDoing.Wait)]
    // [TestCase(RobotDoing.Timeout)]
    // [TestCase(RobotDoing.Rotate90)]
    // [TestCase(RobotDoing.RotateNeg90)]
    // [TestCase(RobotDoing.Forward)]
    private static RobotDoing[] vars = { RobotDoing.Wait, RobotDoing.Timeout, RobotDoing.Forward, RobotDoing.Rotate90, RobotDoing.RotateNeg90 }; 
    
    [UnityTest]
    public IEnumerator CheckValidStep_ResultingErrorNone([ValueSource(nameof(vars))] RobotDoing whatToDo)
    {
        Dictionary<SimRobot, Stack<RobotDoing>> dicc = 
            new() { {_robie,new Stack<RobotDoing>(new []{whatToDo})} };
        _robieMan.AddRobot(_robie.Id,_robie.GridPosition);
        
        SimRobot robieTwo = new SimRobot(1, new Vector2Int(2,2));
        _robieMan.AddRobot(robieTwo.Id,robieTwo.GridPosition);
        dicc.Add(robieTwo,new Stack<RobotDoing>(new []{whatToDo}));

        var tasks = _robieMan.CheckValidSteps(dicc,_33Map);

        yield return new WaitUntil(() => tasks.IsCompleted);
        (Error happened,SimRobot robieFirst, SimRobot robieSecond) = tasks.Result;
        Assert.IsFalse(tasks.IsFaulted);
        Assert.AreEqual(Error.None,happened);
    }
    //
    //     [Test]
    //     public void CheckValidSteps_ResultingRunIntoWall()
    //     {
    //         //TODO => Blaaa: Adjust positions
    //         Dictionary<SimRobot, Stack<RobotDoing>> dicc = 
    //             new() { {_robie,new Stack<RobotDoing>(new [] { RobotDoing.Forward})} };
    //         _robieMan.AddRobot(_robie.Id,_robie.GridPosition);
    //         
    //         SimRobot robieTwo = new SimRobot(1, new Vector2Int(2,2));
    //         _robieMan.AddRobot(robieTwo.Id,robieTwo.GridPosition);
    //         dicc.Add(robieTwo,new Stack<RobotDoing>(new [] { RobotDoing.Forward }));
    //         
    //         (Error happened, SimRobot who, SimRobot whoer) boop = Asyncer().Result;
    //         
    //         Assert.AreEqual(Error.None,boop.happened);
    //
    //         async Task<(Error, SimRobot, SimRobot)> Asyncer()
    //         {
    //             return await _robieMan.CheckValidSteps(dicc,_33Map);
    //         }
    //     }
    }
}