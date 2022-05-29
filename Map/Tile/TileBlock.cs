using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileBlock : MonoBehaviour
{
    private TextMeshProUGUI costText;

    private TileCube tilecube;
    private TileMap tilemap;
    private TileMap.Node node;
    private Color status;

    public TileCube Tilecube { get => tilecube; set => tilecube = value; }
    public TileMap Tilemap { get => tilemap; set => tilemap = value; }
    public TileMap.Node Node { get => node; set => node = value; }
    public Color Status { get => status; set => status = value; }

    public void SetStatus(Color status)
    {
        this.status = status;
        tilecube.SetColorByStatus();
    }
    private void Awake()
    {
        costText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        tilecube = transform.GetChild(0).GetComponent<TileCube>();
    }
    public void InitTileBlock(TileMap.Node node)
    {
        this.node = node;
        node.TileBlock = this;
        status = (Node.Cost != 0) ? TileStatus.NORMAL : TileStatus.UNMOVABLE;
        if (Node.Cost != 0)
        {
            tilecube.OriginalColor = new Color((255 - 50 * Node.Cost) / 255f, 1, 1);
            costText.text = Node.Cost.ToString();
        }
        tilecube.SetColorByStatus();
    }
}
