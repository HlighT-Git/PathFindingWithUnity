using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic : MonoBehaviour
{
    private static int ManhattanHeristic(TileBlock startBlock, TileBlock endBlock)
    {
        Vector2 cost = startBlock.Node.Position - endBlock.Node.Position;
        return Mathf.RoundToInt(Mathf.Abs(cost.x) + Mathf.Abs(cost.y));
    }
    private static int FloydHeuristic(TileBlock startBlock, TileBlock endBlock)
    {
        Vector2 index = endBlock.Node.Index();
        return startBlock.Node.CostTo[Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y)];
    }
    public static int Between(TileBlock startBlock, TileBlock endBlock)
    {
        if (TileMap.isWeightGraphMap)
        {
            return FloydHeuristic(startBlock, endBlock);
        }
        return ManhattanHeristic(startBlock, endBlock);
    }
}
