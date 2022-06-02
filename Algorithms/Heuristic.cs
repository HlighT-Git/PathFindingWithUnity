using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic : MonoBehaviour
{
    public static int type = 0;
    private static int ManhattanHeuristic(TileBlock startBlock, TileBlock endBlock)
    {
        Vector2 cost = startBlock.TileNode.Position - endBlock.TileNode.Position;
        return Mathf.RoundToInt(Mathf.Abs(cost.x) + Mathf.Abs(cost.y));
    }
    private static int FloydHeuristic(TileBlock startBlock, TileBlock endBlock)
    {
        Vector2 index = endBlock.TileNode.Index();
        return startBlock.TileNode.CostTo[Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y)];
    }
    private static int EuclideanHeuristic(TileBlock startBlock, TileBlock endBlock)
    {
        Vector2 cost = endBlock.TileNode.Index() - startBlock.TileNode.Index();
        return Mathf.RoundToInt(cost.magnitude);
    }
    public static int Between(TileBlock startBlock, TileBlock endBlock)
    {
        if (type == 0)
        {
            return ManhattanHeuristic(startBlock, endBlock);
        }
        else if (type == 1)
        {
            return EuclideanHeuristic(startBlock, endBlock);
        }
        return FloydHeuristic(startBlock, endBlock);
    }
}
