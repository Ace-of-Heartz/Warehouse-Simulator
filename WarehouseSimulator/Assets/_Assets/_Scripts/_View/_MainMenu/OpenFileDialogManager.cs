using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using Assets.Scripts.View.MainMenu;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace WarehouseSimulator.View.MainMenu
{
    
    public class OpenFileDialogManager : MonoBehaviour
    {
        #region Fields
        
        public string pathToFile;

        [SerializeField]
        private List<InputExtension> inputFields;
        
        #endregion

        #region Properties
        /// <summary>
        /// Returns the path to designated file for opening.
        /// May return null if OpenFileDialog hasn't been called beforehand.
        /// </summary>
        public string PathToFile
        {
            get => pathToFile;
        } 
        
        #endregion
        
        #region Methods
        /// <summary>
        /// Virtual method inherited from Monobehaviour
        /// </summary>
        private void Start()
        {
            foreach (var field in inputFields)
            {
                field.inputField.onSelect.AddListener(delegate
                {
                    OpenFileDialog(field,EventArgs.Empty);
                });
            }
        }
        
        /// <summary>
        /// Handles opening the Unity File Dialog window
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentException"></exception>
        private void OpenFileDialog([CanBeNull] object obj , EventArgs args)
        {
            //Debug.Log("OpenFileDialog...");
            InputExtension inputExtension;
            if (obj is InputExtension)
                inputExtension = (InputExtension)obj;
            else
                throw new ArgumentException(); 
            
            pathToFile = EditorUtility.OpenFilePanel("Search file", "~/",inputExtension.extension );
            inputExtension.inputField.text = pathToFile;
            
        }
        #endregion
    }


}