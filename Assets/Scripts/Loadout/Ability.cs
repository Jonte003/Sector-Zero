using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

#region Ability Registry
public static class AbilityRegistry
{
    public static readonly Dictionary<string, Ability> All = new()
    {
        { "Backstep",         new Backstep() },
        { "Blink",            new Blink() },
        { "Chain Lightning",  new ChainLightning() },
        { "Charge",           new Charge() },
        { "Dash",             new Dash() },
        { "Eruption",         new Eruption() },
        { "Explosion",        new Explosion() },
        { "Farsight",         new Farsight() },
        { "Fortify",          new Fortify() },
        { "Grenade",          new Grenade() },
        { "Ground Slam",      new GroundSlam() },
        { "Invincible",       new Invincible() },
        { "Jump",             new Jump() },
        { "Knockback",        new Knockback() },
        { "Lamp",             new Lamp() },
        { "Leap",             new Leap() },
        { "Momentum Shift",   new MomentumShift() },
        { "Vital Surge",      new VitalSurge() },
    };

    public static Sprite GetIcon(string abilityName)
    {
        if (All.TryGetValue(abilityName, out Ability ability))
            return ability.Icon;

        Debug.LogWarning($"Ability '{abilityName}' not found in registry.");
        return null;
    }

    public static Material GetMaterial(string abilityName)
    {
        Sprite icon = GetIcon(abilityName);
        if (icon == null)
            return null;
        Material mat = new Material(Shader.Find("Transparent/Diffuse"));
        mat.mainTexture = icon.texture;
        return mat;
    }
}
#endregion
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

    protected abstract IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs);

    public void Run(GameObject player, List<Transform> enemies, MonoBehaviour runner, List<GameObject> abilityPrefabs)
    {
        if (CurrentCD > 0)
            return;

        Debug.Log(Name + " used");

        Debug.Log(enemies.Count);

        runner.StartCoroutine(AbilityRoutine(player, enemies, abilityPrefabs));

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
    public override bool NotYetImplemented => false;
    public override string Name => "Explosion";
    public override Sprite Icon => Resources.Load<Sprite>("Explosion");
    public override float CD => 14f;
    protected override float CooldownPerLevel => 2.25f;
    public override string Description => "Deals damage and stuns all nearby enemies in a short radius";

    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float baseRange = 5f;
        float rangePerLevel = 2f;

        float baseDamage = 30f;
        float damagePerLevel = 15f;

        float stunDuration = 0.6f;
        float stunDurationPerLevel = 0.15f;

        float explosionForce = 50f;

        player.GetComponent<ParticleManager>().PlayExplosionEffect(player.transform.position, Level * 0.3f + 0.7f);

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
    public override bool NotYetImplemented => true;
    public override string Name => "Knockback";
    public override Sprite Icon => Resources.Load<Sprite>("Knockback");
    public override float CD => 24f;
    protected override float CooldownPerLevel => 4.25f;
    public override string Description => "Knockbacks all nearby enemies in a big radius and slows them for a duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float baseRange = 6f;
        float rangePerLevel = 2f;

        float baseSlow = 25f;
        float slowPerLevel = 7.5f;

        float slowDuration = 1f;

        float baseKbForce = 100f;
        float kbForcePerLevel = 30f;

        player.GetComponent<ParticleManager>().PlayRippleEffect(player.transform.position, Level);

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<EnemyAgentAI>().ApplyKnockback();

                enemies[i].GetComponent<Rigidbody>().AddForce(((enemies[i].transform.position - player.transform.position).normalized + Vector3.up).normalized * (baseKbForce + kbForcePerLevel * (Level - 1)));

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
    public override float CD => 12f;
    protected override float CooldownPerLevel => 2.5f;
    public override string Description => "Dash forwards";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        player.GetComponent<ParticleManager>().PlaySpeedLines(0.6f);

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
    public override float CD => 18f;
    protected override float CooldownPerLevel => 3.25f;
    public override string Description => "Big jump forwards";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float leapForce = 25;

        player.GetComponent<Rigidbody>().AddForce((player.transform.up * 0.5f + player.transform.forward).normalized * leapForce, ForceMode.Impulse);

        yield return null;
    }
}

public class Jump : Ability
{
    public Jump() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Jump";
    public override Sprite Icon => Resources.Load<Sprite>("Jump");
    public override float CD => 14f;
    protected override float CooldownPerLevel => 2.75f;
    public override string Description => "Big jump";
    public override AbilityCategory Category => AbilityCategory.Mobility;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float jumpForce = 17.5f;

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
    public override float CD => 45f;
    protected override float CooldownPerLevel => 7.5f;
    public override string Description => "Gives you defense for the duration and regenerates a percentage of your max hp over the duration";
    public override AbilityCategory Category => AbilityCategory.Defense;


    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float baseDefense = 50f;
        float defensePerLevel = 7.5f;

