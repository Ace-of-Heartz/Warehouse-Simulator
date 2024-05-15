#nullable enable
using System;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;
using Direction = WarehouseSimulator.Model.Enums.Direction;

namespace WarehouseSimulator.Model.Sim.Tests
{
    public class SimRobotUnitTest
    {
        private Map? _emptyMap;
        private Map? _33Map;
        private SimRobot? _robie;
        private SimGoal? _golie;
        private int i;
        [SetUp]
        public void Setup()
        {
            _emptyMap = new Map();
            _robie = new(0, Vector2Int.one);
            _golie = new SimGoal(Vector2Int.one,0);
            string[] input = {"h 3","w 3","map","...","...","..."};
            _33Map = new Map();
            _33Map.CreateMap(input);
            CustomLog.Instance.Init();
            CustomLog.Instance.AddRobotStart(_robie.Id,_robie.GridPosition.x,_robie.GridPosition.y,_robie.Heading);
            CustomLog.Instance.AddTaskData(_golie.GoalID,_golie.GridPosition.x,_golie.GridPosition.y);
            i = 0;
        }
        
        [TestCase(0,Direction.North,RobotBeing.InTask)]
        [TestCase(-2,Direction.East)]
        [TestCase(99,Direction.South)]
        [TestCase(42,Direction.West)]
        public void PropertiesCheck_ResultingCorrectBehaviour(int val,Direction headin,RobotBeing st = RobotBeing.Free)
        {
            int id = val;
            Vector2Int startPos = Vector2Int.one;
            Direction heading = headin;
            _golie = null;
            RobotBeing state = st;
            
            _robie = new SimRobot(id,startPos,heading,_golie,state);
            
            Assert.AreEqual(id,_robie.Id);
            Assert.AreEqual(startPos,_robie.GridPosition);
            Assert.AreEqual(heading,_robie.Heading);
            Assert.AreEqual(_golie,_robie.Goal);
            Assert.AreEqual(state,_robie.State);
            Assert.AreEqual(new Vector2Int(-1,-1),_robie.NextPos);
        }
    
        [Test]
        public void AssignGoal_ResultingCorrectBehaviour()
        {
            //CustomLog.Instance.AddRobotStart(_robie.Id,_robie.GridPosition.x,_robie.GridPosition.y,_robie.Heading);
            _robie!.AssignGoal(_golie!);
            
            Assert.AreEqual(_golie,_robie.Goal);
        }
        
        [Test]
        public void AssignGoal_ResultingInvalidOperationExceptionThrown()
        {
            _robie = new(0, Vector2Int.one, Direction.North,null,RobotBeing.InTask);
    
            _golie = new SimGoal(Vector2Int.one,0);
    
            Assert.Throws<InvalidOperationException>(() => _robie.AssignGoal(_golie));
        }
        
        [Test]
        public void AssignGoal_ResultingArgumentExceptionThrown()
        {
            _golie = null;
    
            Assert.Throws<ArgumentException>(() => _robie!.AssignGoal(_golie));
        }
    
        [Test]
        public void AssignGoal_ResultingChangesProperty()
        {
            _robie!.AssignGoal(_golie);
            Assert.AreEqual(RobotBeing.InTask,_robie.State);
        }
    
        [TestCase(Direction.North, Direction.East)]
        [TestCase(Direction.East, Direction.South)]
        [TestCase(Direction.South, Direction.West)]
        [TestCase(Direction.West, Direction.North)]
        public void TurningClockwise_ResultingCorrectBehaviour(Direction starting, Direction result)
        {
            _robie = new SimRobot(0,Vector2Int.one,starting);
    
            _robie.TryPerformActionRequested(RobotDoing.RotateNeg90, _emptyMap!);
            
            _robie.MakeStep(_emptyMap!);
            
            Assert.AreEqual(result,_robie.Heading);
        }
        
        [TestCase(Direction.North, Direction.West)]
        [TestCase(Direction.West, Direction.South)]
        [TestCase(Direction.South, Direction.East)]
        [TestCase(Direction.East, Direction.North)]
        public void TurningCounterClockwise_ResultingCorrectBehaviour(Direction starting, Direction result)
        {
            _robie = new SimRobot(0,Vector2Int.one,starting);
    
            _robie.TryPerformActionRequested(RobotDoing.Rotate90, _emptyMap!);
            
            _robie.MakeStep(_emptyMap!);
            
            Assert.AreEqual(result,_robie.Heading);
        }
    
        [Test]
        public void TryPerformActionRequested_ResultingArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _robie!.TryPerformActionRequested(RobotDoing.Wait,null!));
        }
    
        [TestCase(RobotDoing.Wait, 1, 1)]
        [TestCase(RobotDoing.Timeout, 1, 1)]
        [TestCase(RobotDoing.Rotate90,1,1)]
        [TestCase(RobotDoing.RotateNeg90,1,1)]
        [TestCase(RobotDoing.Forward,1,0)]
        public void TryPerformActionRequested_WallessMap_ResultingCorrectPositionChange(RobotDoing what, int resultX, int resultY)
        {
            Vector2Int expectedNextPos = new(resultX,resultY);
            
            (bool,SimRobot) res = (true, null!);
            Assert.AreEqual(res,_robie!.TryPerformActionRequested(what,_33Map!));
            
            _robie.MakeStep(_emptyMap!);
            
            Assert.AreEqual(expectedNextPos,_robie!.GridPosition);
        }
    
        [Test]
        public void TryPerformActionRequested_WallessMap_ResultingCorrectMapChange()
        {
            _robie!.TryPerformActionRequested(RobotDoing.Forward, _33Map!);
            
            _robie.MakeStep(_33Map!);
            
            Assert.AreEqual(TileType.RoboOccupied,_33Map!.GetTileAt(_robie!.GridPosition));
        }
    
        [TestCase(0,0)]
        [TestCase(1,1)]
        public void TryPerformActionRequested_ResultingInvalidStep(int startX, int startY) 
        {
            _robie = new(0,new Vector2Int(startX,startY));
            string[] input = {"h 3","w 3","map",".@.","...","..."};
            _emptyMap!.CreateMap(input);
            
            (bool,SimRobot) res = (false, _robie);
    
            Assert.AreEqual(res, _robie!.TryPerformActionRequested(RobotDoing.Forward, _emptyMap));
        }
    
        [Test]
        public void TryPerformActionRequested_ResultingGoalCompleted()
        {
            _golie = new SimGoal(new Vector2Int(1, 0), 0);
            _robie!.TryPerformActionRequested(RobotDoing.Forward,_33Map!);
            Assert.AreEqual(null!,_robie.Goal);
            Assert.AreEqual(RobotBeing.Free,_robie.State);
        }
    }
} 
