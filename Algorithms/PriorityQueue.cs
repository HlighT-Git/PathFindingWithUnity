using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue <T> where T : IComparable<T>
{
    private List<T> nodes;
    public int Count
    {
        get => nodes.Count - 1;
    }
    public PriorityQueue()
    {
        nodes = new();
        nodes.Add(default);
    }
    private void SwapNode(int i, int j)
    {
        T tmp = nodes[i];
        nodes[i] = nodes[j];
        nodes[j] = tmp;
    }
    private void MinHeap(int i)
    {
        int smallest = i;
        int left = 2 * i;
        int right = 2 * i + 1;

        if (left <= Count && nodes[left].CompareTo(nodes[smallest]) < 0)
            smallest = left;

        if (right <= Count && nodes[right].CompareTo(nodes[smallest]) < 0)
            smallest = right;

        if (smallest != i)
        {
            SwapNode(i, smallest);
            MinHeap(smallest);
        }
    }
    public bool Contains(T node)
    {
        return nodes.Contains(node);
    }
    public void Enqueue(T node)
    {
        nodes.Add(node);
        int i = Count;
        while (i > 1 && nodes[i / 2].CompareTo(nodes[i]) > 0)
        {
            SwapNode(i, i / 2);
            i /= 2;
        }
    }
    public T Dequeue()
    {
        if (Count == 0)
        {
            Debug.Log("Priority Queue empty!");
            return default;
        }
        T node = nodes[1];
        SwapNode(1, Count);
        nodes.RemoveAt(Count);
        MinHeap(1);
        return node;
    }
    public void Clear()
    {
        nodes.Clear();
    }

    public void Remove(T node)
    {
        if (Contains(node))
            nodes.Remove(node);
    }
}