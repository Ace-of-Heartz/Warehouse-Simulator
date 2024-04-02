using System;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class UIMessageManager : MonoBehaviour
    {
        [SerializeField] private UIDocument m_UIDocument;

        [SerializeField] private VisualTreeAsset m_complexMessageBox;
        [SerializeField] private VisualTreeAsset m_simpleMessageBox;
        [SerializeField] private VisualTreeAsset m_oneWayMessageBox;

        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, ComplexMessageBoxTypeSelector type, string confirmText = "Confirm",string declineText = "Decline", string cancelText = "Cancel")
        {
            new MessageBox(msg,onDone,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("MessageContainer"),
                m_complexMessageBox
                ); 
        }
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, SimpleMessageBoxTypeSelector type, string confirmText = "Confirm",string declineText = "Decline", string cancelText = "Cancel")
        {
            new MessageBox(msg,onDone,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("MessageContainer"),
                m_simpleMessageBox
            ); 
        }
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, OneWayMessageBoxTypeSelector type, string confirmText = "Confirm",string declineText = "Decline", string cancelText = "Cancel")
        {
            new MessageBox(msg,onDone,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("MessageContainer"),
                m_oneWayMessageBox
            ); 
        }


        
    }
}