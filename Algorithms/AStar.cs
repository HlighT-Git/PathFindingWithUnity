using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public static LinkedList<Step> steps = new();
    public static PriorityQueue open;
    public static Dictionary<TileBlock, TileBlock> parent = new();
    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        steps.Clear();
        open = new();
        parent.Clear();
        open.Enqueue(startBlock, 0, Heuristic.Between(startBlock, endBlock));
        parent[startBlock] = startBlock;
        while (open.Count > 0)
        {
            TileBlock curBlock = open.Dequeue();
            if (curBlock != startBlock)
            {
                steps.AddLast(new InformStep(StepType.VISIT, curBlock, curBlock.Node.SearchCost.Key, curBlock.Node.SearchCost.Value));
            }
            if (curBlock == endBlock)
            {
                return;
            }
            foreach (TileMap.Node node in curBlock.Node.Neighbours)
            {
                int tmpCost = curBlock.Node.SearchCost.Key + node.Cost;
                int evaluation = tmpCost + Heuristic.Between(node.TileBlock, endBlock);
                if (!parent.ContainsKey(node.TileBlock))
                {
                    steps.AddLast(new InformStep(StepType.SEE, node.TileBlock, tmpCost, evaluation));
                    parent[node.TileBlock] = curBlock;
                    open.Enqueue(node.TileBlock, tmpCost, evaluation);
                }
                else if (node.SearchCost.Value > evaluation)
                {
                    parent[node.TileBlock] = curBlock;
                    if (open.Contains(node.TileBlock))
                    {
                        steps.AddLast(new InformStep(StepType.SEE, node.TileBlock, tmpCost, evaluation));
                        node.SearchCost = new KeyValuePair<int, int>(tmpCost, evaluation);
                    }
                    else
                    {
                        steps.AddLast(new InformStep(StepType.SEEAGAIN, node.TileBlock, tmpCost, evaluation));
                        open.Enqueue(node.TileBlock, tmpCost, evaluation);
                    }
                }
            }
        }
        parent.Clear();
    }
}
