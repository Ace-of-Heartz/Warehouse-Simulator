using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View.MainMenu {
public class MainMenuManager : MonoBehaviour
{

    public static SimInputArgs simInputArgs;
    public static PbInputArgs pbInputArgs;

    public List<TMP_InputField> inputFieldsForSim;
    public List<TMP_InputField> inputFieldsForPb;

    private void Start()
    {
        
        GameObject.Find("Button_SimStart").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("Button_PbStart").GetComponent<UnityEngine.UI.Button>().interactable = false;
        
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
            GameObject.Find("Button_PbStart").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            GameObject.Find("Button_PbStart").GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    private void UpdateSimInputStatus()
    {
        if (inputFieldsForSim.All(a => !string.IsNullOrEmpty(a.text)))
        {
            GameObject.Find("Button_SimStart").GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else 
        {
            GameObject.Find("Button_SimStart").GetComponent<UnityEngine.UI.Button>().interactable = false;
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
        {
            SceneHandler.GetInstance().SetCurrentScene(1);
            SceneManager.LoadSceneAsync(SceneHandler.GetInstance().CurrentScene);
            UIMessageManager.GetInstance().SetUIDocument(SceneHandler.GetInstance().CurrentDoc);
        }
        
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
            simInputArgs.PreparationTime = int.Parse(GameObject.Find("InputField_PreparationTime").GetComponent<TMP_InputField>().text);
            simInputArgs.EventLogPath = GameObject.Find("InputField_SimPathToEventLog").GetComponent<TMP_InputField>().text;
            var res = GameObject.Find("Dropdown_SearchAlgorithm").GetComponent<TMP_Dropdown>().value;
            simInputArgs.SearchAlgorithm = res == 1 ? SEARCH_ALGORITHM.A_STAR :
                res == 2 ? SEARCH_ALGORITHM.COOP_A_STAR : SEARCH_ALGORITHM.BFS;
            simInputArgs.EnableDeadlockSolving = GameObject.Find("Toggle_EnableDeadlockSolve").GetComponent<UnityEngine.UI.Toggle>().isOn;

        }
        catch (Exception e)
        {
            
            UIMessageManager.GetInstance().MessageBox("Fatal error occured!\n" + e.Message,
                response => { },
                new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK)
            );
            //Debug.Log("Fatal error occured at input parsing for simulation.");
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
        {
            SceneHandler.GetInstance().SetCurrentScene(2);
            SceneManager.LoadSceneAsync(SceneHandler.GetInstance().CurrentScene);
            UIMessageManager.GetInstance().SetUIDocument(SceneHandler.GetInstance().CurrentDoc);
        }
    }
    
    /// <summary>
    /// Completes the global static pbInputArgs field
    /// </summary>
    private void CompletePbInputArgs()
    {
        try
        { 
            pbInputArgs.MapFilePath = GameObject.Find("InputField_PbMapFileLocation").GetComponent<TMP_InputField>().text;
            pbInputArgs.EventLogPath = GameObject.Find("InputField_PbPathToEventLog").GetComponent<TMP_InputField>().text;
        }
        catch (Exception e)
        {
            
            UIMessageManager.GetInstance().MessageBox("Fatal error occured!\n" + e.Message, response =>
                {
                    
                },
                new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK)
                );
            //Debug.Log("Fatal error occured at input parsing for playback.");
        }
    }
    
    /// <summary>
    /// Exits the program
    /// </summary>
    public void ExitProgram()
    {
        UIMessageManager.GetInstance().MessageBox("Quit application?", response =>
            {
                switch (response)
                {
                    case MessageBoxResponse.CONFIRMED: 
                        Application.Quit();
                        break;
                    case MessageBoxResponse.CANCELED:
                        break;
                    case MessageBoxResponse.DECLINED:
                        break;
                }
            },
            new SimpleMessageBoxTypeSelector(SimpleMessageBoxTypeSelector.MessageBoxType.OK_CANCEL)
            );
        
    }
    

}




}