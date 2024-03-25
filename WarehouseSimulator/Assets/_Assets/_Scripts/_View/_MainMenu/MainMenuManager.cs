using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View.MainMenu {
public class MainMenuManager : MonoBehaviour
{
    public string simScenePath;
    public string pbScenePath;

    

    public void StartSim()
    {
        SceneManager.LoadSceneAsync(simScenePath);
    }

    public void StartPlayback()
    {
        SceneManager.LoadSceneAsync(pbScenePath);
    }
    
    public void ExitProgram()
    {
        Debug.Log("Exiting program...");
        Application.Quit();
    }
    

}




}