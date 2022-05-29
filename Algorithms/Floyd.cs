using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floyd
{
    private static bool IsNextToEachOther(TileMap.Node node1, TileMap.Node node2)
    {
        int subX = (int)(node1.Position.x - node2.Position.x);
        int subY = (int)(node1.Position.y - node2.Position.y);
        if (Mathf.Abs(subX) + Mathf.Abs(subY) == 1)
            return true;
        return false;
    }
    private static void InitDistance()
    {
        for (int i = 0; i < TileMap.mapSizeX; i++)
        {
            for (int j = 0; j < TileMap.mapSizeY; j++)
            {
                if (TileMap.Graph[i, j].Cost == 0)
                {
                    continue;
                }
                for (int k = 0; k < TileMap.mapSizeX; k++)
                {
                    for (int l = 0; l < TileMap.mapSizeY; l++)
                    {
                        if (TileMap.Graph[k, l].Cost == 0)
                        {
                            TileMap.Graph[i, j].CostTo[k, l] = 100;
                        }
                        else if (i == k && j == l)
                            TileMap.Graph[i, j].CostTo[k, l] = 0;
                        else if (IsNextToEachOther(TileMap.Graph[i, j], TileMap.Graph[k, l]))
                        {
                            TileMap.Graph[i, j].CostTo[k, l] = TileMap.Graph[k, l].Cost;
                        }
                        else
                            TileMap.Graph[i, j].CostTo[k, l] = 999;
                    }
                }
            }
        }
    }
    public static void CalculateAllPairCost()
    {
        InitDistance();
        for (int m = 0; m < TileMap.mapSizeX; m++)
        {
            for (int n = 0; n < TileMap.mapSizeY; n++)
            {
                if (TileMap.Graph[m, n].Cost == 0)
                {
                    continue;
                }
                for (int i = 0; i < TileMap.mapSizeX; i++)
                {
                    for (int j = 0; j < TileMap.mapSizeY; j++)
                    {
                        if (TileMap.Graph[i, j].Cost == 0
                            || (i == m && j == n))
                        {
                            continue;
                        }
                        for (int k = 0; k < TileMap.mapSizeX; k++)
                        {
                            for (int l = 0; l < TileMap.mapSizeY; l++)
                            {
                                if (TileMap.Graph[k, l].Cost == 0
                                    || (k == m && l == n)
                                    || (k == i && l == j))
                                {
                                    continue;
                                }
                                TileMap.Graph[i, j].CostTo[k, l] = Mathf.Min(TileMap.Graph[i, j].CostTo[k, l],
                                    TileMap.Graph[i, j].CostTo[m, n] + TileMap.Graph[m, n].CostTo[k, l]);
                            }
                        }
                    }
                }
            }
        }
    }
}
