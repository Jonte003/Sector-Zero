using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public float CurrentCD { get; set; }
    public int Level { get; set; } = 1;

    public abstract float CD { get; }
    protected abstract float CooldownPerLevel { get; }
    public abstract string Description { get; }
    public abstract bool NotYetImplemented { get; }
    public abstract AbilityCategory Category { get; }
    public abstract Sprite Icon { get; }
    public abstract string Name { get; }
    public bool Enabled { get; set; } = false;

    protected abstract IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies);

    public void Run(GameObject player, List<Transform> enemies, MonoBehaviour runner)
    {
        if (CurrentCD > 0)
            return;

        Debug.Log(Name + " used");

        Debug.Log(enemies.Count);

        runner.StartCoroutine(AbilityRoutine(player, enemies));

        CurrentCD = (CD - CooldownPerLevel * (Level - 1)) *
                    (100 / (100 + player.GetComponent<PlayerStats>().abilityHasteBuffs));
    }
}

public enum AbilityCategory
{
    Attack,
    Defense,
    Mobility,
    Vision
}

public class Explosion : Ability
{
    public Explosion() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Explosion";
    public override Sprite Icon => Resources.Load<Sprite>("Explosion");
    public override float CD => 7f;
    protected override float CooldownPerLevel => 0.5f;
    public override string Description => "Deals damage and stuns all nearby enemies in a short radius";

    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float baseRange = 5f;
        float rangePerLevel = 2f;

        float baseDamage = 30f;
        float damagePerLevel = 15f;

        float stunDuration = 0.6f;
        float stunDurationPerLevel = 0.15f;

        float explosionForce = 50f;

        for (int i = 0;  i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<EnemyStats>().DoDamageToEnemy((baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1));

                enemies[i].GetComponent<EnemyAgentAI>().ApplyKnockback();

                enemies[i].GetComponent<Rigidbody>().AddForce(enemies[i].transform.up * explosionForce);

                enemies[i].GetComponent<EnemyAgentAI>().Stun(stunDuration + (stunDurationPerLevel * (Level - 1)));
            }
        }

        yield return null;
    }
}

public class Knockback : Ability
{
    public Knockback() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Knockback";
    public override Sprite Icon => Resources.Load<Sprite>("Knockback");
    public override float CD => 12f;
    protected override float CooldownPerLevel => 1.25f;
    public override string Description => "Knockbacks all nearby enemies in a big radius and slows them for a duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float baseRange = 6f;
        float rangePerLevel = 2f;

        float baseSlow = 25f;
        float slowPerLevel = 7.5f;

        float slowDuration = 1f;

        float baseKbForce = 100f;
        float kbForcePerLevel = 30f;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<EnemyAgentAI>().ApplyKnockback();

                enemies[i].GetComponent<Rigidbody>().AddForce(((enemies[i].transform.position - player.transform.position).normalized + Vector3.up * 0.5f).normalized * (baseKbForce + kbForcePerLevel * (Level - 1)));

                enemies[i].GetComponent<EnemyAgentAI>().Slow(slowDuration, 1 - (baseSlow + slowPerLevel * (Level - 1)) / 100);
            }
        }

        yield return null;
    }
}

public class Dash : Ability
{
    public Dash() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Dash";
    public override Sprite Icon => Resources.Load<Sprite>("Dash");
    public override float CD => 6f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Dash forwards";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float dashForce = 20f;

