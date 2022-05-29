using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCS : MonoBehaviour
{
    public static LinkedList<Step> steps = new();
    public static PriorityQueue open;
    public static Dictionary<TileBlock, TileBlock> parent = new();
    
    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        steps.Clear();
        open = new();
        parent.Clear();
        open.Enqueue(startBlock, 0, 0);
        parent[startBlock] = startBlock;
        while (open.Count > 0)
        {
            TileBlock curBlock = open.Dequeue();
            if (curBlock != startBlock)
            {
                steps.AddLast(new StepByCost(StepType.VISIT, curBlock, curBlock.Node.SearchCost.Value));
            }
            if (curBlock == endBlock)
            {
                return;
            }
            foreach (TileMap.Node node in curBlock.Node.Neighbours)
            {
                int costToNext = curBlock.Node.SearchCost.Value + node.Cost;
                if (!parent.ContainsKey(node.TileBlock))
                {
                    steps.AddLast(new StepByCost(StepType.SEE, node.TileBlock, costToNext));
                    parent[node.TileBlock] = curBlock;
                    open.Enqueue(node.TileBlock, costToNext, costToNext);
                }
                else if (node.SearchCost.Value > costToNext)
                {
                    parent[node.TileBlock] = curBlock;
                    if (open.Contains(node.TileBlock))
                    {
                        steps.AddLast(new StepByCost(StepType.SEE, node.TileBlock, costToNext));
                        node.SearchCost = new KeyValuePair<int, int>(costToNext, costToNext);
                    }
                    else
                    {
                        steps.AddLast(new StepByCost(StepType.SEEAGAIN, node.TileBlock, costToNext));
                        open.Enqueue(node.TileBlock, costToNext, costToNext);
                    }
                }
            }
        }
        parent.Clear();
    }
}
