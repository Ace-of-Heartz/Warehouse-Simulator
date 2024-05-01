using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;




namespace WarehouseSimulator.View
{

    
    public class MessageBox
    {
        public List<Action<MessageBoxResponse>> _doneList;
        public bool IsDone { get; private set; }

        private VisualElement _uiContainer;
        private VisualElement _ui;
        /// <summary>
        /// Instantiates a messagebox.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="onDoneList"></param>
        /// <param name="type"></param>
        /// <param name="uiContainer"></param>
        /// <param name="uiAsset"></param>
        public MessageBox(
            string msg,
            List<Action<MessageBoxResponse>> onDoneList,
            MessageBoxTypeSelector type,
            VisualElement uiContainer,
            VisualTreeAsset uiAsset
            )
        {
            IsDone = false;
            _uiContainer = uiContainer;
            _ui = uiAsset.Instantiate();

            _ui.Q<Label>("InfoLabel").text = msg;

            string[] texts = type.GetButtonText();
            
            if (type is SimpleMessageBoxTypeSelector)
                SetupButtons((SimpleMessageBoxTypeSelector)type, texts);
            else if (type is ComplexMessageBoxTypeSelector)
                SetupButtons((ComplexMessageBoxTypeSelector) type,texts);
            else if (type is OneWayMessageBoxTypeSelector)
                SetupButtons((OneWayMessageBoxTypeSelector) type, texts);

            uiContainer.Add(_ui);
            
            _doneList = onDoneList;
        }

        private void SetupButtons(SimpleMessageBoxTypeSelector type,string[] texts)
        {
            
            _ui.Q<Button>("ConfirmButton").text = texts[0];
            _ui.Q<Button>("ConfirmButton").clicked += Confirm;
            
            _ui.Q<Button>("CancelButton").text = texts[1];
            _ui.Q<Button>("CancelButton").clicked  += Cancel;
        }

        private void SetupButtons(ComplexMessageBoxTypeSelector type, string[] texts)
        {
            _ui.Q<Button>("ConfirmButton").text = texts[0];
            _ui.Q<Button>("ConfirmButton").clicked += Confirm;
            
            _ui.Q<Button>("DeclineButton").text = texts[1];
            _ui.Q<Button>("DeclineButton").clicked += Decline;
            
            _ui.Q<Button>("CancelButton").text = texts[2];
            _ui.Q<Button>("CancelButton").clicked  += Cancel;
        }

        private void SetupButtons(OneWayMessageBoxTypeSelector type, string[] texts)
        {
            _ui.Q<Button>("ConfirmButton").text = texts[0];
            _ui.Q<Button>("ConfirmButton").clicked += Confirm;
        }


        private void Confirm()
        {
            SetDone(MessageBoxResponse.CONFIRMED);
        }

        private void Decline()
        {
            SetDone(MessageBoxResponse.DECLINED);
        }

        private void Cancel()
        {
            SetDone(MessageBoxResponse.CANCELED);
        }
        
        private void SetDone(MessageBoxResponse response)
        {
            if (IsDone) return;

            IsDone = true;
            
            
            _doneList?.ForEach(onDone => onDone?.Invoke(response)); 
            _uiContainer.Remove(_ui);
           
        }
    }
}