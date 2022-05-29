using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    public static LinkedList<Step> steps = new();
    public static Stack<TileBlock> open = new();
    public static Dictionary<TileBlock, TileBlock> parent = new();

    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        steps.Clear();
        open.Clear();
        parent.Clear();

        open.Push(startBlock);
        parent[startBlock] = startBlock;
        while (open.Count > 0)
        {
            TileBlock curBlock = open.Pop();
            if (curBlock != startBlock)
            {
                steps.AddLast(new Step(StepType.VISIT, curBlock));
            }
            if (curBlock == endBlock)
            {
                return;
            }
            foreach (TileMap.Node node in curBlock.Node.Neighbours)
            {
                if (!parent.ContainsKey(node.TileBlock))
                {
                    parent[node.TileBlock] = curBlock;
                    steps.AddLast(new Step(StepType.SEE, node.TileBlock));
                    open.Push(node.TileBlock);
                }
            }
        }
        parent.Clear();
    }
}
