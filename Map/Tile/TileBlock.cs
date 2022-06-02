using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TileBlock : MonoBehaviour
{
    private TextMeshProUGUI costText;
    private TileCube tileCube;

    private TileMap tileMap;
    private TileMap.TileNode tileNode;
    private SearchEntry node;
    private Color status;

    public TileMap TileMap { get => tileMap; set => tileMap = value; }
    public TileMap.TileNode TileNode { get => tileNode; set => tileNode = value; }
    public SearchEntry Node { get => node; set => node = value; }
    public Color Status { get => status; set => status = value; }

    public void SetStatus(Color status)
    {
        this.status = status;
        tileCube.SetColorByStatus();
    }
    private void Awake()
    {
        costText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        tileCube = transform.GetChild(0).GetComponent<TileCube>();
    }
    public void InitTileBlock(TileMap.TileNode node)
    {
        this.tileNode = node;
        node.TileBlock = this;
        status = (TileNode.Cost != 0) ? TileStatus.NORMAL : TileStatus.UNMOVABLE;
        if (TileNode.Cost != 0)
        {
            tileCube.OriginalColor = new Color((255 - 50 * TileNode.Cost) / 255f, 1, 1);
            costText.text = TileNode.Cost.ToString();
        }
        tileCube.SetColorByStatus();
    }
}
