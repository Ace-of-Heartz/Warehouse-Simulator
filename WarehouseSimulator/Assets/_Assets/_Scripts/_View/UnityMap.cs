using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View
{
    public class UnityMap : MonoBehaviour
    {
        [SerializeField]
        private GameObject tilePrefab;

        [SerializeField] private Color wallColor = Color.grey;
        [SerializeField] private Color emptyColor = Color.green;

        private Grid gridComponent;

        private Map map;
        
        
        public Vector2 GetMapCentrer()
        {
            if(map is null) return Vector2.zero;
            
            return gridComponent.GetCellCenterWorld(new Vector3Int(map.MapSize.x / 2, -(map.MapSize.y / 2), 0));
        }
        
        public Vector3 GetWorldPosition(Vector2Int pos)
        {
            return gridComponent.GetCellCenterWorld(new Vector3Int(pos.x, -pos.y, 0));
        }
        
        public Vector2 GetMapSize()
        {
            if(map is null) return Vector2.zero;
            
            return new Vector2(map.MapSize.x, map.MapSize.y);
        }
        
        void Start()
        {
            gridComponent = GetComponent<Grid>();
        }
        
        public void AssignMap(Map map)
        {
            this.map = map;
        }

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
                        ins.GetComponent<SpriteRenderer>().color = wallColor;
                    }
                    else
                    {
                        ins.GetComponent<SpriteRenderer>().color = emptyColor;
                    }
                }
            }
        }
    }

}