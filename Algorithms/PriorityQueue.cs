using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private List<TileBlock> nodeList;
    public List<TileBlock> NodeList { get => nodeList; set => nodeList = value; }
    public int Count
    {
        get => nodeList.Count - 1;
    }
    public PriorityQueue()
    {
        nodeList = new();
        nodeList.Add(null);
    }
    private void SwapNode(int i, int j)
    {
        TileBlock tmp = nodeList[i];
        nodeList[i] = nodeList[j];
        nodeList[j] = tmp;
    }
    private int NodeValue(int index)
    {
        return nodeList[index].Node.SearchCost.Value;
    }
    private void MinHeap(int i)
    {
        int smallest = i;
        int left = 2 * i;
        int right = 2 * i + 1;

        if (left <= Count && NodeValue(left) < NodeValue(smallest))
            smallest = left;

        if (right <= Count && NodeValue(right) < NodeValue(smallest))
            smallest = right;

        if (smallest != i)
        {
            SwapNode(i, smallest);
            MinHeap(smallest);
        }
    }
    public bool Contains(TileBlock tileBlock)
    {
        return nodeList.Contains(tileBlock);
    }
    public void Enqueue(TileBlock tileBlock, int distance, int evaluation)
    {
        tileBlock.Node.SearchCost = new KeyValuePair<int, int>(distance, evaluation);
        nodeList.Add(tileBlock);
        int i = Count;
        while (i > 1 && NodeValue(i / 2) > NodeValue(i))
        {
            SwapNode(i, i / 2);
            i /= 2;
        }
    }
    public TileBlock Dequeue()
    {
        if (Count == 0)
        {
            Debug.Log("Priority Queue empty!");
            return nodeList[0];
        }
        TileBlock node = nodeList[1];
        SwapNode(1, Count);
        nodeList.RemoveAt(Count);
        MinHeap(1);
        return node;
    }
    public void Clear()
    {
        nodeList.Clear();
    }
}
