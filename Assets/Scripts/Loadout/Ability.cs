public abstract class Ability
{
    public abstract void Run();
    public float CurrentCD { get; set; }
    protected abstract float CD { get; }
}

public class ExampleAbility : Ability
{
    protected override float CD => 5f;
    public override void Run()
    {
        if (CurrentCD > 0)
            return;

        // Run

        CurrentCD = CD;
    }
}