        float duration = 4f;
        float durationPerLevel = 0.75f;

        float MaxHpRegen = 8f;
        float MaxHpRegenPerLevel = 2.5f;

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
    public override float CD => 40f;
    protected override float CooldownPerLevel => 7.5f;
    public override string Description => "Become untargetable for a short duration";
    public override AbilityCategory Category => AbilityCategory.Defense;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
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
    public override bool NotYetImplemented => false;
    public override string Name => "Chain Lightning";
    public override Sprite Icon => Resources.Load<Sprite>("ChainLightning");
    public override float CD => 18f;
    protected override float CooldownPerLevel => 3.25f;
    public override string Description => "Strikes an enemy with lightning that chains to others.";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float baseDamage = 20f;
        float damagePerLevel = 5f;
        float falloff = 0.7f;
        int maxJumps = 3 + (int)math.floor((Level - 1f) * 0.5f);

        float bounceDuration = 0.5f - (Level - 1) * 0.05f;
        float range = 15f + (Level - 1) * 2.5f;

        float damage = (baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1);

        if (enemies.Count == 0)
            yield break;

        Transform current = null;
        float dist = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; i++)
        {
            float d = Vector3.Distance(player.transform.position, enemies[i].position);
            if (d < dist && d <= range)
            {
                current = enemies[i];
                dist = d;
            }
        }

        if (current == null)
            yield break;

        GameObject lightning = ChainLightningScript.Spawn(
            abilityPrefabs.First(o => o.name == "ChainLightning")
        );

        lightning.transform.position = player.transform.position;
        lightning.GetComponent<TrailRenderer>().Clear();

        for (int i = 0; i < maxJumps; i++)
        {
            if (current == null)
                break;

            current.GetComponent<EnemyStats>().DoDamageToEnemy(damage);

            Vector3 startPos = lightning.transform.position + Vector3.up * 0.75f;
            Vector3 endPos = current.position;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / bounceDuration;
                lightning.transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            Transform next = null;
            float bestDist = range;

            foreach (var e in enemies)
            {
                if (e == current) continue;

                float d = Vector3.Distance(current.position, e.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    next = e;
                }
            }

            current = next;
            damage *= falloff;
        }

        lightning.GetComponent<ChainLightningScript>().DestroyLightning();
    }
}

public class Eruption : Ability
{
    public Eruption() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Eruption";
    public override Sprite Icon => Resources.Load<Sprite>("Eruption");
    public override float CD => 22f;
    protected override float CooldownPerLevel => 3.75f;
    public override string Description => "After a short delay, erupts the ground dealing heavy AOE damage.";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float delay = 1.5f;
        float baseRange = 6f;
        float rangePerLevel = 1f;

        float baseDamage = 50f;
        float damagePerLevel = 20f;

        Physics.Raycast(player.transform.position, player.transform.Find("Camera").forward, out RaycastHit ray, 50, 72);

        Vector3 position = ray.point;
        player.GetComponent<ParticleManager>().PlayEruptionEffect(Level * 0.3f + 0.7f, position, delay);


        yield return new WaitForSeconds(delay);

        float radius = baseRange + rangePerLevel * (Level - 1);
        float damage = (baseDamage + damagePerLevel * (Level - 1)) * (player.GetComponent<PlayerStats>().damageBuffs + 1);

        foreach (var e in enemies)
        {
            if (Vector3.Distance(position, e.transform.position) <= radius)
                e.GetComponent<EnemyStats>().DoDamageToEnemy(damage);
        }
    }
}

public class Blink : Ability
{
    public Blink() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Blink";
    public override Sprite Icon => Resources.Load<Sprite>("Blink");
    public override float CD => 16f;
    protected override float CooldownPerLevel => 3f;
    public override string Description => "Instantly teleport a short distance forward.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float baseDistance = 8f;
        float distancePerLevel = 2f;

        float dist = baseDistance + distancePerLevel * (Level - 1);
        
        if (Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, dist, 8))
        {
            player.transform.position = hit.point + (-player.transform.forward * 0.5f);
        }
        else
        {
            player.transform.position += player.transform.forward * dist;
        }

        yield return null;
    }
}

