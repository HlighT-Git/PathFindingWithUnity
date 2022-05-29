using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public static bool upToDate = false;
    public static int mapSizeX;
    public static int mapSizeY;
    public static int mapRootX;
    public static int mapRootY;
    public static bool isWeightGraphMap;
    private static Node[,] graph;
    public static Node[,] Graph { get => graph; set => graph = value; }
    public static Vector2 GetGraphIndexByCoordinates(float x, float y)
    {
        return new Vector2(x - mapRootX, y - mapRootY);
    }
    public class Node
    {
        private TileBlock tileBlock;
        private List<Node> neighbours;
        private Vector3 position;
        private int cost;
        private int[,] costTo;
        private KeyValuePair<int, int> searchCost;

        public Node(int cost)
        {
            neighbours = new List<Node>();
            this.cost = cost;
            costTo = new int[mapSizeX, mapSizeY];
        }
        public Vector2 Index()
        {
            return new Vector2(position.x - mapRootX, position.y - mapRootY);
        }
        public void FindNeighbours()
        {
            Vector2 index = Index();
            int x = Mathf.RoundToInt(index.x);
            int y = Mathf.RoundToInt(index.y);
            CheckAndAddNeighbour(x, y, x - 1, y);
            CheckAndAddNeighbour(x, y, x, y - 1);
            CheckAndAddNeighbour(x, y, x + 1, y);
            CheckAndAddNeighbour(x, y, x, y + 1);
        }
        public void CheckAndAddNeighbour(int x, int y, int i, int j)
        {
            if (x < 0 || y < 0 ||
                x > mapSizeX - 1 ||
                y > mapSizeY - 1 || 
                i < 0 || j < 0 ||
                i > mapSizeX - 1 ||
                j > mapSizeY - 1 ||
                graph[i, j].Cost == 0 ||
                graph[x, y].neighbours.Contains(graph[i, j]) ||
                Mathf.Abs(x - i) + Mathf.Abs(y - j) != 1)
            {
                return;
            }
            graph[x, y].neighbours.Add(graph[i, j]);
        }
        public void ChangeStatus()
        {
            upToDate = false;
            if (cost > 0)
            {
                cost = 0;
                foreach(Node ele in neighbours)
                {
                    ele.Neighbours.Remove(this);
                }
                neighbours.Clear();
            }
            else
            {
                cost = 1;
                if (isWeightGraphMap)
                {
                    cost = Random.Range(1, 7);
                }
                FindNeighbours();
                Vector2 index = Index();
                int x = Mathf.RoundToInt(index.x);
                int y = Mathf.RoundToInt(index.y);
                CheckAndAddNeighbour(x - 1, y, x, y);
                CheckAndAddNeighbour(x, y - 1, x, y);
                CheckAndAddNeighbour(x + 1, y, x, y);
                CheckAndAddNeighbour(x, y + 1, x, y);
            }
            tileBlock.InitTileBlock(this);
        }
        public TileBlock TileBlock { get => tileBlock; set => tileBlock = value; }
        public List<Node> Neighbours { get => neighbours; set => neighbours = value; }
        public Vector3 Position { get => position; set => position = value; }
        public int Cost { get => cost; set => cost = value; }
        public int[,] CostTo { get => costTo; set => costTo = value; }
        public KeyValuePair<int, int> SearchCost { get => searchCost; set => searchCost = value; }
    }
    [SerializeReference] private GameObject tileVisualPrefab;
    [SerializeReference] private GameObject player;
    //[SerializeReference] private GameObject graphSetup;
    [SerializeReference] private PathFinder pathFinder;
    [SerializeReference] private VisualController visualController;
    public GameObject Player { get => player; set => player = value; }
    public VisualController VisualController { get => visualController; set => visualController = value; }
    public PathFinder PathFinder { get => pathFinder; set => pathFinder = value; }

    public void GenerateMap(int sizeX, int sizeY, bool isWeightGraph)
    {
        mapSizeX = sizeX;
        mapSizeY = sizeY;
        isWeightGraphMap = isWeightGraph;
        GenerateMapData();
        GenerateMapVisual();
        GenerateGraph();
        InitWorldStatus();
        UpdateMap();
    }
    public void UpdateMap()
    {
        Floyd.CalculateAllPairCost();
        upToDate = true;
    }
    void Start()
    {
        GenerateMap(10, 10, true);
    }
    private void GenerateMapData()
    {
        mapRootX = - mapSizeX / 2;
        mapRootY = - mapSizeY / 2;
        graph = new Node[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                int cost = Random.Range(1, 7);
                if (!isWeightGraphMap)
                {
                    cost = 1;
                }
                graph[x, y] = new Node(cost);
            }
        }
    }
    private void InitWorldStatus()
    {
        int playerX, playerY;
        playerX = Random.Range(0, mapSizeX);
        playerY = Random.Range(0, mapSizeY);
        while (graph[playerX, playerY].Cost == 0)
        {
            playerX = Random.Range(0, mapSizeX);
            playerY = Random.Range(0, mapSizeY);
        }
        player.transform.position = new Vector3(graph[playerX, playerY].Position.x, graph[playerX, playerY].Position.y, -1);
    }
    private void GenerateGraph()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (graph[x, y].Cost == 0)
                    continue;
                graph[x, y].FindNeighbours();
            }
        }
    }
    public void RefreshMapExcept(params Color[] colors)
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                bool needReset = graph[x, y].Cost != 0;
                foreach (Color color in colors)
                {
                    if (graph[x, y].TileBlock.Status == color)
                    {
                        needReset = false;
                        break;
                    }
                }
                if (needReset)
                {
                    graph[x, y].TileBlock.SetStatus(TileStatus.NORMAL);
                }
            }
        }
    }
    public void RefreshMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (graph[x, y].Cost != 0)
                {
                    graph[x, y].TileBlock.SetStatus(TileStatus.NORMAL);
                }
            }
        }
    }
    private void GenerateMapVisual()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y].Position = new Vector3(x + mapRootX, y + mapRootY, 0);
                GameObject go = Instantiate(tileVisualPrefab
                    , new Vector3(graph[x, y].Position.x, graph[x, y].Position.y), Quaternion.identity);
                go.transform.parent = gameObject.transform;
                TileBlock tb = go.GetComponent<TileBlock>();
                tb.Tilemap = this;
                tb.InitTileBlock(graph[x, y]);
            }
        }
    }

    public void DestroyMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
