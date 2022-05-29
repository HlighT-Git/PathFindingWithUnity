public class InformStep : Step
{
    private int currentCost;
    private int evaluateValue;

    public InformStep(StepType type, TileBlock entryBlock, int currentCost, int evaluateValue)
        : base(type, entryBlock)
    {
        this.currentCost = currentCost;
        this.evaluateValue = evaluateValue;
    }
    public int CurrentCost { get => currentCost; set => currentCost = value; }
    public int EvaluateValue { get => evaluateValue; set => evaluateValue = value; }
}
