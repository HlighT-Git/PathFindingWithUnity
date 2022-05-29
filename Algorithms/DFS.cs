using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    public static LinkedList<Step> steps;
    public static Stack<TileBlock> open;
    public static LinkedList<TileBlock> path;
    public static List<TileBlock> seen;

    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        steps = new();
        open = new();
        path = new();
        seen = new();

        open.Push(startBlock);
        while (open.Count > 0)
        {
            TileBlock curBlock = open.Pop();
            path.AddLast(curBlock);
            if (curBlock != startBlock)
            {
                steps.AddLast(new Step(StepType.VISIT, path.Last.Value, curBlock));
            }
            if (curBlock == endBlock)
            {
                return;
            }
            bool canMove = false;
            foreach (TileMap.Node node in curBlock.Node.Neighbours)
            {
                if (!seen.Contains(node.TileBlock))
                {
                    canMove = true;
                    seen.Add(node.TileBlock);
                    Step nextStep = new(StepType.SEE, curBlock, node.TileBlock);
                    steps.AddLast(nextStep);
                    open.Push(node.TileBlock);
                }
            }
            if (!canMove)
            {
                path.RemoveLast();
            }
        }
        open = null;
    }
}
