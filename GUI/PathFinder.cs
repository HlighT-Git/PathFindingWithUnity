using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathFinder : MonoBehaviour
{
    public static bool isFinding;
    public static bool isIterativeDepending;
    public static TileBlock startBlock;
    public static TileBlock endBlock;
    public static LinkedListNode<Step> currentStepNode;
    public static LinkedList<Step> steps = new();
    public static LinkedList<TileBlock> path = new();
    public static int totalCost;
    [SerializeReference] private TMP_Dropdown searchType;
    [SerializeReference] private TMP_Dropdown algorithms;
    [SerializeReference] private TMP_Dropdown heuristicDr;
    [SerializeReference] private TMP_InputField betaVal;
    [SerializeReference] private GraphSetup graphSetup;
    [SerializeReference] private GameObject character;
    [SerializeReference] private VisualController visualController;
    private List<TMP_Dropdown.OptionData> uninformedSearchChoices;
    private List<TMP_Dropdown.OptionData> informedSearchChoices;
    private List<TMP_Dropdown.OptionData> heuristicChoices;
    private void Awake()
    {
        RefreshPathFinderStatus();
        uninformedSearchChoices = new();
        informedSearchChoices = new();
        heuristicChoices = new();
        string[] uninformedSearchs = { "DFS", "BFS", "UCS", "IDS" };
        string[] informedSearchs = { "Greedy", "A*", "IDA*" };
        string[] heuristics = { "Manhattan", "Euclidean", "Floyd" };
        foreach (string type in uninformedSearchs)
            uninformedSearchChoices.Add(new TMP_Dropdown.OptionData(type));
        foreach (string type in informedSearchs)
            informedSearchChoices.Add(new TMP_Dropdown.OptionData(type));
        foreach (string type in heuristics)
            heuristicChoices.Add(new TMP_Dropdown.OptionData(type));
    }
    public static void RefreshPathFinderStatus()
    {
        isFinding = false;
        startBlock = null;
        currentStepNode = null;
        endBlock = null;
        steps.Clear();
        totalCost = 0;
    }
    public static void SetTarget(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.startBlock = startBlock;
        PathFinder.endBlock = endBlock;
        startBlock.SetStatus(TileStatus.START);
        endBlock.SetStatus(TileStatus.END);
        steps.Clear();
        totalCost = 0;
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
                UpdateOptionList(uninformedSearchChoices);
                break;
            case 1:
                UpdateOptionList(informedSearchChoices);
                break;
        }
    }
    public void UpdateHeuristic()
    {
        switch (searchType.value)
        {
            case 0:
                heuristicDr.ClearOptions();
                heuristicDr.AddOptions(new List<TMP_Dropdown.OptionData>{ new TMP_Dropdown.OptionData("-- Heuristic --") });
                heuristicDr.interactable = false;
                break;
            case 1:
                {
                    heuristicDr.ClearOptions();
                    heuristicDr.interactable = true;
                    if (graphSetup.WeightGraphToggle.isOn)
                    {
                        heuristicDr.AddOptions(heuristicChoices);
                    }
                    else
                    {
                        heuristicDr.AddOptions(heuristicChoices.GetRange(0, 2));
                    }
                    break;
                }
        }
    }
    public void UpdateSelectedHeuristic()
    {
        Heuristic.type = heuristicDr.value;
    }
    public void UpdateDepthLimitField()
    {
        betaVal.interactable = (searchType.value == 0 && algorithms.value == 3) 
            || (searchType.value == 1 && algorithms.value == 2);
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
            steps.AddLast(new Step(tileBlock, TileStatus.VISITED, TileStatus.PATH));
            totalCost += tileBlock.TileNode.Cost;
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
        int val = IDAStar.betaVal;
        try
        {
            val = System.Int32.Parse(betaVal.text);
        }
        catch
        {
            val = 1;
        }
        finally
        {
            IDAStar.betaVal = val;
            betaVal.text = val.ToString();
        }
        IDAStar.FindPath(startBlock, endBlock);
        MergePathToStep(IDAStar.steps, IDAStar.parent);
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

        if (path.Count == 0)
        {
            steps.AddLast(new Step("- KHÔNG TÌM THẤY ĐƯỜNG ĐI!"));
        }
        else
        {
            steps.AddLast(new Step($"- TÌM ĐƯỢC ĐƯỜNG ĐI VỚI CHI PHÍ: {totalCost}"));
        }
        visualController.StepDisplay.Display();
        EntryToStep(steps.First);
        graphSetup.SetInteracable(false);
        visualController.RefreshButton.interactable = true;
        visualController.RunControllButton.interactable = true;
        visualController.NextStepButton.interactable = true;
    }
    public static void EntryToStep(LinkedListNode<Step> stepNode)
    {
        if (currentStepNode != null)
        {
            currentStepNode.Value.TextVisual.color = Color.black;
            currentStepNode.Value.TextVisual.fontStyle = FontStyles.Normal;
        }
        stepNode.Value.TextVisual.color = Color.red;
        stepNode.Value.TextVisual.fontStyle = FontStyles.Bold;
        if (stepNode.Value.IsAnnounce)
            stepNode.Value.TextVisual.color = Color.red;
        else
        {
            stepNode.Value.TextVisual.color = stepNode.Value.NewStatus;
            stepNode.Value.EntryBlock.SetStatus(TileStatus.ENTRY);
        }
        currentStepNode = stepNode;
    }
    public IEnumerator RunTo(Step target)
    {
        visualController.UpdateInteracable(false);
        int curOrder = currentStepNode.Value.TextVisual.GetComponentInParent<StepJumping>().Order;
        int targetOrder = target.TextVisual.GetComponentInParent<StepJumping>().Order;
        int distance = curOrder - targetOrder;
        while (isFinding 
            && VisualController.visualStatus == VisualController.VisualStatus.RUNNING 
            && currentStepNode.Value != target)
        {
            if (distance < 0)
                yield return visualController.NextStep();
            else
                yield return visualController.BackStep();
        }
        visualController.Stop((currentStepNode == steps.Last) ? 
            VisualController.VisualStatus.DONECONFIRM : 
            VisualController.VisualStatus.IDLE);
        visualController.UpdateInteracable(true);
    }
}