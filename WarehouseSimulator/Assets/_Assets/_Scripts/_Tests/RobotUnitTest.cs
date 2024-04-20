#nullable enable
using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;
using Direction = WarehouseSimulator.Model.Enums.Direction;

public class RobotUnitTest
{
    private Map mockMap;
    [SetUp]
    public void Setup()
    {
        mockMap = new Map();
    }

    [TestCase(Direction.North, Direction.East)]
    [TestCase(Direction.East, Direction.South)]
    [TestCase(Direction.South, Direction.West)]
    [TestCase(Direction.West, Direction.North)]
    public void SimRobot_TurningClockwise_ResultingCorrectBehaviour(Direction starting, Direction result)
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,starting);

        instance.TryPerformActionRequested(RobotDoing.RotateNeg90, mockMap);
        
        Assert.AreEqual(result,instance.Heading);
    }
    
    [TestCase(Direction.North, Direction.West)]
    [TestCase(Direction.West, Direction.South)]
    [TestCase(Direction.South, Direction.East)]
    [TestCase(Direction.East, Direction.North)]
    public void SimRobot_TurningCounterClockwise_ResultingCorrectBehaviour(Direction starting, Direction result)
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,starting);

        instance.TryPerformActionRequested(RobotDoing.Rotate90, mockMap);
        
        Assert.AreEqual(result,instance.Heading);
    }

    [TestCase(0,Direction.North,RobotBeing.InTask)]
    [TestCase(-2,Direction.East)]
    [TestCase(99,Direction.South)]
    [TestCase(42,Direction.West)]
    public void SimRobot_PropertiesCheck_ResultingCorrectBehaviour(int val,Direction headin,RobotBeing st = RobotBeing.Free)
    {
        int id = val;
        Vector2Int startPos = Vector2Int.one;
        Direction heading = headin;
        SimGoal? golie = null;
        RobotBeing state = st;
        
        SimRobot robie = new SimRobot(id,startPos,heading,golie,state);
        
        Assert.AreEqual(id,robie.Id);
        Assert.AreEqual(startPos,robie.GridPosition);
        Assert.AreEqual(heading,robie.Heading);
        Assert.AreEqual(golie,robie.Goal);
        Assert.AreEqual(state,robie.State);
        Assert.AreEqual(new Vector2Int(-1,-1),robie.NextPos);
    }
    
        

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator RobotUnitTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
