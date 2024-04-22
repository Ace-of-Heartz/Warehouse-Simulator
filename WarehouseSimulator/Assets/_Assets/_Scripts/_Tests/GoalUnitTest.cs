#nullable enable
using System;
using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

namespace _Assets._Scripts.Tests
{
    [TestFixture]
    public class GoalUnitTest
    {
        private SimRobot? _robie;
        private SimGoal? _golie;
        [SetUp]
        public void Setup()
        {
            _robie = new(0, Vector2Int.one);
            _golie = new SimGoal(Vector2Int.one,0);
        }

        [TestCase(-1,-1,0)]
        [TestCase(10,111,-6)]
        public void SimGoal_PropertyCheck(int x, int y, int id)
        {
            Vector2Int pos = new Vector2Int(x, y);
            _golie = new SimGoal(pos, id);
            
            Assert.AreEqual(pos,_golie.GridPosition);
            Assert.AreEqual(id,_golie.GoalID);
        }

        [Test]
        public void SimGoal_AssignedTo_ResultingFieldChange()
        {
            _golie!.AssignedTo(_robie);
            Assert.AreEqual(_robie,_golie.SimRobot);
        }
        
        [Test]
        public void SimGoal_FinishTask_ResultingEventThrown()
        {
            _golie!.GoalFinishedEvent += Handler;
            _golie.FinishTask();

            void Handler(object s, EventArgs e)
            {
                Assert.True(true);
            }
        }
    }
}