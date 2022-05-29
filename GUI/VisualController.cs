using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VisualController : MonoBehaviour
{
    public static bool isRunning = false;
    public static float delayTime;
    [SerializeReference] private TileMap tileMap;
    [SerializeReference] private PathFinder pathFinder;
    [SerializeReference] private Button runControllButton;
    [SerializeReference] private Button backStepButton;
    [SerializeReference] private Button nextStepButton;
    [SerializeReference] private Button refreshButton;
    [SerializeReference] private Slider speedSlider;

    public Button RunControllButton { get => runControllButton; set => runControllButton = value; }
    public Button BackStepButton { get => backStepButton; set => backStepButton = value; }
    public Button NextStepButton { get => nextStepButton; set => nextStepButton = value; }
    public Button RefreshButton { get => refreshButton; set => refreshButton = value; }

    private void Awake()
    {
        UpdateSpeed();
    }
    public void UpdateSpeed()
    {
        delayTime = speedSlider.maxValue - speedSlider.value;
    }
    void Stop()
    {
        isRunning = false;
        StopCoroutine(pathFinder.Run());
    }
    void Run()
    {
        isRunning = true;
        StartCoroutine(pathFinder.Run());
    }
    void UpdateRunControllButtonText()
    {
        if (isRunning)
        {
            runControllButton.GetComponentInChildren<TextMeshProUGUI>().text = "Dừng";
        }
        else
        {
            if (PathFinder.currentStep != null && PathFinder.currentStep.Next == null)
            {
                runControllButton.GetComponentInChildren<TextMeshProUGUI>().text = "Xong!";
            }
            else
            {
                runControllButton.GetComponentInChildren<TextMeshProUGUI>().text = "Chạy";
            }
        }
    }
    void Done()
    {
        PathFinder.isFinding = false;
        RefreshButtonClicked();
        runControllButton.interactable = false;
    }
    public void RunControllButtonClicked()
    {
        if (isRunning)
        {
            Stop();
        }
        else
        {
            if (PathFinder.currentStep != null && PathFinder.currentStep.Next == null)
            {
                Done();
            }
            else
            {
                Run();
            }
        }
        UpdateInteracable(!isRunning);
        UpdateRunControllButtonText();
    }
    public void NextStep()
    {
        backStepButton.interactable = true;
        TileBlock tileBlock = PathFinder.currentStep.Value.TargetBlock;
        StepType stepType = PathFinder.currentStep.Value.Type;
        if (PathFinder.currentStep.Next != null)
        {
            switch (stepType)
            {
                case StepType.SEE:
                    if (tileBlock != PathFinder.endBlock)
                    {
                        tileBlock.SetStatus(TileStatus.SEEN);
                    }
                    break;
                case StepType.VISIT:
                    if (tileBlock != PathFinder.endBlock)
                    {
                        tileBlock.SetStatus(TileStatus.VISITED);
                    }
                    break;
                case StepType.SEEAGAIN:
                    tileBlock.SetStatus(TileStatus.SEEN);
                    break;
                case StepType.MOVE:
                      tileBlock.SetStatus(TileStatus.PATH);
                    break;
            }
            PathFinder.currentStep = PathFinder.currentStep.Next;
        }
        else
        {
            nextStepButton.interactable = false;
            Stop();
            UpdateRunControllButtonText();
        }
        if (tileBlock == PathFinder.endBlock)
        {
            tileBlock.SetStatus(TileStatus.END);
        }
    }
    public void BackStep()
    {
        nextStepButton.interactable = true;
        TileBlock tileBlock = PathFinder.currentStep.Value.TargetBlock;
        StepType stepType = PathFinder.currentStep.Value.Type;
        switch (stepType)
        {
            case StepType.SEE:
                if (tileBlock != PathFinder.endBlock)
                {
                    tileBlock.SetStatus(TileStatus.NORMAL);
                }
                break;
            case StepType.VISIT:
                if (tileBlock != PathFinder.endBlock)
                {
                    tileBlock.SetStatus(TileStatus.SEEN);
                }
                break;
            case StepType.SEEAGAIN:
                tileBlock.SetStatus(TileStatus.VISITED);
                break;
            case StepType.MOVE:
                if (tileBlock != PathFinder.endBlock)
                {
                    tileBlock.SetStatus(TileStatus.VISITED);
                }
                break;
        }
        if (PathFinder.currentStep.Previous != null)
        {
            PathFinder.currentStep = PathFinder.currentStep.Previous;
        }
        else
        {
            backStepButton.interactable = false;
        }
        UpdateRunControllButtonText();
    }
    public void RefreshButtonClicked()
    {
        PathFinder.RefreshPathFinderStatus();
        tileMap.RefreshMap();
        UpdateRunControllButtonText();
        UpdateInteracable(false);
    }
    public void UpdateInteracable(bool status)
    {
        backStepButton.interactable = status;
        nextStepButton.interactable = status;
        refreshButton.interactable = status;
    }
}
