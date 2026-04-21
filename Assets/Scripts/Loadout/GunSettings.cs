using UnityEngine;

public class GunSettings
{
    public string GunName;

    // Normal
    [Tooltip("Damage per bullet"), Header("Normal Stats")] public float Damage;
    [Tooltip("Amount of bullets fired per second")] public float FireRate;
    [Tooltip("The Range you can shoot in unity units")] public float Range;
    [Tooltip("The amount of seconds it takes to reload")] public float ReloadSpeed;
    [Tooltip("The amount of seconds it takes to reload")] public int MaxAmmo;
    [Tooltip("If you can hold to shoot multiple bullets")] public bool FullAuto;
    [Tooltip("Movement speed modifier while holding the weapon")] public float MoveSpeed;

    // Damage Falloff
    [Tooltip("Percentage of normal damage connected to the set range"), Header("Damage Falloff")] public float[] DamageFalloffPercentage;
    [Tooltip("Range where damage falloff will change connected to the percentage")] public float[] DamageFalloffRange;
    [Tooltip("If damage falloff will lerp or not")] public bool DamageFalloffLerp;
    [Tooltip("Pierce falloff per enemy pierced")] public float PierceFalloff;

    // Multiple Bullets
    [Tooltip("Delay between each bullet in seconds"), Header("Multiple Bullets")] public float DelayBetweenBullets;
    [Tooltip("Amount of bullets fired per shot")] public int BulletCount;
    [Tooltip("Ammo consumed per shot")] public int AmmoPerShot;
    [Tooltip("Minimum amount of spread")] public Vector2 MinSpread;
    [Tooltip("Maximum amount of spread")] public Vector2 MaxSpread;

    // Recoil
    [Tooltip("How much recoil shooting will cause"), Header("Recoil")] public Vector2 RecoilMagnitude;
    [Tooltip("Minimum amount of recoil shooting will cause, has to be lower than Recoil Max"), Range(0, 1)] public float RecoilMin;
    [Tooltip("Maximum amount of recoil shooting will cause, has to be higher than Recoil Min"), Range(0, 1)] public float RecoilMax;

    // Bullet Tracers
    [Tooltip("How many tracers can be active at a time"), Header("Bullet Tracers")] public int TracerPoolSize = 30;
    [Tooltip("Where the tracers spawn")] public Transform Muzzle;

    private GunSettings(
        string gunName,
        float damage, float fireRate, float range, float reloadSpeed, int maxAmmo, bool fullAuto, float moveSpeed, 
        float[] damageFalloffPercentage, float[] damageFalloffRange, bool damageFalloffLerp, float pierceFalloff,
        float delayBetweenBullets, int bulletCount, int ammoPerShot, Vector2 minSpread, Vector2 maxSpread,
        Vector2 recoilMagnitude, float recoilMin, float recoilMax,
        int tracerPoolSize)
    {
        GunName = gunName;

        Damage = damage;
        FireRate = fireRate;
        Range = range;
        ReloadSpeed = reloadSpeed;
        MaxAmmo = maxAmmo;
        FullAuto = fullAuto;
        MoveSpeed = moveSpeed;

        DamageFalloffPercentage = damageFalloffPercentage;
        DamageFalloffRange = damageFalloffRange;
        DamageFalloffLerp = damageFalloffLerp;

        DelayBetweenBullets = delayBetweenBullets;
        BulletCount = bulletCount;
        AmmoPerShot = ammoPerShot;
        MinSpread = minSpread;
        MaxSpread = maxSpread;

        RecoilMagnitude = recoilMagnitude;
        RecoilMin = recoilMin;
        RecoilMax = recoilMax;

        TracerPoolSize = tracerPoolSize;
        PierceFalloff = pierceFalloff;
    }

    public static GunSettings AssaultRifle = new
        (
            "Assault Rifle",
            7f, 4f, 150f, 2.5f, 35, true, 1f,
            new float[] { 0.85f, 0.7f }, new float[] { 75f, 100f }, false, 60f,
            0, 1, 1, new(-1.5f, -1.5f), new(1.5f, 1.5f),
            new(1, 2), 0.5f, 1f,
            30
        );

    public static GunSettings BurstRifle => new
        (
            "Burst Rifle",
            10f, 1.5f, 150f, 2.5f, 30, true, 0.95f,
            new float[] { 0.85f, 0.7f }, new float[] { 75f, 100f }, false, 65f,
            0.1f, 3, 3, new(-1f, -1f), new(1f, 1f),
            new(1, 2), 0.5f, 1f,
            30
        );

    public static GunSettings Shotgun => new
        (
            "Shotgun",
            3.5f, 1.5f, 40f, 2f, 8, true, 0.85f,
            new float[] { 0.8f, 0.5f }, new float[] { 20f, 30f }, true, 75f,
            0, 8, 1, new(-4f, -4.5f), new(4f, 4f),
            new(1, 2), 0.5f, 1f,
            30
        );

    public static GunSettings Pistol => new
        (
            "Pistol",
            10f, 3.5f, 125f, 1.2f, 12, true, 1.25f,
            new float[] { 0.8f, 0.65f, 0.5f }, new float[] { 75f, 100f, 125f }, true, 80f,
            0, 1, 1, new(-0.75f, -0.75f), new(0.75f, 0.75f),
            new(0.75f, 3), 0.5f, 1f,
            30
        );

    public static GunSettings Smg => new
        (
            "Smg",
            4f, 8f, 100f, 0.7f, 20, true, 1.15f,
            new float[] { 0.9f, 0.75f, 0.5f }, new float[] { 50f, 75f, 100f }, true, 85f,
            0, 1, 1, new(-1.25f, -1.25f), new(1.25f, 1.25f),
            new(1.5f, 2f), 0.75f, 1f,
            30
        );
}