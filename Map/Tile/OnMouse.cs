using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour
{
    private Color previousStatus;
    private TileBlock tileBlockParent;
    public static bool hoverABlock = false;

    private void Awake()
    {
        tileBlockParent = this.gameObject.GetComponent<TileCube>().TileBlockParent;
    }

    private void OnMouseDown()
    {
        TileMap tilemap = tileBlockParent.Tilemap;
        if (PathFinder.isFinding
            || TileStatus.Match(tileBlockParent.Status,
            TileStatus.UNMOVABLE,
            TileStatus.START, 
            TileStatus.END)
            ){
            return;
        }
        tilemap.RefreshMapExcept();
        if (!TileMap.upToDate)
        {
            tilemap.UpdateMap();
        }
        TileBlock startBlock = tilemap.Player.GetComponent<CharacterStats>().CurrentTileBlockStanding();
        TileBlock endBlock = tileBlockParent;
        tileBlockParent.Tilemap.PathFinder.FindPath(startBlock, endBlock);
    }
    private void OnMouseEnter()
    {
        hoverABlock = true;
        if (TileStatus.Match(tileBlockParent.Status, TileStatus.UNMOVABLE, TileStatus.START, TileStatus.END))
        {
            return;
        }
        if (tileBlockParent.Status != TileStatus.HOVER)
            previousStatus = tileBlockParent.Status;
        tileBlockParent.SetStatus(TileStatus.HOVER);
    }
    private void OnMouseExit()
    {
        hoverABlock = false;
        if (tileBlockParent.Status == TileStatus.HOVER)
        {
            tileBlockParent.SetStatus(previousStatus);
        }
        GameObject player = tileBlockParent.Tilemap.Player;
        if (Input.GetMouseButton(1) && !PathFinder.isFinding)
        {
            if (player.transform.position.x != tileBlockParent.transform.position.x
                || player.transform.position.y != tileBlockParent.transform.position.y)
            {
                tileBlockParent.Node.ChangeStatus();
            }
        }
    }
}
