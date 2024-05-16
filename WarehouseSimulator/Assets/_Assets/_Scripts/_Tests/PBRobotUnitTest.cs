using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.PB;

namespace WarehouseSimulator.Model.Pb.Tests
{
    [TestFixture]
    public class PbRobotUnitTest
    {
        private PbRobot _robie;
        [SetUp]
        public void Setup()
        {
            _robie = new(0, Vector2Int.one, 2);   
        }
        
        [TestCase(0,1,1,Direction.North,RobotBeing.InTask)]
        [TestCase(-2,0,1,Direction.East)]
        [TestCase(99,1,0,Direction.South)]
        [TestCase(42,34,65,Direction.West)]
        public void PropertiesCheck_ResultingCorrectBehaviour(int id, int x, int y, Direction headin,
            RobotBeing st = RobotBeing.Free)
        {
            Vector2Int startPos = new(x, y);
            _robie = new PbRobot(id,startPos,1,headin,st);
            
            Assert.AreEqual(id,_robie.Id);
            Assert.AreEqual(startPos,_robie.GridPosition);
            Assert.AreEqual(headin,_robie.Heading);
            Assert.AreEqual(null,_robie.Goal);
            Assert.AreEqual(st,_robie.State);
        }

        [TestCase(6)]
        [TestCase(-5)]
        public void SetTimeTo_ResultingExceptionThrown(int stateindex)
        {
            Assert.Throws<ArgumentException>(() => _robie.SetTimeTo(stateindex));
        }
        
        [TestCase(RobotDoing.Forward,RobotDoing.RotateNeg90,RobotDoing.Forward,RobotDoing.Forward,Direction.East,3,0)]
        [TestCase(RobotDoing.Wait,RobotDoing.Timeout,RobotDoing.Wait,RobotDoing.Wait,Direction.North,1,1)]
        [TestCase(RobotDoing.Rotate90,RobotDoing.RotateNeg90,RobotDoing.Rotate90,RobotDoing.Rotate90,Direction.South,1,1)]
        public void SetTimeTo_ResultingCorrectBehaviour_IndirectlyCheckingCalcTimeLineCorrectBehaviour(RobotDoing fstaction, RobotDoing sndaction, 
            RobotDoing thraction,RobotDoing frthaction,Direction expectedHeading,int x, int y)
        {
            _robie = new PbRobot(0, Vector2Int.one, 4);
            List<RobotDoing> actions = new (){ fstaction,sndaction, thraction, frthaction};
            _robie.CalcTimeLine(actions);
            _robie.SetTimeTo(4);
            Assert.AreEqual(new Vector2Int(x,y),_robie.GridPosition);
            Assert.AreEqual(expectedHeading,_robie.Heading);
        }

        [Test]
        public void CalcTimeLine_ResultingExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => _robie.CalcTimeLine(new List<RobotDoing>()));
        }
    }
}