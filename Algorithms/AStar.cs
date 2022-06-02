using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public static LinkedList<Step> steps = new();
    public static PriorityQueue<SearchEntry> open;
    public static Dictionary<TileBlock, TileBlock> parent = new();
    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.isIterativeDepending = false;
        steps.Clear();
        open = new();
        parent.Clear();
        open.Enqueue(new(startBlock, 0, Heuristic.Between(startBlock, endBlock)));
        parent[startBlock] = startBlock;
        while (open.Count > 0)
        {
            SearchEntry curNode = open.Dequeue();
            if (curNode.Block != startBlock)
            {
                steps.AddLast(new InformStep(curNode.Block, TileStatus.SEEN, TileStatus.VISITED, curNode.GainCost, curNode.Value));
            }
            if (curNode.Block == endBlock)
            {
                return;
            }
            foreach (TileMap.TileNode tileNode in curNode.Block.TileNode.Neighbours)
            {
                int gainCost = curNode.GainCost + tileNode.Cost;
                int value = gainCost + Heuristic.Between(tileNode.TileBlock, endBlock);
                if (!parent.ContainsKey(tileNode.TileBlock))
                {
                    steps.AddLast(new InformStep(tileNode.TileBlock, TileStatus.NORMAL, TileStatus.SEEN, gainCost, value));
                    parent[tileNode.TileBlock] = curNode.Block;
                    open.Enqueue(new(tileNode.TileBlock, gainCost, value));
                }
                else if (tileNode.TileBlock.Node.Value > value)
                {
                    parent[tileNode.TileBlock] = curNode.Block;
                    open.Remove(tileNode.TileBlock.Node);
                    steps.AddLast(new InformStep(tileNode.TileBlock,
                        open.Contains(tileNode.TileBlock.Node) ? TileStatus.SEEN : TileStatus.VISITED,
                        TileStatus.SEEN, gainCost, value));
                    open.Enqueue(new(tileNode.TileBlock, gainCost, value));
                }
            }
        }
        parent.Clear();
    }
}
