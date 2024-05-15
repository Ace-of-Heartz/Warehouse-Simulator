using System;
using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model.Sim.Tests
{
    [TestFixture]
    public class SimGoalManagerUnitTest
    {
        private SimGoalManager _golieMan;
        private Map _33Map;

        [SetUp]
        public void Setup()
        {
            _golieMan = new();
            string[] input = {"h 3","w 3","map","...","...","..."};
            _33Map = new();
            _33Map.CreateMap(input);
        }

        [TestCase(1)]
        [TestCase(5)]
        public void AddNewGoal_ResultingGoalNumberIncrease(int addedGoalsNumber)
        {
            int oldN = _golieMan.GoalCount;
            
            for (int i = 0; i < addedGoalsNumber; ++i)
            {
                _golieMan.AddNewGoal(Vector2Int.one,_33Map);
            }

            int nIncrease = _golieMan.GoalCount - oldN;
            
            Assert.AreEqual(addedGoalsNumber,nIncrease);
        }

        [TestCase(-1, -1)]
        [TestCase(1, 1)]
        public void AddNewGoal_ResultingExceptionThrown(int x, int y)
        {
            string[] input = {"h 3","w 3","map","...",".@.","..."};
            _33Map = new();
            _33Map.CreateMap(input);
            Assert.Throws<ArgumentException>(() => _golieMan.AddNewGoal(new Vector2Int(x, y), _33Map));
        }

        [Test]
        public void GetNext_ResultingNonNullReturn()
        {
            _golieMan.AddNewGoal(Vector2Int.one,_33Map);
            Assert.AreNotEqual(null,_golieMan.GetNext());
        }
        
        [Test]
        public void GetNext_ResultingNullReturn()
        {
            Assert.AreEqual(null,_golieMan.GetNext());
        }
    }
}