public class Charge : Ability
{
    public Charge() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Charge";
    public override Sprite Icon => Resources.Load<Sprite>("Charge");
    public override float CD => 20f;
    protected override float CooldownPerLevel => 3.75f;
    public override string Description => "Charge forward, pushing enemies aside.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
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
            player.GetComponent<ParticleManager>().PlaySpeedLines(force * 0.01f);

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
    public override bool NotYetImplemented => false;
    public override string Name => "Vital Surge";
    public override Sprite Icon => Resources.Load<Sprite>("VitalSurge");
    public override float CD => 40f;
    protected override float CooldownPerLevel => 6f;
    public override string Description => "Rapidly regenerate health for a short duration";
    public override AbilityCategory Category => AbilityCategory.Defense;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float baseRegenPercent = 10f;
        float regenPerLevel = 5f;

        float duration = 2f - 0.125f * (Level - 1);

        float regenTranslated = (baseRegenPercent + regenPerLevel * (Level - 1)) / duration;

        var stats = player.GetComponent<PlayerStats>();
        stats.regenBuffs += regenTranslated;

        player.GetComponent<ParticleManager>().PlayHealthEffect(duration * 0.5f);
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
    public override float CD => 16f;
    protected override float CooldownPerLevel => 3f;
    public override string Description => "Quickly dash backwards to evade attacks";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float force = 25f + 4f * (Level - 1);

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
    public override float CD => 22f;
    protected override float CooldownPerLevel => 4f;
    public override string Description => "Redirect your momentum toward your aim direction.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        var rb = player.GetComponent<Rigidbody>();

        float speed = rb.linearVelocity.magnitude;
        Vector3 newDir = player.transform.Find("Camera").forward;

        rb.linearVelocity = newDir * speed;

        yield return null;
    }
}

public class GroundSlam : Ability
{
    public GroundSlam() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Ground Slam";
    public override Sprite Icon => Resources.Load<Sprite>("GroundSlam");
    public override float CD => 20f;
    protected override float CooldownPerLevel => 3.5f;
    public override string Description => "Slam straight downward and launch nearby enemies upward and away.";
    public override AbilityCategory Category => AbilityCategory.Mobility;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
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

public class Grenade : Ability
{
    public Grenade() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Grenade";
    public override Sprite Icon => Resources.Load<Sprite>("Grenade");
    public override float CD => 20f;
    protected override float CooldownPerLevel => 3.5f;
    public override string Description => "Throw a grenade that damages enemies";
    public override AbilityCategory Category => AbilityCategory.Attack;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float radius = 5 + 1.25f * (Level - 1);

        float damage = 10 + 2.5f * (Level - 1);

        float throwForce = 500;

        Vector3 dir = player.transform.Find("Camera").forward;

        GrenadeScript.Spawn(abilityPrefabs.Where(o => o.name == "Grenade").ToArray()[0], radius, damage, throwForce, dir, player.transform.position, player.GetComponent<Collider>());

        yield return null;
    }
}

public class Lamp : Ability
{
    public Lamp() { }
    public override bool NotYetImplemented => true;
    public override string Name => "Lamp";
    public override Sprite Icon => Resources.Load<Sprite>("Lamp");
    public override float CD => 20f;
    protected override float CooldownPerLevel => 3.5f;
    public override string Description => "Throw a lamp to light up the surroundings";
    public override AbilityCategory Category => AbilityCategory.Vision;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float radius = 5 + 1.25f * (Level - 1);

        float throwForce = 500;

        Vector3 dir = player.transform.Find("Camera").forward;

        LampScript.Spawn(abilityPrefabs.Where(o => o.name == "Lamp").ToArray()[0], radius, throwForce, dir, player.transform.position, player.GetComponent<Collider>());

        yield return null;
    }
}

public class Farsight : Ability
{
    public Farsight() { }
    public override bool NotYetImplemented => false;
    public override string Name => "Farsight";
    public override Sprite Icon => Resources.Load<Sprite>("Farsight");
    public override float CD => 30f;
    protected override float CooldownPerLevel => 5f;
    public override string Description => "Increase your vision range";
    public override AbilityCategory Category => AbilityCategory.Vision;

    protected override IEnumerator AbilityRoutine(GameObject player, List<Transform> enemies, List<GameObject> abilityPrefabs)
    {
        float multiplier = 1.5f + 0.125f * (Level - 1);

        float duration = 3 + 1 * (Level - 1);

        player.GetComponent<PlayerVision>().UpdateVisionRange(player.GetComponent<PlayerStats>().VisionRange * multiplier);

        yield return new WaitForSeconds(duration);

        player.GetComponent<PlayerVision>().UpdateVisionRange(player.GetComponent<PlayerStats>().VisionRange / multiplier);
    }
}