using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using _Assets._Scripts._View._MainMenu;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View.MainMenu {
public class MainMenuManager : MonoBehaviour
{
    [FormerlySerializedAs("simScenePath")] public string _simScenePath;
    [FormerlySerializedAs("pbScenePath")] public string _pbScenePath;

    public static SimInputArgs simInputArgs;
    public static PbInputArgs pbInputArgs;
    
    /// <summary>
    /// Starts the simulation 
    /// </summary>
    public void StartSim()
    {
        CompleteSimInputArgs();
        if (!simInputArgs.IsComplete())
            throw new ArgumentException();
        else 
            SceneManager.LoadSceneAsync(_simScenePath);
        
    }

    private void CompleteSimInputArgs()
    {
        try
        {
            simInputArgs.ConfigFilePath = GameObject.Find("InputField_SimConfigFileLocation").GetComponent<TMP_InputField>().text;
            simInputArgs.NumberOfSteps = int.Parse(GameObject.Find("InputField_NumberOfSteps").GetComponent<TMP_InputField>().text);
            simInputArgs.IntervalOfSteps = int.Parse(GameObject.Find("InputField_IntervalOfSteps").GetComponent<TMP_InputField>().text);
            simInputArgs.PreparationTime = float.Parse(GameObject.Find("InputField_PreparationTime").GetComponent<TMP_InputField>().text);
            simInputArgs.EventLogPath = GameObject.Find("InputField_SimPathToEventLog").GetComponent<TMP_InputField>().text;
        }
        catch (Exception e)
        {
            Debug.Log("Fatal error occured at input parsing for simulation.");
        }
    }
    
    /// <summary>
    /// Starts the playback
    /// </summary>
    public void StartPlayback()
    {
        CompletePbInputArgs();
        if (!pbInputArgs.IsComplete())
            throw new ArgumentException();
        else 
            SceneManager.LoadSceneAsync(_pbScenePath);
        
    }

    private void CompletePbInputArgs()
    {
        try
        {
            simInputArgs.ConfigFilePath =
                GameObject.Find("InputField_PbConfigFileLocation").GetComponent<TMP_InputField>().text;
            simInputArgs.EventLogPath =
                GameObject.Find("InputField_SimPathToEventLog").GetComponent<TMP_InputField>().text;
        }
        catch (Exception e)
        {
            Debug.Log("Fatal error occured at input parsing for playback.");
        }
    }
    
    /// <summary>
    /// Exits the program
    /// </summary>
    public void ExitProgram()
    {
        Debug.Log("Exiting program...");
        Application.Quit();
    }
    

}




}