using UnityEngine;
using TMPro;
public class Step
{
    private TileBlock entryBlock;
    private Color oldStatus;
    private Color newStatus;
    private string text;
    private TextMeshProUGUI textVisual;
    private bool isAnnounce = false;
    public Step(string text)
    {
        this.text = text;
        isAnnounce = true;
    }
    public Step(TileBlock entryBlock, Color oldStatus, Color newStatus)
    {
        this.entryBlock = entryBlock;
        this.oldStatus = oldStatus;
        this.newStatus = newStatus;
    }
    public TileBlock EntryBlock { get => entryBlock; set => entryBlock = value; }
    public Color OldStatus { get => oldStatus; set => oldStatus = value; }
    public Color NewStatus { get => newStatus; set => newStatus = value; }
    public string Text { get => text; set => text = value; }
    public bool IsAnnounce { get => isAnnounce; set => isAnnounce = value; }
    public TextMeshProUGUI TextVisual { get => textVisual; set => textVisual = value; }

    public virtual void InitTextVisual(TextMeshProUGUI textVisual)
    {
        if (!IsAnnounce)
        {
            Vector2 entryBlockPos = entryBlock.TileNode.Index();
            int x = Mathf.RoundToInt(entryBlockPos.x);
            int y = Mathf.RoundToInt(entryBlockPos.y);
            if (newStatus == TileStatus.SEEN)
                text = $"- Thêm vào Open nút ({x}, {y}).";
            else if (newStatus == TileStatus.VISITED)
                text = $"- Duyệt nút ({x}, {y}).";
            else if (newStatus == TileStatus.PATH || newStatus == TileStatus.END)
                text = $"- Di chuyển tới ({x}, {y}).";
        }
        this.textVisual = textVisual;
        textVisual.text = text;
    }
}
