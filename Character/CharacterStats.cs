using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeReference] private GameObject map;
    public TileBlock CurrentTileBlockStanding()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        Vector2 index = TileMap.GetGraphIndexByCoordinates(x, y);
        return TileMap.Graph[Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y)].TileBlock;
    }
}
