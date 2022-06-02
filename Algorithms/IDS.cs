using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDS : MonoBehaviour
{
    public static LinkedList<Step> steps = new();
    public static Dictionary<TileBlock, TileBlock> parent = new();
    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.isIterativeDepending = true;
        steps.Clear();
        int curDepthLim = 0;
        int depthLimit = 7;
        LinkedList<TileBlock> path = new();
        bool FoundPathWithLimDepth(TileBlock curBlock)
        {
            if (curBlock != startBlock)
            {
                steps.AddLast(new Step(curBlock, TileStatus.NORMAL, TileStatus.VISITED));
            }
            if (curBlock == endBlock)
            {
                return true;
            }
            if (path.Count >= curDepthLim)
            {
                return false;
            }
            foreach (TileMap.TileNode node in curBlock.TileNode.Neighbours)
            {
                if (path.Contains(node.TileBlock))
                {
                    continue;
                }
                path.AddLast(node.TileBlock);
                if (FoundPathWithLimDepth(node.TileBlock))
                {
                    return true;
                }
                path.RemoveLast();
            }
            return false;
        }
        while(curDepthLim < depthLimit) {
            curDepthLim += 1;
            parent.Clear();

            Vector2 startPos = startBlock.TileNode.Index();
            int startX = Mathf.RoundToInt(startPos.x);
            int startY = Mathf.RoundToInt(startPos.y);
            steps.AddLast(new Step($"*** Bắt đầu tại ({startX}, {startY}) (depth < {curDepthLim + 1}) ***"));
            parent[startBlock] = startBlock;
            if (FoundPathWithLimDepth(startBlock))
            {
                LinkedListNode<TileBlock> entry = path.First;
                parent[entry.Value] = startBlock;
                while (entry.Next != null)
                {
                    parent[entry.Next.Value] = entry.Value;
                    entry = entry.Next;
                }
                return;
            }
        }
        parent.Clear();
    }
}