using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    private float rollSpeed = 5;

    private void Awake()
    {
        this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        for (int i = 0; i < 90 / rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, rollSpeed);
            yield return new WaitForSeconds(Time.deltaTime / 2);
        }
    }
    public IEnumerator Move(LinkedList<TileBlock> path)
    {
        TileBlock currentBlock = GetComponent<CharacterStats>().CurrentTileBlockStanding();
        void Coloring(TileBlock tileBlock)
        {
            if (tileBlock == path.Last.Value)
            {
                tileBlock.SetStatus(TileStatus.START);
            }
            else if (tileBlock == path.First.Value)
            {
                tileBlock.SetStatus(TileStatus.END);
            }
            else
            {
                tileBlock.SetStatus(TileStatus.PATH);
            }
        }
        foreach (TileBlock block in path)
        {
            Coloring(block);
            Vector3 dir = Vector3.zero;
            if (block.Node.Position.x < currentBlock.Node.Position.x) dir = Vector3.left;
            if (block.Node.Position.x > currentBlock.Node.Position.x) dir = Vector3.right;
            if (block.Node.Position.y < currentBlock.Node.Position.y) dir = Vector3.down;
            if (block.Node.Position.y > currentBlock.Node.Position.y) dir = Vector3.up;
            if (dir == Vector3.zero) continue;
            Vector3 anchor = (currentBlock.Node.Position + block.Node.Position + Vector3.back) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.back, dir);
            currentBlock = block;
            yield return Roll(anchor, axis);
        }
    }
}
