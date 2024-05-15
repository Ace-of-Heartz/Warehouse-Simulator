using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.View.MainMenu
{
    /// <summary>
    /// Struct for wrapping data for input fields
    /// </summary>
    [Serializable]
    public struct InputExtension
    {
        /// <summary>
        /// The input field to be used for file dialog.
        /// </summary>
        [SerializeField] public TMP_InputField inputField;
        /// <summary>
        /// The extension of the file to be opened.
        /// </summary>
        [SerializeField] public string extension;
        /// <summary>
        /// The default name of the file to be opened. Mainly used for saving files.
        /// </summary>
        [SerializeField] public string defaultName;
    } 
}