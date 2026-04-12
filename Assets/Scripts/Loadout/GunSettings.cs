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
    [Tooltip("The prefab for the tracer")] public BulletTracer TracerPrefab;
}