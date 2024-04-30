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
        private static UIMessageManager _uiUIMessageManagerInstance;

        private UIDocument _UIDocument;
        private VisualTreeAsset _complexMessageBox;
        private VisualTreeAsset _simpleMessageBox;
        private VisualTreeAsset _oneWayMessageBox;

        private bool _isMessageBoxOpen; 
        #endregion
        
        #region Properties

        /// <summary>
        /// Returns true if a message box is currently open, otherwise false.
        /// </summary>
        public bool IsMessageBoxOpen
        {
            get => _isMessageBoxOpen;
            private set {
                _isMessageBoxOpen = value;
                Debug.Log(_isMessageBoxOpen);
        }
        }
        
        #endregion 
        
        /// <summary>
        /// Private constructor for singleton instance.
        /// </summary>
        private UIMessageManager()
        {
            IsMessageBoxOpen = false;
        }
    
        /// <summary>
        /// Singleton instance getter.
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static UIMessageManager GetInstance()
        {
            if (_uiUIMessageManagerInstance == null)
                _uiUIMessageManagerInstance = new();
            return _uiUIMessageManagerInstance;
        }
        


        /// <summary>
        /// Returns true if all necessary components are present for message boxes to be displayed, otherwise false.
        /// </summary>
        /// <returns></returns>
        public bool IsComplete => _UIDocument is not null &&
                   _complexMessageBox is not null &&
                   _simpleMessageBox is not null &&
                   _oneWayMessageBox is not null;
        
        
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
                _UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                _complexMessageBox
                );

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
                _UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                _simpleMessageBox
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
                _UIDocument.rootVisualElement.Q<VisualElement>("PopupArea"),
                _oneWayMessageBox
            );

        }

        
        /// <summary>
        /// Set UIDocument to be used for message boxes.
        /// </summary>
        /// <param name="doc"></param>
        public void SetUIDocument(UIDocument doc)
        {
            _UIDocument = doc;
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
            _complexMessageBox = complexMessageBox;
            _simpleMessageBox = simpleMessageBox;
            _oneWayMessageBox = oneWayMessageBox;
        }

        /// <summary>
        /// Checks if all necessary components are present for message boxes to be displayed.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="MissingVisualElementException">Exception occurs when a necessary component is not present in the UI Document used.</exception>
        private void CheckComponentAvailability()
        {
            if (!IsComplete)
            {
                throw new NullReferenceException("Not all necessary components are present for popup windows");
            }

            if (_UIDocument.rootVisualElement.Q<VisualElement>("PopupArea") is null)
            {
                throw new MissingVisualElementException("PopupArea not found in UIDocument");
            }
            
        }




    }
}