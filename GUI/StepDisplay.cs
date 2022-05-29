using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StepDisplay : MonoBehaviour
{
    [SerializeReference] private GameObject textPrefab;
    private List<string> textList;
    private List<TextMeshProUGUI> TMPList;
    private void Awake()
    {
        textList = new();
        TMPList = new();
    }
    public void Display(LinkedList<Step> steps, params string[] announces)
    {
        foreach (Step step in steps)
        {
            textList.Add(step.Text);
        }
        foreach (string announce in announces)
        {
            textList.Add(announce);
        }
        var rt = GetComponent<RectTransform>();
        if (textList.Count > 11)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 15 * (textList.Count - 12));
            rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y - 7.5f * (textList.Count - 12), rt.localPosition.z);
        }
        for (int i = 0; i < textList.Count; i++)
        {
            GameObject go = Instantiate(textPrefab, transform.GetChild(0));
            go.GetComponent<RectTransform>().localPosition = new Vector3(10, rt.sizeDelta.y/2 - 15 * (i + 1), 0);
            TMPList.Add(go.GetComponent<TextMeshProUGUI>());
            TMPList[i].text = textList[i];
        }
    }
    public void RefreshStepDisplay()
    {
        foreach (TextMeshProUGUI tmp in TMPList)
        {
            Destroy(tmp.gameObject);
        }
        textList.Clear();
        TMPList.Clear();
        var rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 200);
        rt.localPosition = new Vector3(-8.5f, 0, 0);
    }
}
