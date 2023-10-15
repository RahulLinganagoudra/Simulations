using System;
using System.Collections.Generic;
using UnityEngine;

public class SpatialHash
{
    public static uint res;
    public static uint halfRes;
    public uint cellSize;
    Dictionary<uint, List<int>> spatialHashTable;
    public SpatialHash()
    {
        res = (uint)Math.Sqrt(Math.Pow(2, 32));
        halfRes = res >> 2;
        cellSize = 2;
        spatialHashTable = new Dictionary<uint, List<int>>();
    }
    public uint GetSpatialHashIndex(float x, float y, uint cellSize = 1)
    {
        int gridX = Mathf.FloorToInt(x / (int)cellSize);
        int gridY = Mathf.FloorToInt(y / (int)cellSize);
        
        uint converted_X_Index = (uint)(halfRes + gridX);
        uint converted_Y_Index = (uint)(halfRes + gridY);

        return (res * converted_X_Index) + converted_Y_Index;
    }
    public void Insert(Vector2 pos, int index)
    {
        uint hash = GetSpatialHashIndex(pos.x, pos.y, cellSize);
        if (!spatialHashTable.ContainsKey(hash))
        {
            spatialHashTable[hash] = new List<int>
            {
                index
            };
        }
        spatialHashTable[hash].Add(index);
    }
    public int[] GetCellElements(Vector2 pos,uint cellSize=1)
    {
        int[] cells = new int[0];
        uint key = GetSpatialHashIndex(pos.x, pos.y, cellSize);
        if (spatialHashTable.ContainsKey(key))
        {
            cells = spatialHashTable[key].ToArray();
        }

        return cells;
    }

}

