using System;
using _Assets._Scripts._View;
using UnityEngine;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class UIMessageManager : MonoBehaviour
    {
        [SerializeField] private UIDocument m_UIDocument;

        [SerializeField] private VisualTreeAsset m_messageBox;

        public void MessageBox(string msg, Action<MessageBoxResponse> onDone)
        {
            new MessageBox(msg,onDone,m_UIDocument,m_messageBox);
        }
    }
}