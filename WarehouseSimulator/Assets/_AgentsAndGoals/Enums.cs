using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RobotAction
{
    Wait,
    Forward,
    Rotate90,
    RotateNeg90,
    Timeout
}    
public enum Direction
{
    North,
    West,
    East,
    South
}    
public enum ERobotState
{
    Free,
    InTask
}

public class Enums : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
