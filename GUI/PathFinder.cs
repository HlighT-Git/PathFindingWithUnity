using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathFinder : MonoBehaviour
{
    public static bool isFinding;
    public static TileBlock startBlock;
    public static TileBlock endBlock;
    public static LinkedListNode<Step> currentStep;
    public static LinkedList<Step> steps = new();
    public static LinkedList<TileBlock> path = new();
    [SerializeReference] private TMP_Dropdown searchType;
    [SerializeReference] private TMP_Dropdown algorithms;
    [SerializeReference] private TMP_InputField heuristic;
    [SerializeReference] private TMP_InputField depthLimit;
    [SerializeReference] private GraphSetup graphSetup;
    [SerializeReference] private GameObject character;
    [SerializeReference] private VisualController visualController;
    private List<TMP_Dropdown.OptionData> uninformedSearch;
    private List<TMP_Dropdown.OptionData> informedSearch;

    private void Awake()
    {
        RefreshPathFinderStatus();
        uninformedSearch = new();
        informedSearch = new();
        string[] searchType1 = { "DFS", "BFS", "UCS", "IDS" };
        foreach (string type in searchType1)
        {
            uninformedSearch.Add(new TMP_Dropdown.OptionData(type));
        }
        string[] searchType2 = { "Greedy", "A*", "IDA*" };
        foreach (string type in searchType2)
        {
            informedSearch.Add(new TMP_Dropdown.OptionData(type));
        }
    }
    public static void RefreshPathFinderStatus()
    {
        isFinding = false;
        startBlock = null;
        currentStep = null;
        endBlock = null;
        steps.Clear();
    }
    public static void SetTarget(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.startBlock = startBlock;
        PathFinder.endBlock = endBlock;
        startBlock.SetStatus(TileStatus.START);
        endBlock.SetStatus(TileStatus.END);
        steps.Clear();
    }
    public void UpdateAlgorithmOptions()
    {
        void UpdateOptionList(List<TMP_Dropdown.OptionData> options)
        {
            algorithms.ClearOptions();
            algorithms.AddOptions(options);
        }
        switch (searchType.value)
        {
            case 0:
                UpdateOptionList(uninformedSearch);
                break;
            case 1:
                UpdateOptionList(informedSearch);
                break;
        }
    }
    public void UpdateHeuristic()
    {
        switch (searchType.value)
        {
            case 0:
                heuristic.text = string.Empty;
                break;
            case 1:
                {
                    if (graphSetup.WeightGraphToggle.isOn)
                    {
                        heuristic.text = "Floyd";
                    }
                    else
                    {
                        heuristic.text = "Manhattan";
                    }
                    break;
                }
        }
    }
    public void UpdateDepthLimitField()
    {
        if ((searchType.value == 0 && algorithms.value == 3)
            || (searchType.value == 1 && algorithms.value == 2))
        {
            depthLimit.interactable = true;
        }
        else
        {
            depthLimit.interactable = false;
        }
    }
    void MergePathToStep(LinkedList<Step> steps, Dictionary<TileBlock, TileBlock> parent)
    {
        PathFinder.steps = steps;
        path.Clear();
        if (parent.Count == 0)
        {
            return;
        }
        TileBlock entry = endBlock;
        while (entry != startBlock)
        {
            path.AddFirst(entry);
            entry = parent[entry];
        }
        foreach (TileBlock tileBlock in path)
        {
            steps.AddLast(new Step(StepType.MOVE, tileBlock));
        }
    }
    void FindByDFS()
    {
        DFS.FindPath(startBlock, endBlock);
        MergePathToStep(DFS.steps, DFS.parent);
    }
    void FindByBFS()
    {
        BFS.FindPath(startBlock, endBlock);
        MergePathToStep(BFS.steps, BFS.parent);
    }
    void FindByUCS()
    {
        UCS.FindPath(startBlock, endBlock);
        MergePathToStep(UCS.steps, UCS.parent);
    }
    void FindByIDS()
    {
        if (depthLimit.text == string.Empty)
        {
            depthLimit.text = IDS.depthLim.ToString();
        }
        else
        {
            IDS.depthLim = System.Int32.Parse(depthLimit.text);
        }
        IDS.FindPath(startBlock, endBlock);
        MergePathToStep(IDS.steps, IDS.parent);
    }
    void FindByGreedy()
    {
        Greedy.FindPath(startBlock, endBlock);
        MergePathToStep(Greedy.steps, Greedy.parent);
    }
    void FindByAStar()
    {
        AStar.FindPath(startBlock, endBlock);
        MergePathToStep(AStar.steps, AStar.parent);
    }
    void FindByIDAStar()
    {
        AStar.FindPath(startBlock, endBlock);
        MergePathToStep(AStar.steps, AStar.parent);
    }
    public void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
        isFinding = true;
        SetTarget(startBlock, endBlock);
        switch (searchType.value)
        {
            case 0:
                {
                    switch (algorithms.value)
                    {
                        case 0: FindByDFS();
                            break;
                        case 1: FindByBFS();
                            break;
                        case 2: FindByUCS();
                            break;
                        case 3: FindByIDS();
                            break;
                    }
                    break;
                }
            case 1:
                {
                    switch (algorithms.value)
                    {
                        case 0: FindByGreedy();
                            break;
                        case 1: FindByAStar();
                            break;
                        case 2: FindByIDAStar();
                            break;
                    }
                    break;
                }
        }
        visualController.RefreshButton.interactable = true;
        currentStep = steps.First;
        
        if (path.Count == 0)
        {
            visualController.StepDisplay.Display(steps, "- KHÔNG TÌM THẤY ĐƯỜNG ĐI!");
        }
        else
        {
            visualController.RunControllButton.interactable = true;
            visualController.NextStepButton.interactable = true;
            visualController.StepDisplay.Display(steps);
        }
    }
    public IEnumerator Run()
    {
        while (isFinding)
        {
            if (VisualController.isRunning)
            {
                currentStep.Value.EntryBlock.SetStatus(TileStatus.ENTRY);
                yield return new WaitForSeconds(VisualController.delayTime);
                visualController.NextStep();
                yield return new WaitForSeconds(VisualController.delayTime);
            }
            else
            {
                yield return null;
            }
        }
    }
}