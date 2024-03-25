using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private int _id;
    private Vector2 _gridPosition;
    private Direction _heading;
    [CanBeNull] private Goal _goal;
    private ERobotState _state;

    public Robot(int i,Vector2 gPos, Direction h, Goal g, ERobotState s)
    {
        _id = i;
        _gridPosition = gPos;
        _heading = h;
        _goal = g;
        _state = s;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
