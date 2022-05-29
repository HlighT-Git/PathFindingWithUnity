using UnityEngine;
using TMPro;

public enum StepType
{
    SEE,
    VISIT,
    SEEAGAIN,
    MOVE
}
public class Step
{
    private StepType type;
    private TileBlock entryBlock;
    private string text;
    public Step(StepType type, TileBlock entryBlock)
    {
        this.type = type;
        this.entryBlock = entryBlock;
        text = string.Empty;
        InitText();
    }
    protected virtual void InitText()
    {
        Vector2 entryBlockPos = entryBlock.Node.Index();
        int x = Mathf.RoundToInt(entryBlockPos.x);
        int y = Mathf.RoundToInt(entryBlockPos.y);
        switch (type)
        {
            case StepType.SEE:
                text = $"- Thêm vào Open nút ({x}, {y}).";
                break;
            case StepType.VISIT:
                text = $"- Duyệt nút ({x}, {y}).";
                break;
            case StepType.MOVE:
                text = $"- Di chuyển tới ({x}, {y}).";
                break;
        }
    }
    public StepType Type { get => type; }
    public TileBlock EntryBlock { get => entryBlock; set => entryBlock = value; }
    public string Text { get => text; set => text = value; }
}
