using System;
using System.Collections.Generic;
using System.Linq;
using _Assets._Scripts._View._MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WarehouseSimulator.View.Sim;

namespace WarehouseSimulator.View.MainMenu {
public class MainMenuManager : MonoBehaviour
{
    [FormerlySerializedAs("simScenePath")] public string _simScenePath;
    [FormerlySerializedAs("pbScenePath")] public string _pbScenePath;

    public static SimInputArgs simInputArgs;
    public static PbInputArgs pbInputArgs;

    public List<TMP_InputField> inputFieldsForSim;
    public List<TMP_InputField> inputFieldsForPb;

    private void Start()
    {
        
        GameObject.Find("Button_SimStart").GetComponent<Button>().interactable = false;
        GameObject.Find("Button_PbStart").GetComponent<Button>().interactable = false;
        
        foreach (var field in inputFieldsForSim)
        {
            field.onValueChanged.AddListener(delegate
            {
                UpdateSimInputStatus();
            });
        }

        foreach (var field in inputFieldsForPb)
        {
            field.onValueChanged.AddListener(delegate
            {
                UpdatePbInputStatus();
            });
        }
    }



    private void UpdatePbInputStatus()
    {
        if (inputFieldsForPb.All(a => !string.IsNullOrEmpty(a.text)))
        {
            GameObject.Find("Button_PbStart").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("Button_PbStart").GetComponent<Button>().interactable = false;
        }
    }

    private void UpdateSimInputStatus()
    {
        if (inputFieldsForSim.All(a => !string.IsNullOrEmpty(a.text)))
        {
            GameObject.Find("Button_SimStart").GetComponent<Button>().interactable = true;
        }
        else 
        {
            GameObject.Find("Button_SimStart").GetComponent<Button>().interactable = false;
        }
    }


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

    /// <summary>
    /// Completes the global static simInputArgs field
    /// </summary>
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
        catch (Exception)
        {
            GameObject.Find("UIErrorManager").GetComponent<UIMessageManager>().MessageBox("Fatal error occured!", response =>
                {
                    
                }
            );
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
    
    /// <summary>
    /// Completes the global static pbInputArgs field
    /// </summary>
    private void CompletePbInputArgs()
    {
        try
        { 
            pbInputArgs.ConfigFilePath = GameObject.Find("InputField_PbConfigFileLocation").GetComponent<TMP_InputField>().text;
            pbInputArgs.EventLogPath = GameObject.Find("InputField_PbPathToEventLog").GetComponent<TMP_InputField>().text;
        }
        catch (Exception)
        {
            
            GameObject.Find("UIErrorManager").GetComponent<UIMessageManager>().MessageBox("Fatal error occured!", response =>
                {
                    
                }
            );
            Debug.Log("Fatal error occured at input parsing for playback.");
        }
    }
    
    /// <summary>
    /// Exits the program
    /// </summary>
    public void ExitProgram()
    {
        GameObject.Find("UIErrorManager").GetComponent<UIMessageManager>().MessageBox("Quit application?", response =>
            {
                switch (response)
                {
                    case MessageBoxResponse.Confirmed: 
                        Application.Quit();
                        break;
                    default: break;
                }
            }
            );
        
    }
    

}




}