public class StepByCost : Step
{
    private int currentCost;
    public StepByCost(StepType type, TileBlock entryBlock, int currentCost)
        : base(type, entryBlock)
    {
        this.currentCost = currentCost;
    }
    public int CurrentCost { get => currentCost; set => currentCost = value; }
}
