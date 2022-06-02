using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VisualController : MonoBehaviour
{
    public enum VisualStatus
    {
        IDLE,
        RUNNING,
        DONECONFIRM
    }
    public static VisualStatus visualStatus;
    public static IEnumerator runProcess;
    public static float delayTime;
    [SerializeReference] private TileMap tileMap;
    [SerializeReference] private PathFinder pathFinder;
    [SerializeReference] private Button runControllButton;
    [SerializeReference] private Button backStepButton;
    [SerializeReference] private Button nextStepButton;
    [SerializeReference] private Button refreshButton;
    [SerializeReference] private Slider speedSlider;
    [SerializeReference] private StepDisplay stepDisplay;
    [SerializeReference] private GraphSetup graphSetup;


    public Button RunControllButton { get => runControllButton; set => runControllButton = value; }
    public Button BackStepButton { get => backStepButton; set => backStepButton = value; }
    public Button NextStepButton { get => nextStepButton; set => nextStepButton = value; }
    public Button RefreshButton { get => refreshButton; set => refreshButton = value; }
    public StepDisplay StepDisplay { get => stepDisplay; set => stepDisplay = value; }
    public PathFinder PathFinder { get => pathFinder; set => pathFinder = value; }

    private void Awake()
    {
        UpdateSpeed();
    }
    public void UpdateSpeed()
    {
        delayTime = speedSlider.maxValue - speedSlider.value;
    }
    public void Stop(VisualStatus vs)
    {
        visualStatus = vs;
        UpdateRunControllButtonText();
    }
    public void Run(Step step)
    {
        visualStatus = VisualStatus.RUNNING;
        runProcess = pathFinder.RunTo(step);
        StartCoroutine(runProcess);
        UpdateRunControllButtonText();
    }
    void UpdateRunControllButtonText()
    {
        string text = string.Empty;
        switch (visualStatus)
        {
            case VisualStatus.RUNNING:
                text = "Dừng";
                break;
            case VisualStatus.IDLE:
                text = "Chạy";
                break;
            case VisualStatus.DONECONFIRM:
                text = "Xong!";
                break;
        }
        runControllButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    IEnumerator Done()
    {
        runControllButton.interactable = false;
        backStepButton.interactable = false;
        visualStatus = VisualStatus.IDLE;
        yield return tileMap.Player.GetComponent<CharacterActions>().Move(PathFinder.path);
        PathFinder.isFinding = false;
        RefreshButtonClicked();
    }
    public void RunControllButtonClicked()
    {
        switch (visualStatus)
        {
            case VisualStatus.RUNNING:
                {
                    Stop(VisualStatus.IDLE);
                    break;
                }
            case VisualStatus.IDLE:
                {
                    Run(PathFinder.steps.Last.Value);
                    break;
                }
            case VisualStatus.DONECONFIRM:
                StartCoroutine(Done());
                break;
        }
    }
    public void NextStepButtonClicked()
    {
        StartCoroutine(NextStep());
    }
    public IEnumerator NextStep()
    {
        if (visualStatus != VisualStatus.RUNNING)
        {
            nextStepButton.interactable = false;
            backStepButton.interactable = false;
        }
        Step step = PathFinder.currentStepNode.Value;
        if (!step.IsAnnounce && step.EntryBlock.Status == step.OldStatus)
            step.EntryBlock.SetStatus(step.NewStatus);
        PathFinder.EntryToStep(PathFinder.currentStepNode.Next);
        yield return new WaitForSeconds(delayTime);
        step = PathFinder.currentStepNode.Value;
        if (step.IsAnnounce)
        {
            if(step.Text.Contains('*'))
                TileMap.TransformBlocks(TileStatus.VISITED, TileStatus.NORMAL);
        }
        else{
            if (step.EntryBlock != PathFinder.endBlock)
                step.EntryBlock.SetStatus(step.NewStatus);
            else
                step.EntryBlock.SetStatus(TileStatus.END);
        }
        yield return new WaitForSeconds(delayTime);
            nextStepButton.interactable = false;
        if (PathFinder.currentStepNode == PathFinder.steps.Last)
        {
            visualStatus = VisualStatus.DONECONFIRM;
            UpdateRunControllButtonText();
        }
        if (visualStatus != VisualStatus.RUNNING)
        {
            nextStepButton.interactable = PathFinder.currentStepNode != PathFinder.steps.Last;
            backStepButton.interactable = !PathFinder.isIterativeDepending;
        }
    }
    public void BackStepButtonClicked()
    {
        StartCoroutine(BackStep());
    }
    public IEnumerator BackStep()
    {
        if (visualStatus != VisualStatus.RUNNING)
        {
            nextStepButton.interactable = false;
            backStepButton.interactable = false;
        }
        if (visualStatus == VisualStatus.DONECONFIRM)
        {
            visualStatus = VisualStatus.IDLE;
            UpdateRunControllButtonText();
        }
        Step step = PathFinder.currentStepNode.Value;
        PathFinder.EntryToStep(PathFinder.currentStepNode);
        yield return new WaitForSeconds(delayTime);
        if (!step.IsAnnounce && step.EntryBlock.Status != step.OldStatus)
            step.EntryBlock.SetStatus(step.OldStatus);
        PathFinder.EntryToStep(PathFinder.currentStepNode.Previous);
        step = PathFinder.currentStepNode.Value;
        if (!step.IsAnnounce)
        {
            if (step.EntryBlock != PathFinder.endBlock)
                step.EntryBlock.SetStatus(step.NewStatus);
            else
                step.EntryBlock.SetStatus(TileStatus.END);
        }
        yield return new WaitForSeconds(delayTime);
        if (visualStatus != VisualStatus.RUNNING)
        {
            nextStepButton.interactable = true;
            backStepButton.interactable = PathFinder.currentStepNode != PathFinder.steps.First;
        }
    }
    public void RefreshButtonClicked()
    {
        stepDisplay.RefreshStepDisplay();
        TileMap.RefreshMap();
        PathFinder.RefreshPathFinderStatus();
        UpdateRunControllButtonText();
        runControllButton.interactable = false;
        graphSetup.SetInteracable(true);
        UpdateInteracable(false);
    }
    public void UpdateInteracable(bool status)
    {
        backStepButton.interactable = !PathFinder.isIterativeDepending 
            && PathFinder.currentStepNode != PathFinder.steps.First
            && status;
        nextStepButton.interactable = PathFinder.currentStepNode != PathFinder.steps.Last
            && status;
        refreshButton.interactable = status;
    }
}
