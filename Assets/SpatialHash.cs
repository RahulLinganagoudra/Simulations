using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Visualization;
//      resolutionX * resolutionY = total no of cells ( uint can hold 4,29,49,67,296 elements)                       Cell size
//      resolutionX = resolutionY = root of Max of uint ( 65,536 cells in one axis)                                _____________
//      half resolution i.e  32,768 cells in -X axis                                                               |           |
//      By pushing world coord values hres times down we can achieve cell hash to go in               cell size -> | grid cell |
//      +Quadrant which is helpfull for indexing                                                                   |           |
//                                                                                                                   
//
public class SpatialHash
{

    public static uint res;
    public static uint halfRes;
    public uint cellSize;

    Dictionary<uint, List<int>> spatialHashTable;



    #region constructor
    private SpatialHash()
    {
        res = (uint)Math.Sqrt(uint.MaxValue);
        halfRes = res >> 1;
        cellSize = 2;
        spatialHashTable = new Dictionary<uint, List<int>>();
    }

    //preventing me from skipping cellsize
    public SpatialHash(uint cellSize) : this()
    {
        this.cellSize = cellSize;
    }
    #endregion

    /// <summary>
    /// Creates a unique hash number for the grid cell
    /// </summary>
    /// <param name="x">X coordinate of the grid( * grid coords )</param>
    /// <param name="y">Y coordinate of the grid( * grid coords )</param>
    /// <param name="cellSize">grid cell size</param>
    /// <returns>Unique grid cell hash</returns>
    public uint GetSpatialHashIndex(Vector2 pos)
    {
        GetGridXY(pos, out int gridX, out int gridY);
        return GetSpatialHashIndex(gridX, gridY);
    }
    /// <summary>
    /// Creates a unique hash number for the grid cell
    /// </summary>
    /// <param name="x">X coordinate of the grid( * grid coords )</param>
    /// <param name="y">Y coordinate of the grid( * grid coords )</param>
    /// <param name="cellSize">grid cell size</param>
    /// <returns>Unique grid cell hash</returns>
    public uint GetSpatialHashIndex(int gridX, int gridY)
    {
        uint converted_X_Index = (uint)(halfRes + gridX);
        uint converted_Y_Index = (uint)(halfRes + gridY);

        return (res * converted_X_Index) + converted_Y_Index;
    }


    /// <summary>
    /// Inserts the elements to the dictionary by finding the grid cell hashIndex. 
    /// Cell size should be predefined otherwise dictionary might need to be refilled again
    /// </summary>
    /// <param name="pos">determines the hash ( * should be in world Coords)</param>
    /// <param name="index">index of the point in the main array</param>
    /// 
    public void Insert(Vector2 pos, int index)
    {
        uint hash = GetSpatialHashIndex(pos);


        if (!spatialHashTable.ContainsKey(hash))
        {
            spatialHashTable[hash] = new List<int>
            {
                index
            };
        }
        spatialHashTable[hash].Add(index);
    }

    /// <summary>
    /// this function is used to retrieve the elements in the same grid as the specified position
    /// </summary>
    /// <param name="pos">position specifies which grid cell to look for</param>
    /// <param name="cellSize"></param>
    /// <returns>the elements inside the specified grid cell</returns>
    public int[] GetCellElements(uint hash)
    {
        int[] cells = new int[0];
        if (spatialHashTable.ContainsKey(hash))
        {
            cells = spatialHashTable[hash].ToArray();
        }

        return cells;
    }
    /// <summary>
    /// this function is used to retrieve the elements in the same grid as the specified position
    /// </summary>
    /// <param name="pos">position specifies which grid cell to look for</param>
    /// <param name="cellSize"></param>
    /// <returns>the elements inside the specified grid cell</returns>
    public int[] GetCellElements(int gridX, int gridY, float radius = 1)
    {
        int[] cells = new int[0];

        if (radius >= cellSize)
        {
            GetGridXY(Vector2.one * Mathf.CeilToInt(radius), out int x, out int y);

            for (int i = -x; i < x + 1; i++)
            {
                for (int j = -y; j < y + 1; j++)
                {
                    uint nieghbourHash = GetSpatialHashIndex(gridX + i, gridY + j);
                    if (spatialHashTable.ContainsKey(nieghbourHash))
                    {
                        cells = cells.Concat(GetCellElements(nieghbourHash)).ToArray();
                    }
                }
            }
        }
        else
        {
            uint hash = GetSpatialHashIndex(gridX, gridY);

            if (spatialHashTable.ContainsKey(hash))
            {
                cells = spatialHashTable[hash].ToArray();
            }
        }
        return cells;
    }
    public void GetGridXY(Vector2 pos, out int x, out int y)
    {
        x = Mathf.FloorToInt(pos.x / (int)cellSize);
        y = Mathf.FloorToInt(pos.y / (int)cellSize);
    }
    public void Trash()
    {
        foreach (var item in spatialHashTable.Keys)
        {
            spatialHashTable[item].Clear();
        }
        spatialHashTable.Clear();
    }
    public void DrawGrid()
    {
        Color originalColor = Gizmos.color;
        Gizmos.color = Color.grey;

        for (int i = -10 * (int)cellSize; i <= 10 * (int)cellSize; i += (int)cellSize)
        {
            int s = -10, e = 10;
            if (i % cellSize == 0)
            {
                Visualizer.DrawLine(new(i, s, 0), new(i, e, 0),1f);
                Visualizer.DrawLine(new(s, i, 0), new(e, i, 0),1f);
            }

        }
        Gizmos.color = originalColor;
    }

}

