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
        public new void AddRobot(SimRobot robie)
        {
            base.AddRobot(robie);
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
            string[] input = { "h 3", "w 3", "map", "...", "...", "..." };
            _33Map = new Map();
            _33Map.CreateMap(input);
            CustomLog.Instance.Init();
        }

        [Test]
        public void AssignTasksToFreeRobots_ResultingEventInvoked()
        {
            _robieMan.GoalAssignedEvent += Handler;
            _golieMan.AddNewGoal(Vector2Int.one, _33Map);
            _robieMan.AddRobot(_robie);
            SimRobot robieTwo = new(69, Vector2Int.up);
            _robieMan.AddRobot(robieTwo);
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
            _robieMan.AddRobot(_robie);
            SimRobot robieTwo = new(666,Vector2Int.one);
            _robieMan.AddRobot(robieTwo);
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
            //Unity async handling
            Dictionary<SimRobot, RobotDoing> dicc =
                new() { { _robie, RobotDoing.Wait } };

            var runner = _robieMan.CheckValidSteps(dicc, _emptyMap);

            yield return new WaitUntil(() => runner.IsCompleted);

            Assert.IsTrue(runner.IsFaulted);
            Assert.IsTrue(runner.Exception?.InnerExceptions[0] is ArgumentException);
        }
        
        private static RobotDoing[] _vars =
            { RobotDoing.Forward, RobotDoing.Wait, RobotDoing.Timeout, RobotDoing.Rotate90, RobotDoing.RotateNeg90 };

        [UnityTest]
        public IEnumerator CheckValidStep_ResultingNoError([ValueSource(nameof(_vars))] RobotDoing whatToDo)
        {
            Dictionary<SimRobot, RobotDoing> dicc =
                new() { { _robie, whatToDo } };
            _robieMan.AddRobot(_robie);

            SimRobot robieTwo = new SimRobot(1, new Vector2Int(2, 2));
            _robieMan.AddRobot(robieTwo);
            dicc.Add(robieTwo, whatToDo);

            var tasks = _robieMan.CheckValidSteps(dicc, _33Map);

            yield return new WaitUntil(() => tasks.IsCompleted);
            bool isValidStep = tasks.Result;
            Assert.IsFalse(tasks.IsFaulted);
            Assert.AreEqual(true, isValidStep);
        }

        [UnityTest]
        public IEnumerator CheckValidSteps_ResultingError_BecauseRobotRanIntoTheWall()
        {
            string[] input = { "h 3", "w 3", "map", ".@.", "...", "..." };
            _33Map = new();
            _33Map.CreateMap(input);
            Dictionary<SimRobot, RobotDoing> dicc =
                new() { { _robie, RobotDoing.Forward } };
            _robieMan.AddRobot(_robie);

            var tasks = _robieMan.CheckValidSteps(dicc, _33Map);

            yield return new WaitUntil(() => tasks.IsCompleted);
            bool isValidStep = tasks.Result;
            Assert.IsFalse(tasks.IsFaulted);
            Assert.AreEqual(false, isValidStep);   
        }
        
        [UnityTest]
        public IEnumerator CheckValidSteps_ResultingError_BecauseRobotsWantedToStepToTheSameField()
        {
            Dictionary<SimRobot, RobotDoing> dicc =
                new() { { _robie, RobotDoing.Forward } };
            _robieMan.AddRobot(_robie);
            SimRobot robieTwo = new(1, new Vector2Int(0, 0), Direction.West);
            
            dicc.Add(robieTwo,RobotDoing.Forward);
            _robieMan.AddRobot(robieTwo);
            var tasks = _robieMan.CheckValidSteps(dicc, _33Map);

            yield return new WaitUntil(() => tasks.IsCompleted);
            bool isValidStep = tasks.Result;
            Assert.IsFalse(tasks.IsFaulted);
            Assert.AreEqual(false, isValidStep);   
        }
    }
}