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
        PossibleLevelUpStats.MovementSpeed,
        PossibleLevelUpStats.VisionRange
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
    MovementSpeed,
    VisionRange
}

public enum StatRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Stat
{
    public PossibleLevelUpStats StatType { get; }
    public float Value { get; }
    public StatRarity Rarity { get; }

    private float BaseValue =>
        StatType switch
        {
            PossibleLevelUpStats.Hp => 10f,
            PossibleLevelUpStats.Regen => 1f,
            PossibleLevelUpStats.Damage => 10f,
            PossibleLevelUpStats.AbilityHaste => 15f,
            PossibleLevelUpStats.Defense => 10f,
            PossibleLevelUpStats.JumpHeight => 20f,
            PossibleLevelUpStats.MovementSpeed => 15f,
            PossibleLevelUpStats.VisionRange => 20f,
            _ => 0f
        };

    public Stat(PossibleLevelUpStats statType)
    {
        StatType = statType;

        float key = Random.value;

        StatRarity rarity = key switch
        {
            < 0.5f => StatRarity.Common,
            < 0.75f => StatRarity.Uncommon,
            < 0.9f => StatRarity.Rare,
            < 0.975f => StatRarity.Epic,
            _ => StatRarity.Legendary
        };

        Rarity = rarity;

        float multiplier = rarity switch
        {
            StatRarity.Common => 1,
            StatRarity.Uncommon => 1.3f,
            StatRarity.Rare => 1.75f,
            StatRarity.Epic => 2.25f,
            StatRarity.Legendary => 3f,
            _ => 0f
        };

        Value = multiplier * BaseValue;
    }
}
