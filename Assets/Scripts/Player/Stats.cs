using NUnit.Framework;
using System.Collections.Generic;
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
        List<PossibleLevelUpStats> unusedStats = new List<PossibleLevelUpStats>();

        foreach (PossibleLevelUpStats stat in PossibleStats)
        {
            unusedStats.Add(stat);
        }

        for (int i = unusedStats.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (unusedStats[i], unusedStats[j]) = (unusedStats[j], unusedStats[i]);
        }

        for (int i = 0; i < amount; i++)
        {
            stats[i] = new Stat(unusedStats[i]);
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
