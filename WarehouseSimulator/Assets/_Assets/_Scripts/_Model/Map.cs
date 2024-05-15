using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Model representation of the map
    /// </summary>
    public class Map
    {
        /// <summary>
        /// The map's representation in a 2D array
        /// </summary>
        private TileType[,] mapRepresentaion;
        /// <summary>
        /// The maps size. X is the width, Y is the height
        /// </summary>
        private Vector2Int mapSize;

        /// <summary>
        /// See <see cref="MapSize"/> for details
        /// </summary>
        public Vector2Int MapSize => mapSize;
        
        /// <summary>
        /// Get the tile type at a specific position
        /// </summary>
        /// <param name="position">The position we are interested in</param>
        /// <returns>The tile's type at that position or Wall if the position is out of bounds</returns>
        public TileType GetTileAt(Vector2Int position)
        {
            if (position.x < 0 || position.x >= mapSize.x || position.y < 0 || position.y >= mapSize.y)
                return TileType.Wall; //implied value outsize the map
            return mapRepresentaion[position.y, position.x];
        }
        
        /// <summary>
        /// Get the tile type at a specific position.
        /// </summary>
        /// <param name="i">The linear position we are interested in. The linear position is treated as 'linear-position = y * width + x</param>
        /// <returns>The tile's type at that position or Wall if the position is out of bounds</returns>
        public TileType GetTileAt(int i)
        {
            if(i < 0 || i >= mapRepresentaion.Length)
                return TileType.Wall; //implied value outsize the map
            return mapRepresentaion[i / mapSize.x, i % mapSize.x];
        }

        /// <summary>
        /// Load the map from a file
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        public void LoadMap(string filePath)
        {
            using StreamReader reader = new(filePath);
            reader.ReadLine(); // "type octile". Not needed, throw away
            string all = reader.ReadToEnd();
            string[] lines = all.Replace("\r","").Split('\n');
            
            CreateMap(lines);
        }

        /// <summary>
        /// Create the map from the input
        /// </summary>
        /// <param name="input">The description of the map as an array of strings.</param>
        /// <exception cref="InvalidFileException">Thrown if the format of the file is incorrect</exception>
        /// <exception cref="InvalidOperationException">Thrown if the maps width or height are not integers</exception>
        /// <remarks> Example elements of <paramref name="input"/>: ["height h", "width w", "map", "row 1", "row 2", ... , "row h"] </remarks>
        public void CreateMap(string[] input)
        {
            if (mapSize == Vector2Int.zero)
            {
                if (input.Length < 3)
                {
                    throw new InvalidFileException("Invalid map file format: There weren't enough lines in the file!");
                }
                
                try
                {
                    mapSize.y = int.Parse(input[0].Split(' ')[1] ?? throw new InvalidOperationException(""));
                    mapSize.x = int.Parse(input[1].Split(' ')[1] ?? throw new InvalidOperationException(""));
                    if (mapSize.y <= 0 || mapSize.x <= 0)
                    {
                        throw new InvalidFileException("Invalid map file format: The size of the map cannot be zero or less.");
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
                        throw new InvalidFileException("The content of the map file wasn't in the right format.\nExpected more lines.");
                    }
                    string line = input[i+3];
                    if (line.Length != mapSize.x)

                        throw new InvalidFileException($"The content of the map file wasn't in the right format.\nExpected {mapSize.x} characters, got {line.Length} with content:\n{line}");
                
                    for (int j = 0; j < mapSize.x; j++)
                    {
                        mapRepresentaion[i, j] = (line[j] == '.' ? TileType.Empty : TileType.Wall);
                    }
                }
            }
        }
        
        

        /// <summary>
        /// Mark an empty tile as occupied by a robot
        /// </summary>
        /// <param name="dis">The position of the tile to be occupied</param>
        public void OccupyTile(Vector2Int dis)
        {
            if(GetTileAt(dis) == TileType.Empty)
            {
                mapRepresentaion[dis.y,dis.x] = TileType.RoboOccupied;
            }
        }
        /// <summary>
        /// Mark an occupied tile as empty
        /// </summary>
        /// <param name="dis">The position of the tile to be de-occupied</param>
        public void DeoccupyTile(Vector2Int dis)
        {
            if (GetTileAt(dis) == TileType.RoboOccupied)
            {
                mapRepresentaion[dis.y,dis.x] = TileType.Empty;
            }
        }
    }

}