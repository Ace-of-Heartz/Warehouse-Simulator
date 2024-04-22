using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model.Sim;

namespace _Assets._Scripts._Tests
{
    [TestFixture]
    public class SimGoalManagerUnitTest
    {
        private SimGoalManager _golieMan;

        [SetUp]
        public void Setup()
        {
            _golieMan = new();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void AddNewGoal_ResultingGoalNumberIncrease(int addedGoalsNumber)
        {
            int oldN = _golieMan.GoalCount;
            
            for (int i = 0; i < addedGoalsNumber; ++i)
            {
                _golieMan.AddNewGoal(Vector2Int.one);
            }

            int nIncrease = _golieMan.GoalCount - oldN;
            
            Assert.AreEqual(addedGoalsNumber,nIncrease);
        }

        [Test]
        public void GetNext_ResultingNonNullReturn()
        {
            _golieMan.AddNewGoal(Vector2Int.one);
            Assert.AreNotEqual(null,_golieMan.GetNext());
        }
        
        [Test]
        public void GetNext_ResultingNullReturn()
        {
            Assert.AreEqual(null,_golieMan.GetNext());
        }
    }
}