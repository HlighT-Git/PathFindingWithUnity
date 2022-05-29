using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GraphSetup : MonoBehaviour
{
    [SerializeReference] private GameObject map;
    [SerializeReference] private TMP_InputField widthField;
    [SerializeReference] private TMP_InputField heightField;
    [SerializeReference] private Toggle weightGraphToggle;

    private TileMap tileMap;

    public Toggle WeightGraphToggle { get => weightGraphToggle; set => weightGraphToggle = value; }

    void Awake()
    {
        tileMap = map.GetComponent<TileMap>();
    }
    public void CreateGraph()
    {
        if (widthField.text == string.Empty)
        {
            widthField.text = Random.Range(5, 15).ToString();
        }
        if (heightField.text == string.Empty)
        {
            heightField.text = Random.Range(5, 15).ToString();
        }
        tileMap.GenerateMap(System.Int32.Parse(widthField.text), System.Int32.Parse(heightField.text), weightGraphToggle.isOn);
    }
}
