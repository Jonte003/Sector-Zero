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
    public abstract string Description { get; }

    public abstract AbilityCategory Category { get; }

    public abstract Sprite Icon { get; }
    public abstract string Name { get; }
    public bool Enabled { get; set; } = false;
}

public enum AbilityCategory
{
    Attack,
    Defense,
    Mobility
}

public class Explosion : Ability
{
    public override string Name => "Explosion";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 7f;
    protected override float CooldownPerLevel => 0.5f;
    public override string Description => "Deals damage and stuns all nearby enemies in a short radius";

    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseRange = 2f;
        float rangePerLevel = 1f;

        float baseDamage = 30f;
        float damagePerLevel = 15f;

        float stunDuration = 0.6f;
        float stunDurationPerLevel = 0.15f;

        float explosionForce = 5f;

        for (int i = 0;  i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<EnemyStats>().DoDamageToEnemy((baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1));

                enemies[i].GetComponent<Rigidbody>().AddForce(enemies[i].transform.up * explosionForce, ForceMode.Impulse);
                enemies[i].GetComponent<EnemyAI>().Stun(stunDuration + (stunDurationPerLevel * (Level - 1)));
            }
        }

        yield return null;
    }
}

public class Knockback : Ability
{
    public override string Name => "Knockback";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 12f;
    protected override float CooldownPerLevel => 1.25f;
    public override string Description => "Knockbacks all nearby enemies in a big radius and slows them for a duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


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
                
                enemies[i].GetComponent<EnemyAI>().Slow(slowDuration, 1 - (baseSlow + slowPerLevel * (Level - 1)) / 100);
            }
        }

        yield return null;
    }
}

public class Dash : Ability
{
    public override string Name => "Dash";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 6f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Dash forwards";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float dashForce = 10;

        player.GetComponent<Rigidbody>().AddForce(player.transform.forward * dashForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Leap : Ability
{
    public override string Name => "Leap";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 9f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Big jump forwards";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float leapForce = 10;

        player.GetComponent<Rigidbody>().AddForce((player.transform.up + player.transform.forward).normalized * leapForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Jump : Ability
{
    public override string Name => "Jump";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 7f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Big jump";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float jumpForce = 15;

        player.GetComponent<Rigidbody>().AddForce(player.transform.up * jumpForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Fortify : Ability
{
    public override string Name => "Fortify";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 30f;
    protected override float CooldownPerLevel => 3f;
    public override string Description => "Gives you defense for the duration and regenerates a percentage of your max hp over the duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


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

public class Invincible : Ability
{
    public override string Name => "Invincible";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 20f;
    protected override float CooldownPerLevel => 2.25f;
    public override string Description => "Become untargetable for a short duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float duration = 2.5f;
        float durationPerLevel = 0.25f;

        player.GetComponent<PlayerStats>().invincible = true;

        yield return new WaitForSeconds(duration + durationPerLevel * (Level - 1));

        player.GetComponent<PlayerStats>().invincible = false;
    }
}


public class ChainLightning : Ability
{
    public override string Name => "Chain Lightning";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 9f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Strikes an enemy with lightning that chains to others.";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        if (enemies.Count == 0)
            yield break;

        float baseDamage = 35f;
        float damagePerLevel = 12f;
        float falloff = 0.8f;
        int maxJumps = 3 + (Level - 1);

        float damage = (baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1);

        GameObject current = enemies[0];

        for (int i = 0; i < maxJumps; i++)
        {
            if (current == null)
                break;

            current.GetComponent<EnemyStats>().DoDamageToEnemy(damage);

            GameObject next = null;
            float bestDist = Mathf.Infinity;

            foreach (var e in enemies)
            {
                if (e == current) continue;

                float d = Vector3.Distance(current.transform.position, e.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    next = e;
                }
            }

            current = next;
            damage *= falloff;
        }

        yield return null;
    }
}

public class Eruption : Ability
{
    public override string Name => "Eruption";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 11f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "After a short delay, erupts the ground dealing heavy AOE damage.";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float delay = 1.5f;
        float baseRange = 3f;
        float rangePerLevel = 0.5f;

        float baseDamage = 50f;
        float damagePerLevel = 20f;

        yield return new WaitForSeconds(delay);

        float radius = baseRange + rangePerLevel * (Level - 1);
        float damage = (baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1);

        foreach (var e in enemies)
        {
            if (Vector3.Distance(player.transform.position, e.transform.position) <= radius)
                e.GetComponent<EnemyStats>().DoDamageToEnemy(damage);
        }
    }
}

public class Blink : Ability
{
    public override string Name => "Blink";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 8f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Instantly teleport a short distance forward.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseDistance = 4f;
        float distancePerLevel = 1f;

        float dist = baseDistance + distancePerLevel * (Level - 1);

        player.transform.position += player.transform.forward * dist;

        yield return null;
    }
}

public class Charge : Ability
{
    public override string Name => "Charge";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 10f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Charge forward, pushing enemies aside.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseForce = 14f;
        float forcePerLevel = 2f;

        float kbForce = 6f;

        float force = baseForce + forcePerLevel * (Level - 1);

        var rb = player.GetComponent<Rigidbody>();
        rb.AddForce(player.transform.forward * force, ForceMode.Impulse);

        yield return new WaitForFixedUpdate();


        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(player.transform.position, e.transform.position);
            if (dist <= 2f)
            {
                Vector3 dir = (e.transform.position - player.transform.position).normalized;
                e.GetComponent<Rigidbody>().AddForce(dir * kbForce, ForceMode.Impulse);
            }
        }

        yield return null;
    }
}

public class VitalSurge : Ability
{
    public override string Name => "Vital Surge";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 14f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Rapidly regenerate health for a short duration";
    public override AbilityCategory Category => AbilityCategory.Defense;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float baseRegenPercent = 12f;
        float regenPerLevel = 3f;

