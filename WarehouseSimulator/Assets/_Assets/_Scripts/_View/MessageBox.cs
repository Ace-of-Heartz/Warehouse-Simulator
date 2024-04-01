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

        private UIDocument m_uiDocument;
        private VisualElement m_ui;
        
        public MessageBox(string msg, Action<MessageBoxResponse> onDone, UIDocument uiDocument,VisualTreeAsset uiAsset)
        {
            IsDone = false;
            m_uiDocument = uiDocument;
            m_ui = uiAsset.Instantiate();

            m_ui.Q<Label>("InfoLabel").text = msg;
            m_ui.Q<Button>("ConfirmButton").clicked += Confirm;
            m_ui.Q<Button>("DeclineButton").clicked += Decline;
            m_ui.Q<Button>("CancelButton").clicked  += Cancel;

            m_uiDocument.rootVisualElement.Add(m_ui);
            
            m_done = onDone;
        }

        private void Confirm()
        {
            SetDone(MessageBoxResponse.Confirmed);
        }

        private void Decline()
        {
            SetDone(MessageBoxResponse.Declined);
        }

        private void Cancel()
        {
            SetDone(MessageBoxResponse.Canceled);
        }
        
        private void SetDone(MessageBoxResponse response)
        {
            if (IsDone) return;

            IsDone = true;
            

            m_done?.Invoke(response);
            m_uiDocument.rootVisualElement.Remove(m_ui);
           
        }
    }
}