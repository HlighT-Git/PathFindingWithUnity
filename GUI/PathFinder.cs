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
    public static LinkedList<Step> steps;
    public static LinkedList<TileBlock> path;
    [SerializeReference] private TMP_Dropdown searchType;
    [SerializeReference] private TMP_Dropdown algorithms;
    [SerializeReference] private TMP_InputField heuristic;
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
        steps = null;
    }
    public static void SetTarget(TileBlock startBlock, TileBlock endBlock)
    {
        PathFinder.startBlock = startBlock;
        PathFinder.endBlock = endBlock;
        startBlock.SetStatus(TileStatus.START);
        endBlock.SetStatus(TileStatus.END);
        steps = new();
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
    void FindByDFS()
    {
        DFS.FindPath(startBlock, endBlock);
        steps = DFS.steps;
        //if (DFS.open == null)
        //{
        //    return;
        //}
        //path = DFS.path;
        //LinkedListNode<TileBlock> entry = path.First;
        //steps.AddLast(new Step(StepType.MOVE, startBlock, entry.Value));
        //entry = entry.Next;
        //while (entry != null)
        //{
        //    steps.AddLast(new Step(StepType.MOVE, entry.Previous.Value, entry.Value));
        //    entry = entry.Next;
        //}
    }
    void FindByBFS()
    {
        return;
    }
    void FindByUCS()
    {
        return;
    }
    void FindByIDS()
    {
        return;
    }
    void FindByGreedy()
    {
        return;
    }
    void FindByAStar()
    {
        //return AStar.FindPath(character, startBlock, endBlock));
        return;
    }
    void FindByIDAStar()
    {
        return;
    }
    public void FindPath(TileBlock startBlock, TileBlock endBlock)
    {
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
        visualController.RunControllButton.interactable = true;
        visualController.NextStepButton.interactable = true;
        visualController.RefreshButton.interactable = true;
        currentStep = steps.First;
    }
    public IEnumerator Run()
    {
        isFinding = true;
        while (isFinding)
        {
            if (VisualController.isRunning)
            {
                currentStep.Value.TargetBlock.SetStatus(TileStatus.ENTRY);
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