using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model.PB;

namespace WarehouseSimulator.Model.Pb.Tests
{
    [TestFixture]
    public class PbGoalUnitTest
    {
        private PbGoal _golie;
        [SetUp]
        public void Setup()
        {
            _golie = new(0,Vector2Int.one);
        }

        [TestCase(-1, -1, 0)]
        [TestCase(10, 111, -6)]
        public void PropertyCheck(int x, int y, int id)
        {
            Vector2Int pos = new Vector2Int(x, y);
            _golie = new PbGoal(id, pos);

            Assert.AreEqual(pos, _golie.GridPosition);
            Assert.AreEqual(id, _golie.GoalID);
            Assert.AreEqual(null, _golie.Robot);
            Assert.AreEqual("0", _golie.RoboId);
        }

        [Test]
        public void SetAliveFrom_ResultingExceptionThrown()
        {
            _golie.SetAliveFrom(8,0);
            Assert.Throws<InvalidFileException>(() => _golie.SetAliveFrom(8,0));
        }
        
        [Test]
        public void SetAliveTo_ResultingExceptionThrown()
        {
            _golie.SetAliveTo(8);
            Assert.Throws<InvalidFileException>(() => _golie.SetAliveTo(8));
        }

        [Test]
        public void SetAliveFrom_ResultingPropertyChanged()
        {
            _golie.SetAliveFrom(9,5);
            Assert.AreEqual("6",_golie.RoboId);
        }

        [TestCase(22,false)]
        [TestCase(5, true)]
        public void SetTimeTo_ResultingEventInvoked(int time, bool isAlive)
        {
            _golie.SetAliveFrom(0,0);
            _golie.SetAliveTo(10);
            _golie.JesusEvent += Boop;
            _golie.SetTimeTo(time);

            void Boop(object sender, bool b)
            {
                Assert.IsTrue(b != isAlive);
            }
        }
    }
}