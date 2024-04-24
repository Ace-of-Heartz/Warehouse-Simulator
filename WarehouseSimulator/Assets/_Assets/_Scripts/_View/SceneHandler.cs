using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class SceneHandler
    {
        #region Fields
        private Dictionary<uint, string> m_scenes;
        private Dictionary<uint, UIDocument> m_documents;
        private uint m_currentSceneId;
        #endregion
        
        private static SceneHandler m_instance;
        
        #region Properties
        /// <summary>
        /// Returns the current scene in use
        /// </summary>
        public string CurrentScene
        {
            get => m_scenes[m_currentSceneId];
        }

        /// <summary>
        /// Returns the current scene ID
        /// </summary>
        public uint CurrentSceneID
        {
            get => m_currentSceneId;
        }

        /// <summary>
        /// Returns the current UI document in use 
        /// </summary>
        public UIDocument CurrentDoc
        {
            get => m_documents[m_currentSceneId];
        }
        #endregion
        
        #region Constructor 
        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SceneHandler()
        {
            m_scenes = new();
            m_documents = new();
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the instance of the SceneHandler singleton
        /// </summary>
        /// <returns></returns>
        public static SceneHandler GetInstance()
        {
            if (m_instance is null)
            {
                m_instance = new();
            }

            return m_instance;
        }

        /// <summary>
        /// Adds a scene and an associated document to the SceneHandler
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scene"></param>
        /// <param name="doc"></param>
        public void AddSceneNDoc( uint id ,string scene, UIDocument doc)
        {
            GetInstance().m_scenes.Add(id,scene);
            GetInstance().m_documents.Add(id, doc);
        }
        
        /// <summary>
        /// Gets the scene associated with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSceneOfID(uint id)
        {
            return GetInstance().m_scenes[id];
        }
        
        /// <summary>
        /// Gets the document associated with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static UIDocument GetDocOfID(uint id)
        {
            return GetInstance().m_documents[id];
        }

        /// <summary>
        /// Sets the current scene to the given ID and updates the UI documents accordingly
        /// </summary>
        /// <param name="id"></param>
        public void SetCurrentScene(uint id)
        {
            m_currentSceneId = id;
            UpdateDocs(); 
        }

        /// <summary>
        /// Updates the UI documents to display the UI associated with the current scene
        /// Disables all other UI documents 
        /// </summary>
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
        #endregion
    }
}