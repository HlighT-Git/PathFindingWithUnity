using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    public static bool isMoving = false;

    private void Awake()
    {
        this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        float rollSpeed = 10 - 10 * VisualController.delayTime / 0.2f;
        rollSpeed = rollSpeed < 1 ? 1 : rollSpeed;
        for (int i = 0; i < 90 / rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, rollSpeed);
            yield return new WaitForSeconds(Time.deltaTime / 2);
        }
    }
    public IEnumerator Move(LinkedList<TileBlock> path)
    {
        isMoving = true;
        TileBlock currentBlock = GetComponent<CharacterStats>().CurrentTileBlockStanding();
        foreach (TileBlock block in path)
        {            Vector3 dir = Vector3.zero;
            if (block.TileNode.Position.x < currentBlock.TileNode.Position.x) dir = Vector3.left;
            if (block.TileNode.Position.x > currentBlock.TileNode.Position.x) dir = Vector3.right;
            if (block.TileNode.Position.y < currentBlock.TileNode.Position.y) dir = Vector3.down;
            if (block.TileNode.Position.y > currentBlock.TileNode.Position.y) dir = Vector3.up;
            if (dir == Vector3.zero) continue;
            Vector3 anchor = (currentBlock.TileNode.Position + block.TileNode.Position + Vector3.back) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.back, dir);
            currentBlock = block;
            yield return Roll(anchor, axis);
        }
        isMoving = false;
    }
}
