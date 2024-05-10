using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.View.MainMenu;
using JetBrains.Annotations;
using SimpleFileBrowser;


namespace WarehouseSimulator.View.MainMenu
{
    
    public class OpenFileDialogManager : MonoBehaviour
    {
        #region Fields
        
        public string m_pathToFile;

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
            get => m_pathToFile;
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
            
            FileBrowser.SetFilters(true,
                inputExtension.extension.ToString().ToUpper(),
                inputExtension.extension.ToString().ToLower());
            FileBrowser.SetDefaultFilter(inputExtension.extension.ToString().ToLower());
            
            if (!FileBrowser.ShowLoadDialog(
                    (path =>
                    {
                        m_pathToFile = path[0];  //Debug.Log(path[0]); 
                        inputExtension.inputField.text = m_pathToFile;
                    }),
                    (() => { m_pathToFile = "";  }),
                    FileBrowser.PickMode.Files,
                    false,
                    "G:\\Uni\\4th_semester\\soft_tech\\sample_files", //TODO: Change this to "~\\User"
                    "",
                    "Load Config File",
                    "Select"
                ))
            {
               
                UIMessageManager.GetInstance().MessageBox(
                    "Fatal error occured! File explore could not be opened!",
                    response => { }
                    ,new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
            } 
            
            
            
        }
        #endregion
    }


}