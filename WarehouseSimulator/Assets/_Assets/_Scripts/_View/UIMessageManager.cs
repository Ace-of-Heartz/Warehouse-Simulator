using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    /// <summary>
    /// Singleton class to be used as component through MonoBehaviour classes
    /// </summary>
    public class UIMessageManager
    {
        #region Fields
        private static UIMessageManager m_uiUIMessageManagerInstance;

        private bool m_isMessageBoxOpen; 
        #endregion
        
        #region Properties

        public bool IsMessageBoxOpen
        {
            get => m_isMessageBoxOpen;
            private set => m_isMessageBoxOpen = value;
        }
        
        #endregion 
        
        private UIMessageManager()
        {
            IsMessageBoxOpen = false;
        }

        public static UIMessageManager GetInstance()
        {
            if (m_uiUIMessageManagerInstance == null)
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
        
        /// <summary>
        /// Used for calling a message box on screen with 1 button.
        /// Can only result in "MessageBoxResponse.CONFIRMED", "MessageBoxResponse.CANCELED" and "MessageBoxResponse.DECLINED" state.
        /// </summary>
        /// <example><code>  UIMessageManager.GetInstance().MessageBox(
        ///            "Fatal error occured! File explore could not be opened!",
        ///            response => {
        ///             switch (response){
        ///                 case MessageBoxResponse.CONFIRMED: Debug.Log("Confirmed"); break;
        ///                 case MessageBoxResponse.CANCELED: Debug.Log("Canceled"); break;
        ///                 case MessageBoxResponse.DECLINED: Debug.Log("Declined"); break;
        ///             }
        ///             }
        ///            ,new ComplexMessageBoxTypeSelector(ComplexMessageBoxTypeSelector.MessageBoxType.CONFIRM_DECLINE_CANCEL));
        ///                </code></example>
        /// <param name="msg">Message to be displayed for the user.</param>
        /// <param name="onDone">Action to be executed upon any button click. Will receive result of message box query.</param>
        /// <param name="type">Type of MessageBox to be used.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="MissingVisualElementException"></exception>
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, ComplexMessageBoxTypeSelector type)
        {
            CheckComponentAvailability();
            
            if (IsMessageBoxOpen)
            {
                return;
            }
            
            IsMessageBoxOpen = true;
            
            var onDoneList = new List<Action<MessageBoxResponse>>(){(_) => { IsMessageBoxOpen = false; },onDone};
            var mb = new MessageBox(msg,onDoneList,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                m_complexMessageBox
                );
            mb._doneList.Add( (_) =>
            {
                IsMessageBoxOpen = false;
            });
        }
        
        /// <summary>
        /// Used for calling a message box on screen with 1 button.
        /// Can only result in "MessageBoxResponse.CONFIRMED" and "MessageBoxResponse.CANCELED" state.
        /// </summary>
        /// 
        /// <example>
        /// <code>UIMessageManager.GetInstance().MessageBox(
        ///            "Fatal error occured! File explore could not be opened!",
        ///            response => {
        ///             switch (response){
        ///                 case MessageBoxResponse.CONFIRMED: Debug.Log("Confirmed"); break;
        ///                 case MessageBoxResponse.CANCELED: Debug.Log("Canceled"); break;
        ///             }
        ///             }
        ///            ,new SimpleMessageBoxTypeSelector(SimpleMessageBoxTypeSelector.MessageBoxType.OK_CANCEL));
        ///                </code></example>
        /// <param name="msg">Message to be displayed for the user.</param>
        /// <param name="onDone">Action to be executed upon any button click. Will receive result of message box query.</param>
        /// <param name="type">Type of MessageBox to be used.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="MissingVisualElementException"></exception>
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, SimpleMessageBoxTypeSelector type)
        {
            CheckComponentAvailability();
            
            if (IsMessageBoxOpen)
            {
                return;
            }
            
            IsMessageBoxOpen = true;

            var onDoneList = new List<Action<MessageBoxResponse>>(){(_) => { IsMessageBoxOpen = false; },onDone};
            var mb = new MessageBox(msg,onDoneList,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                m_simpleMessageBox
            ); 
            
        }
        /// <summary>
        /// Used for calling a message box on screen with 1 button.
        /// Can only result in "MessageBoxResponse.CONFIRMED" state.
        /// </summary>
        /// <example><code>  UIMessageManager.GetInstance().MessageBox(
        ///            "Fatal error occured! File explore could not be opened!",
        ///            response => { }
        ///            ,new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
        ///                </code></example>
        /// <param name="msg">Message to be displayed for the user.</param>
        /// <param name="onDone">Action to be executed upon any button click. Will receive result of message box query.</param>
        /// <param name="type">Type of MessageBox to be used.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="MissingVisualElementException"></exception>
        public void MessageBox(string msg, Action<MessageBoxResponse> onDone, OneWayMessageBoxTypeSelector type)
        {
            CheckComponentAvailability();
            
            if (IsMessageBoxOpen)
            {
                return;
            }
            
            IsMessageBoxOpen = true;

            var onDoneList = new List<Action<MessageBoxResponse>>(){(_) => { IsMessageBoxOpen = false; },onDone};
            var mb = new MessageBox(msg,onDoneList,
                type,
                m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                m_oneWayMessageBox
            );
            mb._doneList.Add((_) =>
            {
                IsMessageBoxOpen = false;
            });
        }

        public void SetUIDocument(UIDocument doc)
        {
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

        private void CheckComponentAvailability()
        {
            if (!IsComplete())
            {
                throw new NullReferenceException("Not all necessary components are present for popup windows");
            }

            if (m_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea") is null)
            {
                throw new MissingVisualElementException();
            }
            
        }




    }
}