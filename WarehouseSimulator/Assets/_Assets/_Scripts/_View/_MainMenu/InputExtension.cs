using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.View.MainMenu
{
    [Serializable]
    public struct InputExtension
    {
        [SerializeField] public TMP_InputField inputField;
        [SerializeField] public string extension;
        [SerializeField] public string defaultName;
    } 
}