using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   

    [SerializeField]
    public void StartSim()
    {
    }

    [SerializeField]
    public void StartPlayback()
    {
    }


    [SerializeField]
    public void ExitProgram()
    {
        Debug.Log("ExitProgram called");
        Application.Quit();
    }


    
}
