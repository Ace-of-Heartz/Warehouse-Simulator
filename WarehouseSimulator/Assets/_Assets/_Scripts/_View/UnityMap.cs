using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enum;

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
                    GameObject ins = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, this.gameObject.transform);
                    ins.transform.position = gridComponent.GetCellCenterWorld(new Vector3Int(i, j));
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