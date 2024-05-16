using System;
using System.Collections.Generic;
using NUnit.Framework;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.PB;
using WarehouseSimulator.Model.Structs;

namespace WarehouseSimulator.Model.Pb.Tests
{
    [TestFixture]
    public class PbRobotManagerUnitTest
    {
        PbRobotManager _robieMan;
        [SetUp]
        public void Setup()
        {
            _robieMan = new ();
        }

        [Test]
        public void SetUpAllRobots_ResultingEventInvoked()
        {
            List<RobotStartPos> startPositions = new() { 
                new(1, 1, Direction.North), 
                new(2, 2, Direction.South),
                new(0,1,Direction.East)
            };
            _robieMan.RobotAddedEvent += Blaaa;
            try
            {
                _robieMan.SetUpAllRobots(5,startPositions);
            } catch {/*ignored*/}
            
            void Blaaa(object sender, RobotCreatedEventArgs e)
            {
                Assert.IsTrue(sender is PbRobotManager);
                Assert.IsTrue(e.Robot is PbRobot);
            }
        }
    }
}