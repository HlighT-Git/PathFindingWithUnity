using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDAStar : MonoBehaviour
{
    public static int betaVal = 1;
    public static LinkedList<Step> steps = new();
    public static Stack<SearchEntry> open;
    public static Dictionary<TileBlock, TileBlock> parent = new();
    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.isIterativeDepending = true;
        steps.Clear();
        open = new();
        int costLim = 0;
        bool seenAll = false;
        while (!seenAll)
        {
            seenAll = true;
            costLim += betaVal;
            parent.Clear();

            Vector2 startPos = startBlock.TileNode.Index();
            int startX = Mathf.RoundToInt(startPos.x);
            int startY = Mathf.RoundToInt(startPos.y);
            steps.AddLast(new Step($"*** Bắt đầu tại ({startX}, {startY}) (H_cost < {costLim + 1}) ***"));
            open.Push(new(startBlock, 0, Heuristic.Between(startBlock, endBlock)));
            parent[startBlock] = startBlock;
            List<TileBlock> curPath = new();
            while (open.Count > 0)
            {
                SearchEntry curNode = open.Pop();
                if (curNode.Block != startBlock)
                {
                    steps.AddLast(new InformStep(curNode.Block, TileStatus.SEEN, TileStatus.VISITED, curNode.GainCost, curNode.Value));
                    int index = curPath.IndexOf(curNode.Block);
                    if (index == -1)
                    {
                        curPath.Add(curNode.Block);
                    }
                    else
                    {
                        curPath = curPath.GetRange(0, index + 1);
                    }
                }
                if (curNode.Block == endBlock)
                {
                    return;
                }
                Stack<SearchEntry> order = new();
                foreach (TileMap.TileNode tileNode in curNode.Block.TileNode.Neighbours)
                {
                    if (curPath.Contains(tileNode.TileBlock))
                        continue;
                    int gainCost = curNode.GainCost + tileNode.Cost;
                    int value = gainCost + Heuristic.Between(tileNode.TileBlock, endBlock);
                    if (value > costLim)
                    {
                        seenAll = false;
                        continue;
                    }
                    parent[tileNode.TileBlock] = curNode.Block;
                    steps.AddLast(new InformStep(tileNode.TileBlock, TileStatus.NORMAL, TileStatus.SEEN, curNode.GainCost, curNode.Value));
                    order.Push(new(tileNode.TileBlock, gainCost, value));
                }
                foreach (SearchEntry node in order)
                {
                    open.Push(node);
                }
            }
        }
        parent.Clear();
    }
}
