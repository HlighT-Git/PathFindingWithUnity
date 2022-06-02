using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepJumping : MonoBehaviour
{
    private Step step;
    private int order;
    public Step Step { get => step; set => step = value; }
    public int Order { get => order; set => order = value; }

    public void StepClicked()
    {
        VisualController visualController = GetComponentInParent<StepDisplay>().visualController;
        if (VisualController.visualStatus == VisualController.VisualStatus.RUNNING)
            return;
        visualController.Run(step);
    }
}
