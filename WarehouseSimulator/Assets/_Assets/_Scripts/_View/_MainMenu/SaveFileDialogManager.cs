using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.View.MainMenu
{
    public class SaveFileDialogManager : MonoBehaviour
    {
        #region Fields
        
        [CanBeNull] private string _pathToFile;

        [SerializeField]
        private List<InputExtension> inputFields;
        
        #endregion

        #region Properties
        /// <summary>
        /// Returns the path to saved file destination.
        /// May return null if SaveFileDialog method hasn't been called beforehand.
        /// </summary>
        public string PathToFile
        {
            get => _pathToFile;
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
                    SaveFileDialog(field,EventArgs.Empty);
                });
            }
        }
        
        /// <summary>
        /// Handles opening the Unity File Dialog window
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SaveFileDialog([CanBeNull] object obj , EventArgs args)
        {
            //Debug.Log("OpenFileDialog...");
            InputExtension inputExtension;
            if (obj is InputExtension)
                inputExtension = (InputExtension)obj;
            else
                throw new ArgumentException(); 
            
            _pathToFile = EditorUtility.SaveFilePanel("Search file", "~/",inputExtension.defaultName,inputExtension.extension );
            inputExtension.inputField.text = _pathToFile;
            
        }
        #endregion
    }

}