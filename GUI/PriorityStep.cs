public class PriorityStep : Step
{
    private int cost;

    public PriorityStep(StepType type, TileBlock entryBlock, int cost)
        : base(type, entryBlock)
    {
        this.cost = cost;
    }

    public int Cost { get => cost; set => cost = value; }
}
