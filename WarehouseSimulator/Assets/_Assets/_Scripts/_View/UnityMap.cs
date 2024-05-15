using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View
{
    public class UnityMap : MonoBehaviour
    {
        /// <summary>
        /// Prefab to a single map tile
        /// </summary>
        [SerializeField]
        private GameObject tilePrefab;

        /// <summary>
        /// Sprite for the wall
        /// </summary>
        [SerializeField] private Sprite wallSprite;
        /// <summary>
        /// Sprite for the empty tile
        /// </summary>
        [SerializeField] private Sprite emptySprite;
        
        /// <summary>
        /// The grid of the map. Used to calculate the position of the tiles
        /// </summary>
        private Grid gridComponent;

        /// <summary>
        /// The model part of the map
        /// </summary>
        private Map map;
        
        /// <summary>
        /// Gets the center of the map in world coordinates
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMapCenter()
        {
            if(map is null) return Vector2.zero;
            
            return gridComponent.GetCellCenterWorld(new Vector3Int(map.MapSize.x / 2, -(map.MapSize.y / 2), 0));
        }
        
        /// <summary>
        /// Gets the world position of a cell
        /// </summary>
        /// <param name="pos">The integer positions of the cell. (0, 0) is the top left, x increases to the right and y increases downwards</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(Vector2Int pos)
        {
            return gridComponent.GetCellCenterWorld(new Vector3Int(pos.x, -pos.y, 0));
        }
        
        /// <summary>
        /// Gets the size of the map
        /// </summary>
        /// <returns>The size of the map as a Vector2</returns>
        public Vector2 GetMapSize()
        {
            if(map is null) return Vector2.zero;
            
            return new Vector2(map.MapSize.x, map.MapSize.y);
        }
        
        void Start()
        {
            gridComponent = GetComponent<Grid>();
        }
        
        /// <summary>
        /// Assign the model part of the map
        /// </summary>
        /// <param name="map"></param>
        public void AssignMap(Map map)
        {
            this.map = map;
        }

        /// <summary>
        /// Generate the tiles of the map based on the model representation
        /// </summary>
        public void GenerateMap()
        {
            for (int i = 0; i < map.MapSize.x; i++)
            {
                for (int j = 0; j < map.MapSize.y; j++)
                {
                    GameObject ins = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                    ins.transform.position = gridComponent.GetCellCenterWorld(new Vector3Int(i, -j, 1));
                    if(map.GetTileAt(new  Vector2Int(i, j)) == TileType.Wall)
                    {
                        //ins.GetComponent<SpriteRenderer>().color = wallColor;
                        ins.GetComponent<SpriteRenderer>().sprite = wallSprite;
                    }
                    else
                    {
                        //ins.GetComponent<SpriteRenderer>().color = emptyColor;
                        ins.GetComponent<SpriteRenderer>().sprite = emptySprite;
                    }
                }
            }
        }
    }

}