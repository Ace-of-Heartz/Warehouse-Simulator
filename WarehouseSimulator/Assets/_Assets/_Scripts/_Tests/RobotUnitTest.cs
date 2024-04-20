using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

public class RobotUnitTest
{
    [Test]
    public void SimRobot_TurningClockwise_FromNorth_ToEast_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.North,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.RotateNeg90, mockMap);
        
        Assert.AreEqual(Direction.East,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningClockwise_FromEast_ToSouth_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.East,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.RotateNeg90, mockMap);
        
        Assert.AreEqual(Direction.South,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningClockwise_FromSouth_ToWest_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.South,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.RotateNeg90, mockMap);
        
        Assert.AreEqual(Direction.West,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningClockwise_FromWest_ToNorth_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.West,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.RotateNeg90, mockMap);
        
        Assert.AreEqual(Direction.North,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningCounterClockwise_FromNorth_ToWest_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.North,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.Rotate90, mockMap);
        
        Assert.AreEqual(Direction.West,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningCounterClockwise_FromWest_ToSouth_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.West,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.Rotate90, mockMap);
        
        Assert.AreEqual(Direction.South,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningCounterClockwise_FromSouth_ToEast_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.South,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.Rotate90, mockMap);
        
        Assert.AreEqual(Direction.East,instance.Heading);
    }
    
    [Test]
    public void SimRobot_TurningCounterClockwise_FromEast_ToNorth_ResultingCorrectBehaviour()
    {
        SimRobot instance = new SimRobot(0,Vector2Int.one,Direction.East,null,RobotBeing.Free);
        Map mockMap = new Map();

        instance.TryPerformActionRequested(RobotDoing.Rotate90, mockMap);
        
        Assert.AreEqual(Direction.North,instance.Heading);
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
