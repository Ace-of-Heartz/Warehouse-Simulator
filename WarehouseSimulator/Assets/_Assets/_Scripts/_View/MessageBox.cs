using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;




namespace WarehouseSimulator.View
{

    
    public class MessageBox
    {
        public Action<MessageBoxResponse> m_done;
        public bool IsDone { get; private set; }

        private VisualElement m_uiContainer;
        private VisualElement m_ui;
        
        public MessageBox(
            string msg,
            Action<MessageBoxResponse> onDone,
            MessageBoxTypeSelector type,
            VisualElement uiContainer,
            VisualTreeAsset uiAsset
            )
        {
            IsDone = false;
            m_uiContainer = uiContainer;
            m_ui = uiAsset.Instantiate();

            m_ui.Q<Label>("InfoLabel").text = msg;

            string[] texts = type.GetButtonText();
            
            if (type is SimpleMessageBoxTypeSelector)
                SetupButtons((SimpleMessageBoxTypeSelector)type, texts);
            else if (type is ComplexMessageBoxTypeSelector)
                SetupButtons((ComplexMessageBoxTypeSelector) type,texts);
            else if (type is OneWayMessageBoxTypeSelector)
                SetupButtons((OneWayMessageBoxTypeSelector) type, texts);

            uiContainer.Add(m_ui);
            
            m_done = onDone;
        }

        private void SetupButtons(SimpleMessageBoxTypeSelector type,string[] texts)
        {
            
            m_ui.Q<Button>("ConfirmButton").text = texts[0];
            m_ui.Q<Button>("ConfirmButton").clicked += Confirm;
            
            m_ui.Q<Button>("CancelButton").text = texts[1];
            m_ui.Q<Button>("CancelButton").clicked  += Cancel;
        }

        private void SetupButtons(ComplexMessageBoxTypeSelector type, string[] texts)
        {
            m_ui.Q<Button>("ConfirmButton").text = texts[0];
            m_ui.Q<Button>("ConfirmButton").clicked += Confirm;
            
            m_ui.Q<Button>("DeclineButton").text = texts[1];
            m_ui.Q<Button>("DeclineButton").clicked += Decline;
            
            m_ui.Q<Button>("CancelButton").text = texts[2];
            m_ui.Q<Button>("CancelButton").clicked  += Cancel;
        }

        private void SetupButtons(OneWayMessageBoxTypeSelector type, string[] texts)
        {
            m_ui.Q<Button>("ConfirmButton").text = texts[0];
            m_ui.Q<Button>("ConfirmButton").clicked += Confirm;
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
            

            m_done?.Invoke(response);
            m_uiContainer.Remove(m_ui);
           
        }
    }
}