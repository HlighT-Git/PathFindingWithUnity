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
    private TileBlock parentBlock;
    private TileBlock targetBlock;
    private TextMeshProUGUI text;
    public Step(StepType type, TileBlock curBlock, TileBlock targetBlock)
    {
        this.type = type;
        this.parentBlock = curBlock;
        this.targetBlock = targetBlock;
        text = new();
        InitText();

    }
    public virtual void InitText()
    {
        Vector2 targetBlockPos = targetBlock.Node.Position;
        int x = Mathf.RoundToInt(targetBlockPos.x);
        int y = Mathf.RoundToInt(targetBlockPos.y);
        switch (type)
        {
            case StepType.SEE:
                text.text = $"- Thêm vào Open nút ({x}, {y}).";
                break;
            case StepType.VISIT:
                text.text = $"- Duyệt nút ({x}, {y}).";
                break;
            case StepType.MOVE:
                text.text = $"- Di chuyển tới ({x}, {y}).";
                break;
        }
    }
    public StepType Type { get => type; }
    public TileBlock CurBlock { get => parentBlock; set => parentBlock = value; }
    public TileBlock TargetBlock { get => targetBlock; set => targetBlock = value; }
}
