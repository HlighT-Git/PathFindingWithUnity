using System;
using UnityEngine;
public class SearchEntry : IComparable<SearchEntry>//, IEquatable<Node>
{
    private readonly int id;
    private TileBlock block;
    private int gainCost;
    private int value;
    public SearchEntry(TileBlock block, int value)
    {
        this.block = block;
        block.Node = this;
        Vector2 index = block.TileNode.Index();
        id = Mathf.RoundToInt(index.x) * TileMap.mapSizeX + Mathf.RoundToInt(index.y);

        this.value = value;
    }

    public SearchEntry(TileBlock block, int gainCost, int value)
    {
        this.block = block;
        block.Node = this;
        Vector2 index = block.TileNode.Index();
        id = Mathf.RoundToInt(index.x) * TileMap.mapSizeX + Mathf.RoundToInt(index.y);

        this.gainCost = gainCost;
        this.value = value;
    }

    public TileBlock Block { get => block; set => block = value; }
    public int GainCost { get => gainCost; set => gainCost = value; }
    public int Value { get => value; set => this.value = value; }

    public int CompareTo(SearchEntry other)
    {
        if (this.value == other.value)
            return this.id - other.id;
        return this.value - other.value;
    }

    //public bool Equals(Node other)
    //{
    //    if (other == null) return false;
    //    return this.id.Equals(other.id);
    //}
    //public override bool Equals(object obj)
    //{
    //    if (obj == null) return false;
    //    if (obj is not Node objAsNode) return false;
    //    return Equals(objAsNode);
    //}
    //public override int GetHashCode()
    //{
    //    return id;
    //}
}
