using System;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    /// <summary>
    /// Singleton class to be used as component through MonoBehaviour classes
    /// </summary>
    public class UIMessageManager
    {
        private static UIMessageManager m_uiUIMessageManagerInstance;

        private UIMessageManager()
        {
        }

        public static UIMessageManager GetInstance()
        {
            if (m_uiUIMessageManagerInstance is null)
                m_uiUIMessageManagerInstance = new();
            return m_uiUIMessageManagerInstance;
        }
        
        private UIDocument m_UIDocument;
        private VisualTreeAsset m_complexMessageBox;
        private VisualTreeAsset m_simpleMessageBox;
        private VisualTreeAsset m_oneWayMessageBox;

        public bool IsComplete()
        {
            return m_UIDocument is not null &&
                   m_complexMessageBox is not null &&
                   m_simpleMessageBox is not null &&
                   m_oneWayMessageBox is not null;
        }
        
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, ComplexMessageBoxTypeSelector type, string confirmText = "Confirm",string declineText = "Decline", string cancelText = "Cancel")
        {
            if (!IsComplete())
            {
                throw new NullReferenceException("Not all necessary components are present for popup windows");
            }

            if (m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea") is null)
            {
                throw new MissingVisualElementException();
            }
            new MessageBox(msg,onDone,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                m_complexMessageBox
                ); 
        }
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, SimpleMessageBoxTypeSelector type, string confirmText = "Confirm",string declineText = "Decline", string cancelText = "Cancel")
        {
            if (!IsComplete())
            {
                throw new NullReferenceException("Not all necessary components are present for popup windows");
            }
            if (m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea") is null)
            {
                throw new MissingVisualElementException();
            }
            new MessageBox(msg,onDone,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                m_simpleMessageBox
            ); 
        }
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, OneWayMessageBoxTypeSelector type, string confirmText = "Confirm",string declineText = "Decline", string cancelText = "Cancel")
        {
            if (!IsComplete())
            {
                throw new NullReferenceException("Not all necessary components are present for popup windows");
            }

            if (m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea") is null)
            {
                throw new MissingVisualElementException();
            }
            new MessageBox(msg,onDone,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                m_oneWayMessageBox
            ); 
        }

        public void SetUIDocument(UIDocument doc)
        {
            Debug.Log("Doc set");
            m_UIDocument = doc;
        }
        
        /// <summary>
        /// Set message boxes to use for 
        /// </summary>
        /// <param name="complexMessageBox"></param>
        /// <param name="simpleMessageBox"></param>
        /// <param name="oneWayMessageBox"></param>
        public void SetMessageBoxes(
            VisualTreeAsset complexMessageBox, 
            VisualTreeAsset simpleMessageBox,
            VisualTreeAsset oneWayMessageBox)
        {
            m_complexMessageBox = complexMessageBox;
            m_simpleMessageBox = simpleMessageBox;
            m_oneWayMessageBox = oneWayMessageBox;
        }


    }
}