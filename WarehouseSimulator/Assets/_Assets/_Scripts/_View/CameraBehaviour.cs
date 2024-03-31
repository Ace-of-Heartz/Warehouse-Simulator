using System;
using UnityEngine;

namespace WarehouseSimulator.View
{
    [RequireComponent(typeof(Camera))]
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private UnityMap unityMap;
        
        [SerializeField] private float cameraSpeed = 5.0f;
        [SerializeField] private float zoomSpeed = 1.0f;

        void Update()
        {
            if (Input.GetKey(KeyCode.W))
                MoveRequested(Vector2.up);
            if (Input.GetKey(KeyCode.S))
                MoveRequested(Vector2.down);
            if (Input.GetKey(KeyCode.A))
                MoveRequested(Vector2.left);
            if (Input.GetKey(KeyCode.D))
                MoveRequested(Vector2.right);
            if (Input.GetKeyDown(KeyCode.Space))
                ResetCamera();
            
            
        }
        
        private void MoveRequested(Vector2 direction)
        {
            transform.position += new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * cameraSpeed);
        }
        
        private void ResetCamera()
        {
            Vector2 mapCenter = unityMap.GetMapCentrer();
            Vector2 mapSize = unityMap.GetMapSize();
            transform.position = new Vector3(mapCenter.x, mapCenter.y, -10);

            Camera cam = GetComponent<Camera>();
            cam.orthographicSize = Math.Max(mapSize.y, mapSize.x / cam.aspect) / 2 * 1.5f;
        }
    }
   
}