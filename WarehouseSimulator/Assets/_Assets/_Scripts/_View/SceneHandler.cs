using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class SceneHandler
    {
        private Dictionary<uint, string> m_scenes;
        private Dictionary<uint, UIDocument> m_documents;
        private uint m_currentSceneId;
        
        public string CurrentScene
        {
            get => m_scenes[m_currentSceneId];
        }

        public uint CurrentSceneID
        {
            get => m_currentSceneId;
        }

        public UIDocument CurrentDoc
        {
            get => m_documents[m_currentSceneId];
        }
        
        private static SceneHandler m_instance;
        
        private SceneHandler()
        {
            m_scenes = new();
            m_documents = new();
        }

        public static SceneHandler GetInstance()
        {
            if (m_instance is null)
            {
                m_instance = new();
            }

            return m_instance;
        }

        public void AddSceneNDoc( uint id ,string scene, UIDocument doc)
        {
            GetInstance().m_scenes.Add(id,scene);
            GetInstance().m_documents.Add(id, doc);
        }

        public string GetSceneOfID(uint id)
        {
            return GetInstance().m_scenes[id];
        }

        public void SetCurrentScene(uint id)
        {
            m_currentSceneId = id;
            UpdateDocs(); 
        }

        public void UpdateDocs()
        {
            foreach(var k in m_documents.Keys )
            {
                if (m_currentSceneId != k)
                {
                    m_documents[k].rootVisualElement.style.display = DisplayStyle.None;
                }
                else
                    m_documents[k].rootVisualElement.style.display = DisplayStyle.Flex;
    
            }
        }
    }
}