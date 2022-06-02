using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GraphSetup : MonoBehaviour
{
    [SerializeReference] private GameObject map;
    [SerializeReference] private TMP_InputField widthField;
    [SerializeReference] private TMP_InputField heightField;
    [SerializeReference] private Toggle weightGraphToggle;
    [SerializeReference] private Button createGraphButton;

    public Toggle WeightGraphToggle { get => weightGraphToggle; set => weightGraphToggle = value; }
    public void CreateGraph()
    {
        static void Validate(out int mapSize, string text)
        {
            try
            {
                mapSize = System.Int32.Parse(text);
                if (mapSize > 25)
                    mapSize = 25;
            }
            catch
            {
                mapSize = Random.Range(7, 20);
            }
        }
        Validate(out int mapSizeX, widthField.text);
        Validate(out int mapSizeY, heightField.text);
        widthField.text = mapSizeX.ToString();
        heightField.text = mapSizeY.ToString();
        map.GetComponent<TileMap>().GenerateMap(mapSizeX, mapSizeY, weightGraphToggle.isOn);
    }
    public void SetInteracable(bool status)
    {
        widthField.interactable = status;
        heightField.interactable = status;
        weightGraphToggle.interactable = status;
        createGraphButton.interactable = status;
    }
}
