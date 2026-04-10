using UnityEngine;

public static class Stats
{
    private static readonly PossibleLevelUpStats[] PossibleStats =
    {
        PossibleLevelUpStats.Hp,
        PossibleLevelUpStats.Regen,
        PossibleLevelUpStats.Damage,
        PossibleLevelUpStats.AbilityHaste,
        PossibleLevelUpStats.Defense,
        PossibleLevelUpStats.JumpHeight,
        PossibleLevelUpStats.MovementSpeed
    };

    public static Stat[] GetRandomStats(int amount)
    {
        Stat[] stats = new Stat[amount];
        for (int i = 0; i < amount; i++)
        {
            stats[i] = new Stat(PossibleStats[Random.Range(0, PossibleStats.Length)]);
            if (i > 0)
            {
                if (stats[i].StatType == stats[i - 1].StatType)
                {
                    i--;
                }
            }
        }
        return stats;
    }
}

public enum PossibleLevelUpStats
{
    Hp,
    Regen,
    Damage,
    AbilityHaste,
    Defense,
    JumpHeight,
    MovementSpeed
}

public class Stat
{
    public PossibleLevelUpStats StatType { get; }
    public float Value { get; }

    private float BaseValue =>
        StatType switch
        {
            PossibleLevelUpStats.Hp => 10f,
            PossibleLevelUpStats.Regen => 1f,
            PossibleLevelUpStats.Damage => 10f,
            PossibleLevelUpStats.AbilityHaste => 10f,
            PossibleLevelUpStats.Defense => 10f,
            PossibleLevelUpStats.JumpHeight => 15f,
            PossibleLevelUpStats.MovementSpeed => 10f,
            _ => 0f
        };

    public Stat(PossibleLevelUpStats statType)
    {
        StatType = statType;

        float multiplier = (5 + Random.Range(0, 5)) * 0.1f;
        Value = multiplier * BaseValue;
    }
}
