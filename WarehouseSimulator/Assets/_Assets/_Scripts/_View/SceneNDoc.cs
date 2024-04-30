using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    /// <summary>
    /// Struct for wrapping data for scene and UIDocument
    /// </summary>
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