using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    /// <summary>
    /// Can't fret over every class, Jack
    /// </summary>
    public class MotherOfAllSingletons : MonoBehaviour
    {
        #region Singleton Fields
        private UIMessageManager m_UIMessageManager;
        private SceneHandler m_sceneHandler;
        private static MotherOfAllSingletons m_instance;
        #endregion


        #region MessageBox Fields
        [SerializeField] 
        private VisualTreeAsset m_complexMessageBox; 
        [SerializeField] 
        private VisualTreeAsset m_simpleMessageBox;
        [SerializeField] 
        private VisualTreeAsset m_oneWayMessageBox;
        #endregion 
        
        #region UIDocument and Scene Fields
        [SerializeField]
        private List<SceneNDoc> m_UIDocumentsAndScenes;
        
        #endregion 
        /// <summary>
        /// Initialize Singleton classes
        /// </summary>
        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(base.gameObject);
            }
            else
            {
                m_instance = this;
                DontDestroyOnLoad(this);
                
                m_UIMessageManager = UIMessageManager.GetInstance();
                m_UIMessageManager.SetMessageBoxes(m_complexMessageBox,m_simpleMessageBox,m_oneWayMessageBox);
                m_sceneHandler = SceneHandler.GetInstance();
                foreach (var sceneNDoc in m_UIDocumentsAndScenes)
                {
                    m_sceneHandler.AddSceneNDoc(sceneNDoc.m_id,sceneNDoc.m_scene,sceneNDoc.m_doc);
                }
                SceneHandler.GetInstance().SetCurrentScene(0);
                m_UIMessageManager.SetUIDocument(SceneHandler.GetInstance().CurrentDoc);
            
            }
            
        }
        
    }

    [Serializable]
    struct SceneNDoc
    {
        [FormerlySerializedAs("id")] [SerializeField] 
        public uint m_id;
        [SerializeField]
        public string m_scene;
        [SerializeField]
        public UIDocument m_doc;

    } 
}