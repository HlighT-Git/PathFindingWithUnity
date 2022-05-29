public class InformStep : Step
{
    private int currentCost;
    private int evaluateValue;

    public InformStep(StepType type , TileBlock curBlock, TileBlock targetBlock, int currentCost, int evaluateValue)
        : base(type, curBlock, targetBlock)
    {
        this.currentCost = currentCost;
        this.evaluateValue = evaluateValue;
    }
    public int CurrentCost { get => currentCost; set => currentCost = value; }
    public int EvaluateValue { get => evaluateValue; set => evaluateValue = value; }
}
