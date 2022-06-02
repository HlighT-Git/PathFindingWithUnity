using UnityEngine;
using TMPro;
public class InformStep : PriorityStep
{
    private int evaluateValue;

    public InformStep(
        TileBlock entryBlock,
        Color oldStatus,
        Color newStatus,
        int cost, int evaluateValue)
        : base(entryBlock, oldStatus, newStatus, cost)
    {
        this.evaluateValue = evaluateValue;
    }
    public override void InitTextVisual(TextMeshProUGUI textVisual)
    {
        if (!IsAnnounce)
        {
            Vector2 entryBlockPos = EntryBlock.TileNode.Index();
            int x = Mathf.RoundToInt(entryBlockPos.x);
            int y = Mathf.RoundToInt(entryBlockPos.y);
            if (NewStatus == TileStatus.SEEN)
                Text = $"- Thêm vào Open ({x}, {y}) <{evaluateValue} = {cost} + {evaluateValue - cost}>.";
            else if (NewStatus == TileStatus.VISITED)
                Text = $"- Duyệt ({x}, {y}) <{evaluateValue} = {cost} + {evaluateValue - cost}>.";
            else if (NewStatus == TileStatus.PATH || NewStatus == TileStatus.END)
                Text = $"- Di chuyển tới ({x}, {y}).";
        }
        TextVisual = textVisual;
        TextVisual.text = Text;
    }
}