        player.GetComponent<Rigidbody>().AddForce(player.transform.forward * dashForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Leap : Ability
{
    public Leap() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Leap";
    public override Sprite Icon => Resources.Load<Sprite>("Leap");
    public override float CD => 9f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Big jump forwards";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float leapForce = 15;

        player.GetComponent<Rigidbody>().AddForce((player.transform.up + player.transform.forward).normalized * leapForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Jump : Ability
{
    public Jump() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Jump";
    public override Sprite Icon => Resources.Load<Sprite>("Jump");
    public override float CD => 7f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Big jump";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float jumpForce = 15;

        player.GetComponent<Rigidbody>().AddForce(player.transform.up * jumpForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Fortify : Ability
{
    public Fortify() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Fortify";
    public override Sprite Icon => Resources.Load<Sprite>("Fortify");
    public override float CD => 30f;
    protected override float CooldownPerLevel => 3f;
    public override string Description => "Gives you defense for the duration and regenerates a percentage of your max hp over the duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float baseDefense = 30f;
        float defensePerLevel = 5f;

        float duration = 4f;
        float durationPerLevel = 0.75f;

        float MaxHpRegen = 4f;
        float MaxHpRegenPerLevel = 1.5f;

        float regenTranslated = (MaxHpRegen + MaxHpRegenPerLevel * (Level - 1)) / (duration + durationPerLevel * (Level - 1));

        player.GetComponent<PlayerStats>().defenseBuffs += baseDefense + defensePerLevel * (Level - 1);
        player.GetComponent<PlayerStats>().regenBuffs += regenTranslated;

        yield return new WaitForSeconds(duration + durationPerLevel * (Level - 1));

        player.GetComponent<PlayerStats>().defenseBuffs -= baseDefense + defensePerLevel * (Level - 1);
        player.GetComponent<PlayerStats>().regenBuffs -= regenTranslated;
    }
}

public class Invincible : Ability
{
    public Invincible() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Invincible";
    public override Sprite Icon => Resources.Load<Sprite>("Invincible");
    public override float CD => 20f;
    protected override float CooldownPerLevel => 2.25f;
    public override string Description => "Become untargetable for a short duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
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
    public ChainLightning() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Chain Lightning";
    public override Sprite Icon => Resources.Load<Sprite>("ChainLightning");
    public override float CD => 9f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Strikes an enemy with lightning that chains to others.";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        if (enemies.Count == 0)
            yield break;

        float baseDamage = 35f;
        float damagePerLevel = 12f;
        float falloff = 0.8f;
        int maxJumps = 3 + (Level - 1);

        float damage = (baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1);

        Transform current = enemies[0];
        float dist = Vector3.Distance(player.transform.position, current.transform.position);

        for (int i = 1; i <  enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) < dist)
            {
                current = enemies[i];
                dist = Vector3.Distance(player.transform.position, current.transform.position);
            }
        }

        for (int i = 0; i < maxJumps; i++)
        {
            if (current == null)
                break;

            current.GetComponent<EnemyStats>().DoDamageToEnemy(damage);

            Transform next = null;
            float bestDist = 20f;

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
    public Eruption() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Eruption";
    public override Sprite Icon => Resources.Load<Sprite>("Eruption");
    public override float CD => 11f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "After a short delay, erupts the ground dealing heavy AOE damage.";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float delay = 1.5f;
        float baseRange = 6f;
        float rangePerLevel = 1f;

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
    public Blink() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Blink";
    public override Sprite Icon => Resources.Load<Sprite>("Blink");
    public override float CD => 8f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Instantly teleport a short distance forward.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
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
    public Charge() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Charge";
    public override Sprite Icon => Resources.Load<Sprite>("Charge");
    public override float CD => 10f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Charge forward, pushing enemies aside.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float baseForce = 50f;
        float forcePerLevel = 10f;

        float kbForce = 50f;

        float force = baseForce + forcePerLevel * (Level - 1);

        var rb = player.GetComponent<Rigidbody>();
        rb.AddForce(player.transform.forward * force, ForceMode.Impulse);

        yield return new WaitForFixedUpdate();


        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(player.transform.position, e.transform.position);
            if (dist <= 5f)
            {
                Vector3 dir = ((e.transform.position - player.transform.position).normalized + Vector3.up * 0.5f).normalized;
                e.GetComponent<EnemyAgentAI>().ApplyKnockback();
                e.GetComponent<Rigidbody>().AddForce(dir * kbForce);
            }
        }

        yield return null;
    }
}

public class VitalSurge : Ability
{
    public VitalSurge() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Vital Surge";
    public override Sprite Icon => Resources.Load<Sprite>("VitalSurge");
    public override float CD => 20f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Rapidly regenerate health for a short duration";
    public override AbilityCategory Category => AbilityCategory.Defense;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float baseRegenPercent = 8f;
        float regenPerLevel = 2f;

        float duration = 1f;

        float regenTranslated = (baseRegenPercent + regenPerLevel * (Level - 1)) / duration;

        var stats = player.GetComponent<PlayerStats>();
        stats.regenBuffs += regenTranslated;

        yield return new WaitForSeconds(duration);

        stats.regenBuffs -= regenTranslated;
    }
}

public class Backstep : Ability
{
    public Backstep() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Backstep";
    public override Sprite Icon => Resources.Load<Sprite>("Backstep");
    public override float CD => 5f;
    protected override float CooldownPerLevel => 0.75f;
    public override string Description => "Quickly dash backwards to evade attacks";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        float force = 30f + 2f * (Level - 1);

        var rb = player.GetComponent<Rigidbody>();
        rb.AddForce(-player.transform.forward * force, ForceMode.Impulse);

        yield return null;
    }
}

public class MomentumShift : Ability
{
    public MomentumShift() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Momentum Shift";
    public override Sprite Icon => Resources.Load<Sprite>("MomentumShift");
    public override float CD => 7f;
    protected override float CooldownPerLevel => 0.75f;
    public override string Description => "Redirect your momentum toward your aim direction.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
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
    public GroundSlam() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Ground Slam";
    public override Sprite Icon => Resources.Load<Sprite>("GroundSlam");
    public override float CD => 10f;
    protected override float CooldownPerLevel => 1f;
    public override string Description => "Slam straight downward and launch nearby enemies upward and away.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;

        float baseForce = 25f;
        float forcePerLevel = 4f;
        float slamForce = baseForce + forcePerLevel * (Level - 1);

        rb.AddForce(Vector3.down * slamForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);

        float radius = 5f + 0.75f * (Level - 1);
        float upwardForce = 10f + 2f * (Level - 1);
        float outwardForce = 6f;

        foreach (var e in enemies)
        {
            if (Vector3.Distance(player.transform.position, e.transform.position) <= radius)
            {
                Rigidbody er = e.GetComponent<Rigidbody>();

                er.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

                Vector3 dir = ((e.transform.position - player.transform.position).normalized + Vector3.up * 0.5f).normalized;

                e.GetComponent<EnemyAgentAI>().ApplyKnockback();
                e.GetComponent<Rigidbody>().AddForce(dir * outwardForce);
            }
        }
    }
}
