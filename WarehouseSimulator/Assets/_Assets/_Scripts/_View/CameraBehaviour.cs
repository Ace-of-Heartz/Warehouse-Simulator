using System;
using UnityEngine;

namespace WarehouseSimulator.View
{
    [RequireComponent(typeof(Camera))]
    public class CameraBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Reference to the map
        /// </summary>
        [SerializeField] private UnityMap unityMap;
        
        /// <summary>
        /// The camera's movement speed
        /// </summary>
        [SerializeField] private float cameraSpeed = 5.0f;
        /// <summary>
        /// The camera's zoom speed
        /// </summary>
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
            
            ZoomRequested(Input.mouseScrollDelta.y);
        }
        
        /// <summary>
        /// Modify the camera's zoom
        /// </summary>
        /// <param name="zoom">the delta zoom</param>
        private void ZoomRequested(float zoom)
        {
            Camera cam = GetComponent<Camera>();
            cam.orthographicSize = Math.Max(1, cam.orthographicSize - zoom * zoomSpeed);
        }
        
        /// <summary>
        /// Modify the camera's position
        /// </summary>
        /// <param name="direction">the delta poisiton</param>
        private void MoveRequested(Vector2 direction)
        {
            transform.position += new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * cameraSpeed);
        }
        
        /// <summary>
        /// Reset the camera's position and zoom to the default
        /// </summary>
        private void ResetCamera()
        {
            Vector2 mapCenter = unityMap.GetMapCenter();
            Vector2 mapSize = unityMap.GetMapSize();
            transform.position = new Vector3(mapCenter.x, mapCenter.y, -10);

            Camera cam = GetComponent<Camera>();
            cam.orthographicSize = Math.Max(mapSize.y, mapSize.x / cam.aspect) / 2 * 1.5f;
            
        }
    }
   
}