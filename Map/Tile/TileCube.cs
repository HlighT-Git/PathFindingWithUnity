using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCube : MonoBehaviour
{
    private TileBlock tileBlockParent;
    private Renderer cubeRenderer;
    private Color originalColor;

    public TileBlock TileBlockParent { get => tileBlockParent; set => tileBlockParent = value; }
    public Renderer CubeRenderer { get => cubeRenderer; set => cubeRenderer = value; }
    public Color OriginalColor { get => originalColor; set => originalColor = value; }

    private void Awake()
    {
        CubeRenderer = this.gameObject.GetComponent<Renderer>();
        tileBlockParent = this.gameObject.transform.parent.GetComponent<TileBlock>();
    }
    public void SetColor(Color color)
    {
        CubeRenderer.material.color = color;
    }
    public void SetColorByStatus()
    {
        if (tileBlockParent.Status == TileStatus.NORMAL)
            SetColor(originalColor);
        else
            SetColor(tileBlockParent.Status);
    }
}