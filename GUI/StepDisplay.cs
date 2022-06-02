using UnityEngine;
using TMPro;
public class StepDisplay : MonoBehaviour
{
    [SerializeReference] private GameObject textPrefab;
    public VisualController visualController;
    public void Display()
    {
        var rt = GetComponent<RectTransform>();
        int numberOfSteps = PathFinder.steps.Count;
        if (numberOfSteps > 11)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 15 * (numberOfSteps - 12));
            rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y - 7.5f * (numberOfSteps - 12), rt.localPosition.z);
        }
        int line = 0;
        foreach (Step step in PathFinder.steps)
        {
            ++line;
            GameObject go = Instantiate(textPrefab, transform);
            step.InitTextVisual(go.GetComponentInChildren<TextMeshProUGUI>());
            go.GetComponent<StepJumping>().Step = step;
            go.GetComponent<StepJumping>().Order = line;
            go.transform.localPosition = new Vector3(0, rt.sizeDelta.y / 2 - 15 * line, 0);
        }
    }
    public void RefreshStepDisplay()
    {
        foreach (Step step in PathFinder.steps)
        {
            Destroy(step.TextVisual.transform.parent.gameObject);
        }
        var rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 200);
        rt.localPosition = new Vector3(-8.5f, 0, 0);
    }
}
