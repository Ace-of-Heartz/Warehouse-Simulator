using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public Dictionary<Robot, RobotAction> AllRobots;

    private int nextId;

    public int NextId
    {
        get => nextId;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRobot(Vector2 pos, Direction h, int i)
    {
        Robot newR = new(i, pos, h, null, ERobotState.Free);
        AllRobots.Add(newR,RobotAction.Wait);
    }

    public void PerformRobotAction()
    {
        
    }

    public void AssignTasksToFreeRobots()
    {
        
    }
}
