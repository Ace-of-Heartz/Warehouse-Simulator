using System;
using System.Collections.Generic;
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
            string all = reader.ReadToEnd();
            string[] lines = all.Replace("\r","").Split('\n');
            
            CreateMap(lines);
        }

        public void CreateMap(string[] input)
        {
            if (mapSize == Vector2Int.zero)
            {
                if (input.Length < 3)
                {
                    throw new InvalidFileException("Invalid file format: There weren't enough lines in the file!");
                }
                
                try
                {
                    mapSize.y = int.Parse(input[0].Split(' ')[1] ?? throw new InvalidOperationException(""));
                    mapSize.x = int.Parse(input[1].Split(' ')[1] ?? throw new InvalidOperationException(""));
                    if (mapSize.y <= 0 || mapSize.x <= 0)
                    {
                        throw new InvalidFileException("Invalid file format: The size of the map cannot be zero or less.");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidFileException($"The map file wasn't in the correct format, the height or width of the map was invalid, exact exception message: {ex.Message}");
                }
                
                //"map". Not needed (input[2])
                
                mapRepresentaion = new TileType[mapSize.y, mapSize.x];
                for (int i = 0; i < mapSize.y; ++i)
                {
                    if (i + 3 >= input.Length)
                    {
                        throw new InvalidFileException("The content of the file wasn't in the right format.\nExpected more lines.");
                    }
                    string line = input[i+3];
                    if (line.Length != mapSize.x)

                        throw new InvalidFileException($"The content of the file wasn't in the right format.\nExpected {mapSize.x} characters, got {line.Length} with content:\n{line}");
                
                    for (int j = 0; j < mapSize.x; j++)
                    {
                        mapRepresentaion[i, j] = (line[j] == '.' ? TileType.Empty : TileType.Wall);
                    }
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