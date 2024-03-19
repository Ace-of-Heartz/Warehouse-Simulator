using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;


public class FileDialogManager : MonoBehaviour
{
   
    private string pathToFile;
 
    [SerializeField]
    private string extension;
    private string defaultName;
    
    public TMP_InputField inputField;

    [SerializeField]
    public void OpenFileDialog(){
        pathToFile = EditorUtility.OpenFilePanel("Search file", "~/",extension );
        inputField.text = pathToFile;
    }

    [SerializeField]
    public void SaveFileDialog(){
        pathToFile = EditorUtility.SaveFilePanel("Search file", "~/",defaultName, extension);
        inputField.text = pathToFile;
    }
}