        float duration = 2.5f + 0.25f * (Level - 1);

        float regenPerSecond = (baseRegenPercent + regenPerLevel * (Level - 1)) / (duration * 100f);

        var stats = player.GetComponent<PlayerStats>();
        stats.regenBuffs += regenPerSecond;

        yield return new WaitForSeconds(duration);

        stats.regenBuffs -= regenPerSecond;
    }
}

public class Backstep : Ability
{
    public override string Name => "Backstep";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 5f;
    protected override float CooldownPerLevel => 0.75f;
    public override string Description => "Quickly dash backwards to evade attacks";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        float force = 12f + 2f * (Level - 1);

        var rb = player.GetComponent<Rigidbody>();
        rb.AddForce(-player.transform.forward * force, ForceMode.Impulse);

        yield return null;
    }
}

public class MomentumShift : Ability
{
    public override string Name => "Momentum Shift";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 7f;
    protected override float CooldownPerLevel => 0.75f;
    public override string Description => "Redirect your momentum toward your aim direction.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        var rb = player.GetComponent<Rigidbody>();

        float speed = rb.linearVelocity.magnitude;
        Vector3 newDir = player.transform.forward;

        rb.linearVelocity = newDir * speed;

        yield return null;
    }
}

public class GroundSlam : Ability
{
   public override string Name => "Ground Slam";
    public override Sprite Icon => Resources.Load<Sprite>("Default");
    protected override float CD => 10f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Slam straight downward and launch nearby enemies upward and away.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;

        float baseForce = 25f;
        float forcePerLevel = 4f;
        float slamForce = baseForce + forcePerLevel * (Level - 1);

        rb.AddForce(Vector3.down * slamForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);

        float radius = 3f + 0.25f * (Level - 1);
        float upwardForce = 10f + 2f * (Level - 1);
        float outwardForce = 6f;

        foreach (var e in enemies)
        {
            if (Vector3.Distance(player.transform.position, e.transform.position) <= radius)
            {
                Rigidbody er = e.GetComponent<Rigidbody>();

                er.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

                Vector3 dir = (e.transform.position - player.transform.position).normalized;
                er.AddForce(dir * outwardForce, ForceMode.Impulse);
            }
        }
    }
}
