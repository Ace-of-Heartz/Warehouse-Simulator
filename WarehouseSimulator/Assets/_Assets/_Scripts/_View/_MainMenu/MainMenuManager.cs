using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View {
public class MainMenuManager : MonoBehaviour
{
    private string _pathToConfigFile;
    private string _pathToEventLog;

    private int _numberOfSteps;
    

        
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

    public async void GetJsonContent(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);
        while (!inp_stm.EndOfStream)
        {
            string file_content = await inp_stm.ReadToEndAsync();
        }



    }


}




}