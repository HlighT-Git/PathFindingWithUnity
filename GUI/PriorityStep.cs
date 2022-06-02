using UnityEngine;
using TMPro;
public class PriorityStep : Step
{
    protected int cost;
    public PriorityStep(
        TileBlock entryBlock,
        Color oldStatus,
        Color newStatus,
        int currentCost)
        : base(entryBlock, oldStatus, newStatus)
    {
        this.cost = currentCost;
    }
    public override void InitTextVisual(TextMeshProUGUI textVisual)
    {
        if (!IsAnnounce) { 
            Vector2 entryBlockPos = EntryBlock.TileNode.Index();
            int x = Mathf.RoundToInt(entryBlockPos.x);
            int y = Mathf.RoundToInt(entryBlockPos.y);
            if (NewStatus == TileStatus.SEEN)
                Text = $"- Thêm vào Open ({x}, {y}) <{cost}>.";
            else if (NewStatus == TileStatus.VISITED)
                Text = $"- Duyệt nút ({x}, {y}) <{cost}>.";
            else if (NewStatus == TileStatus.PATH || NewStatus == TileStatus.END)
                Text = $"- Di chuyển tới ({x}, {y}).";
        }
        TextVisual = textVisual;
        TextVisual.text = Text;
    }
}
