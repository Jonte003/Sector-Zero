using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public void Run(GameObject player, List<GameObject> enemies)
    {
        if (CurrentCD > 0)
            return;

        StartCoroutine(AbilityRoutine(player, enemies));

        CurrentCD = (CD - CooldownPerLevel * (Level - 1)) * (100 / (100 + player.GetComponent<PlayerStats>().abilityHasteBuffs));
    }

    protected virtual IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        yield return null;
    }
    public float CurrentCD { get; set; }
    protected abstract float CD { get; }
    public int Level { get; set; } = 1;
    protected abstract float CooldownPerLevel { get; }
}

public class Explosion : Ability // Deals damage and stuns all nearby enemies in a short radius
{
    protected override float CD => 7f;
    protected override float CooldownPerLevel => 0.5f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseRange = 2f;
        float rangePerLevel = 1f;

        float baseDamage = 30f;
        float damagePerLevel = 15f;

        float stunDuration = 0.6f;
        float stunDurationPerLevel = 0.15f;


        for (int i = 0;  i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<EnemyStats>().DoDamageToEnemy((baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1));
                // Knockup Enemy
            }
        }

        yield return null;
    }
}

public class Knockback : Ability // Knockbacks all nearby enemies in a big radius and slows them for a duration
{
    protected override float CD => 12f;
    protected override float CooldownPerLevel => 1.25f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseRange = 4f;
        float rangePerLevel = 1.25f;

        float baseSlow = 25f;
        float slowPerLevel = 7.5f;

        float slowDuration = 1f;

        float baseKbForce = 10f;
        float kbForcePerLevel = 3f;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<Rigidbody>().AddForce((enemies[i].transform.position - player.transform.position).normalized * (baseKbForce + kbForcePerLevel * (Level - 1)), ForceMode.Impulse);
                // Slow enemy
            }
        }

        yield return null;
    }
}

public class Dash : Ability // Dash forwards
{
    protected override float CD => 6f;
    protected override float CooldownPerLevel => 1f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float dashForce = 10;

        player.GetComponent<Rigidbody>().AddForce(player.transform.forward * dashForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Leap : Ability // Big jump forwards
{
    protected override float CD => 9f;
    protected override float CooldownPerLevel => 1f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float leapForce = 10;

        player.GetComponent<Rigidbody>().AddForce((player.transform.up + player.transform.forward).normalized * leapForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Jump : Ability // Big jump
{
    protected override float CD => 7f;
    protected override float CooldownPerLevel => 1f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float jumpForce = 15;

        player.GetComponent<Rigidbody>().AddForce(player.transform.up * jumpForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Fortify : Ability // Gives you defense for the duration and regenerates a percentage of your max hp over the duration
{
    protected override float CD => 30f;
    protected override float CooldownPerLevel => 3f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseDefense = 30f;
        float defensePerLevel = 5f;

        float duration = 4f;
        float durationPerLevel = 0.75f;

        float MaxHpRegen = 15f;
        float MaxHpRegenPerLevel = 2.5f;

        float regenTranslated = (MaxHpRegen + MaxHpRegenPerLevel * (Level - 1)) / ((duration + durationPerLevel * (Level - 1)) * 100);

        player.GetComponent<PlayerStats>().defenseBuffs += baseDefense + defensePerLevel * (Level - 1);
        player.GetComponent<PlayerStats>().regenBuffs += regenTranslated;

        yield return new WaitForSeconds(duration + durationPerLevel * (Level - 1));

        player.GetComponent<PlayerStats>().defenseBuffs -= baseDefense + defensePerLevel * (Level - 1);
        player.GetComponent<PlayerStats>().regenBuffs -= regenTranslated;
    }
}

public class Invincible : Ability // Become untargetable for a short duration
{
    protected override float CD => 20f;
    protected override float CooldownPerLevel => 2.25f;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float duration = 2.5f;
        float durationPerLevel = 0.25f;

        player.GetComponent<PlayerStats>().invincible = true;

        yield return new WaitForSeconds(duration + durationPerLevel * (Level - 1));

        player.GetComponent<PlayerStats>().invincible = false;
    }
}