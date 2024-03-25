using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map
{
    private List<TileType> mapRepresentaion;
    private Vector2Int mapSize;
    
    public Vector2Int MapSize => mapSize;
    
    public Map()
    {
        mapRepresentaion = new List<TileType>();
    }
    
    public void LoadMap(string filePath)
    {
        using StreamReader reader = new(filePath);
        reader.ReadLine(); // "type octile". Not needed, throw away

        mapSize.y = int.Parse(reader.ReadLine()?.Split(' ')[1] ?? throw new InvalidOperationException());
        mapSize.x = int.Parse(reader.ReadLine()?.Split(' ')[1] ?? throw new InvalidOperationException());
        
        reader.ReadLine(); //"map". Not needed, throw away
        for (int i = 0; i < mapSize.y; i++)
        {
            string line = reader.ReadLine();
            if(line?.Length != mapSize.x)
                throw new InvalidDataException("The content of the file wasn't in the right format.");
            
            for (int j = 0; j < mapSize.x; j++)
            {
                mapRepresentaion.Add(line[j] == '.' ? TileType.Empty : TileType.Wall);
            }
        }
        
    }
}
