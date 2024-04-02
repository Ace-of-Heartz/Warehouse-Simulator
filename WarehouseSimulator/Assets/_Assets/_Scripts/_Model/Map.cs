using System;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    public class Map
    {
        private TileType[,] mapRepresentaion;
        private Vector2Int mapSize;

        public Vector2Int MapSize => mapSize;
        
        public TileType GetTileAt(Vector2Int position)
        {
            if (position.x < 0 || position.x >= mapSize.x || position.y < 0 || position.y >= mapSize.y)
                return TileType.Wall; //implied value outsize the map
            return mapRepresentaion[position.y, position.x];
        }
        
        public TileType GetTileAt(int i)
        {
            if(i < 0 || i >= mapRepresentaion.Length)
                return TileType.Wall; //implied value outsize the map
            return mapRepresentaion[i / mapSize.x, i % mapSize.x];
        }

        public void LoadMap(string filePath)
        {
            using StreamReader reader = new(filePath);
            reader.ReadLine(); // "type octile". Not needed, throw away

            mapSize.y = int.Parse(reader.ReadLine()?.Split(' ')[1] ?? throw new InvalidOperationException());
            mapSize.x = int.Parse(reader.ReadLine()?.Split(' ')[1] ?? throw new InvalidOperationException());

            reader.ReadLine(); //"map". Not needed, throw away
            mapRepresentaion = new TileType[mapSize.y, mapSize.x];
            for (int i = 0; i < mapSize.y; i++)
            {
                string line = reader.ReadLine();
                if (line?.Length != mapSize.x)
                    throw new InvalidDataException("The content of the file wasn't in the right format.");

                for (int j = 0; j < mapSize.x; j++)
                {
                    mapRepresentaion[i, j] = (line[j] == '.' ? TileType.Empty : TileType.Wall);
                }
            }

        }

        public void OccupyTile(Vector2Int dis)
        {
            if(GetTileAt(dis) == TileType.Empty)
            {
                mapRepresentaion[dis.y,dis.x] = TileType.RoboOccupied;
            }
        }
        public void DeoccupyTile(Vector2Int dis)
        {
            if (GetTileAt(dis) == TileType.RoboOccupied)
            {
                mapRepresentaion[dis.y,dis.x] = TileType.Empty;
            }
        }
    }

}