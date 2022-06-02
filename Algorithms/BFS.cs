using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    public static LinkedList<Step> steps = new();
    public static Queue<TileBlock> open = new();
    public static Dictionary<TileBlock, TileBlock> parent = new();

    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.isIterativeDepending = false;
        steps.Clear();
        open.Clear();
        parent.Clear();

        Vector2 startPos = startBlock.TileNode.Index();
        int startX = Mathf.RoundToInt(startPos.x);
        int startY = Mathf.RoundToInt(startPos.y);
        steps.AddLast(new Step($"*** Bắt đầu tại ({startX}, {startY}) ***"));
        open.Enqueue(startBlock);
        parent[startBlock] = startBlock;
        while (open.Count > 0)
        {
            TileBlock curBlock = open.Dequeue();
            if (curBlock != startBlock)
            {
                steps.AddLast(new Step(curBlock, TileStatus.SEEN, TileStatus.VISITED));
            }
            if (curBlock == endBlock)
            {
                return;
            }
            foreach (TileMap.TileNode node in curBlock.TileNode.Neighbours)
            {
                if (!parent.ContainsKey(node.TileBlock))
                {
                    parent[node.TileBlock] = curBlock;
                    steps.AddLast(new Step(node.TileBlock, TileStatus.NORMAL, TileStatus.SEEN));
                    open.Enqueue(node.TileBlock);
                }
            }
        }
        parent.Clear();
    }
}
