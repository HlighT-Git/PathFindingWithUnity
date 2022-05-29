//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AStar : MonoBehaviour
//{
//    private static WaitForSeconds waitFor50ms = new(0.05f);
//    private static WaitForSeconds waitFor20ms = new(0.02f);
//    class PriorityQueue
//    {
//        private List<TileBlock> nodeList;
//        public List<TileBlock> NodeList { get => nodeList; set => nodeList = value; }

//        public PriorityQueue()
//        {
//            nodeList = new List<TileBlock>();
//            nodeList.Add(null);
//        }
//        private void SwapNode(int i, int j)
//        {
//            TileBlock tmp = nodeList[i];
//            nodeList[i] = nodeList[j];
//            nodeList[j] = tmp;
//        }
//        private int NodeValue(int index)
//        {
//            return nodeList[index].Node.SearchCost.Value;
//        }
//        private void MinHeap(int i)
//        {
//            int smallest = i;
//            int left = 2 * i;
//            int right = 2 * i + 1;

//            if (left <= Size() && NodeValue(left) < NodeValue(smallest))
//                smallest = left;

//            if (right <= Size() && NodeValue(right) < NodeValue(smallest))
//                smallest = right;

//            if (smallest != i)
//            {
//                SwapNode(i, smallest);
//                MinHeap(smallest);
//            }
//        }
//        public int Size()
//        {
//            return nodeList.Count - 1;
//        }
//        public bool Contains(TileBlock tileBlock)
//        {
//            return nodeList.Contains(tileBlock);
//        }
//        public void Push(TileBlock tileBlock, int distance, int evaluation)
//        {
//            tileBlock.Node.SearchCost = new KeyValuePair<int, int>(distance, evaluation);
//            nodeList.Add(tileBlock);
//            int i = Size();
//            while (i > 1 && NodeValue(i / 2) > NodeValue(i))
//            {
//                SwapNode(i, i / 2);
//                i /= 2;
//            }
//        }
//        public TileBlock Pop()
//        {
//            if (Size() == 0)
//            {
//                Debug.Log("Priority Queue empty!");
//                return nodeList[0];
//            }
//            TileBlock node = nodeList[1];
//            SwapNode(1, Size());
//            nodeList.RemoveAt(Size());
//            MinHeap(1);
//            return node;
//        }

//    }
//    private static int ManhattanHeristic(TileBlock startBlock, TileBlock endBlock)
//    {
//        Vector2 cost = startBlock.Node.Position - endBlock.Node.Position;
//        return Mathf.RoundToInt(Mathf.Abs(cost.x) + Mathf.Abs(cost.y));
//    }
//    private static int FloydHeuristic(TileBlock startBlock, TileBlock endBlock)
//    {
//        Vector2 index = endBlock.Node.Index();
//        return startBlock.Node.CostTo[Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y)];
//    }
//    private static int Heuristic(TileBlock startBlock, TileBlock endBlock)
//    {
//        if (TileMap.isWeightGraphMap)
//        {
//            return FloydHeuristic(startBlock, endBlock);
//        }
//        return ManhattanHeristic(startBlock, endBlock);
//    }
//    private static List<TileBlock> GetPath (Dictionary<TileBlock, TileBlock> parent, TileBlock startBlock, TileBlock endBlock)
//    {
//        List<TileBlock> path = new();
//        path.Add(endBlock);
//        TileBlock entry = endBlock;
//        while (entry != startBlock)
//        {
//            entry = parent[entry];
//            path.Add(entry);
//        }
//        path.Reverse();
//        return path;
//    }
//    public static IEnumerator FindPath(GameObject player, TileBlock startBlock, TileBlock endBlock)
//    {
//        PriorityQueue open = new();
//        List<TileBlock> close = new();
//        Dictionary<TileBlock, TileBlock> parent = new();
//        open.Push(startBlock, 0, Heuristic(startBlock, endBlock));
//        while (open.Size() > 0)
//        {
//            TileBlock tileBlock = open.Pop();
//            tileBlock.SetStatus(TileStatus.VISITED);
//            yield return waitFor50ms;
//            if (tileBlock == endBlock)
//            {
//                yield return player.GetComponent<CharacterActions>().Move(GetPath(parent, startBlock, endBlock));
//                break;
//            }
//            close.Add(tileBlock);
//            foreach (TileMap.Node neighbour in tileBlock.Node.Neighbours)
//            {
//                if ((parent.ContainsKey(tileBlock) && neighbour.TileBlock == parent[tileBlock])
//                    || neighbour.TileBlock == startBlock)
//                {
//                    continue;
//                }
//                int tmpCost = tileBlock.Node.SearchCost.Key + neighbour.Cost;
//                int evaluation = tmpCost + Heuristic(neighbour.TileBlock, endBlock);
//                if (open.Contains(neighbour.TileBlock))
//                {
//                    if (neighbour.SearchCost.Value > evaluation)
//                    {
//                        parent[neighbour.TileBlock] = tileBlock;
//                        neighbour.SearchCost = new KeyValuePair<int, int>(tmpCost, evaluation);
//                    }
//                }
//                else if (close.Contains(neighbour.TileBlock))
//                {
//                    if (neighbour.SearchCost.Value > evaluation)
//                    {
//                        open.Push(neighbour.TileBlock, tmpCost, evaluation);
//                        neighbour.TileBlock.SetStatus(TileStatus.INQUEUE);
//                        yield return waitFor50ms;
//                    }
//                }
//                else
//                {
//                    open.Push(neighbour.TileBlock, tmpCost, evaluation);
//                    parent[neighbour.TileBlock] = tileBlock;
//                    neighbour.TileBlock.SetStatus(TileStatus.INQUEUE);
//                    yield return waitFor50ms;
//                }
//            }
//        }
//    }
//}
