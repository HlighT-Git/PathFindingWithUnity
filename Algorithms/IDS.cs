using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDS : MonoBehaviour
{
    public static int depthLim = 1;
    public static int curdepthLim;
    public static LinkedList<Step> steps = new();
    public static Stack<KeyValuePair<TileBlock, int>> open = new();
    public static Dictionary<TileBlock, TileBlock> parent = new();

    public static void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        curdepthLim = 0;
        int limit = TileMap.MoveableBlockAmount();
        while (parent.Count < limit)
        {
            curdepthLim += depthLim;
            steps.Clear();
            open.Clear();
            parent.Clear();

            open.Push(new KeyValuePair<TileBlock, int>(startBlock, 0));
            parent[startBlock] = startBlock;
            while (open.Count > 0)
            {
                KeyValuePair<TileBlock, int> curEntry = open.Pop();
                if (curEntry.Key != startBlock)
                {
                    steps.AddLast(new Step(StepType.VISIT, curEntry.Key));
                }
                if (curEntry.Key == endBlock)
                {
                    return;
                }
                foreach (TileMap.Node node in curEntry.Key.Node.Neighbours)
                {
                    if (curEntry.Value <= depthLim && !parent.ContainsKey(node.TileBlock))
                    {
                        parent[node.TileBlock] = curEntry.Key;
                        steps.AddLast(new Step(StepType.SEE, node.TileBlock));
                        open.Push(new KeyValuePair<TileBlock, int>(node.TileBlock, curEntry.Value + 1));
                    }
                }
            }

        }
    }
}
