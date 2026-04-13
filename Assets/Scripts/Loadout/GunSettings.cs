using UnityEngine;

public class GunSettings : MonoBehaviour
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
        float[] damageFalloffPercentage, float[] damageFalloffRange, bool damageFalloffLerp,
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

        Muzzle = transform.Find("Body").Find("Muzzle");
    }

    public static GunSettings AssaultRifle => new
        (
            "Assault Rifle",
            7f, 4f, 150f, 2.5f, 30, true, 0.9f, 
            new float[] { 0.75f, 0.5f }, new float[] { 50f, 100f }, true,
            0, 1, 1, new(-1.5f, -1.5f), new(1.5f, 1.5f),
            new(1, 2), 0.5f, 1f,
            30
        );
}