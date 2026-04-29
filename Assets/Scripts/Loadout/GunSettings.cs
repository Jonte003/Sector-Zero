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
            "Assault Rifle",             //gunName
            7f,                          //damage
            4f,                          //fireRate
            150f,                        //range
            2.5f,                        //reloadSpeed
            35,                          //maxAmmo
            true,                        //fullAuto
            1f,                          //moveSpeed
            new float[] { 0.85f, 0.7f }, //damageFalloffPercentage
            new float[] { 75f, 100f },   //damageFalloffRange
            false,                       //damageFalloffLerp
            60f,                         //pierceFalloff
            0,                           //delayBetweenBullets
            1,                           //bulletCount
            1,                           //ammoPerShot
            new(-1.5f, -1.5f),           //minSpread
            new(1.5f, 1.5f),             //maxSpread
            new(1, 2),                   //recoilMagnitude
            0.5f,                        //recoilMin
            1f,                          //recoilMax
            30                           //tracerPoolSize
        );

    public static GunSettings BurstRifle => new
        (
            "Burst Rifle",               //gunName
            10f,                         //damage
            1.5f,                        //fireRate
            150f,                        //range
            2.5f,                        //reloadSpeed
            30,                          //maxAmmo
            false,                       //fullAuto
            0.95f,                       //moveSpeed
            new float[] { 0.85f, 0.7f }, //damageFalloffPercentage
            new float[] { 75f, 100f },   //damageFalloffRange
            false,                       //damageFalloffLerp
            65f,                         //pierceFalloff
            0.1f,                        //delayBetweenBullets
            3,                           //bulletCount
            3,                           //ammoPerShot
            new(-1f, -1f),               //minSpread
            new(1f, 1f),                 //maxSpread
            new(1, 2),                   //recoilMagnitude
            0.5f,                        //recoilMin
            1f,                          //recoilMax
            30                           //tracerPoolSize
        );

    public static GunSettings Shotgun => new
        (
            "Shotgun",                         //gunName
            3.5f,                              //damage
            1.5f,                              //fireRate
            40f,                               //range
            2f,                                //reloadSpeed
            8,                                 //maxAmmo
            false,                             //fullAuto
            0.85f,                             //moveSpeed
            new float[] { 0.8f, 0.5f },        //damageFalloffPercentage
            new float[] { 20f, 30f },          //damageFalloffRange
            true,                              //damageFalloffLerp
            75f,                               //pierceFalloff
            0,                                 //delayBetweenBullets
            8,                                 //bulletCount
            1,                                 //ammoPerShot
            new(-4f, -4.5f),                   //minSpread
            new(4f, 4f),                       //maxSpread
            new(1, 2),                         //recoilMagnitude
            0.5f,                              //recoilMin
            1f,                                //recoilMax
            30                                 //tracerPoolSize
        );

    public static GunSettings Pistol => new
        (
            "Pistol",                                    //gunName
            10f,                                         //damage
            3.5f,                                        //fireRate
            125f,                                        //range
            1.2f,                                        //reloadSpeed
            12,                                          //maxAmmo
            false,                                       //fullAuto
            1.25f,                                       //moveSpeed
            new float[] { 0.8f, 0.65f, 0.5f },           //damageFalloffPercentage
            new float[] { 75f, 100f, 125f },             //damageFalloffRange
            true,                                        //damageFalloffLerp
            80f,                                         //pierceFalloff
            0,                                           //delayBetweenBullets
            1,                                           //bulletCount
            1,                                           //ammoPerShot
            new(-0.75f, -0.75f),                         //minSpread
            new(0.75f, 0.75f),                           //maxSpread
            new(0.75f, 3),                               //recoilMagnitude
            0.5f,                                        //recoilMin
            1f,                                          //recoilMax
            30                                           //tracerPoolSize
        );

    public static GunSettings Smg => new
        (
            "Smg",                                       //gunName
            4f,                                          //damage
            8f,                                          //fireRate
            100f,                                        //range
            0.7f,                                        //reloadSpeed
            20,                                          //maxAmmo
            true,                                        //fullAuto
            1.15f,                                       //moveSpeed
            new float[] { 0.9f, 0.75f, 0.5f },           //damageFalloffPercentage
            new float[] { 50f, 75f, 100f },              //damageFalloffRange
            true,                                        //damageFalloffLerp
            85f,                                         //pierceFalloff
            0,                                           //delayBetweenBullets
            1,                                           //bulletCount
            1,                                           //ammoPerShot
            new(-1.25f, -1.25f),                         //minSpread
            new(1.25f, 1.25f),                           //maxSpread
            new(1.5f, 2f),                               //recoilMagnitude
            0.75f,                                       //recoilMin
            1f,                                          //recoilMax
            30                                           //tracerPoolSize
        );

    public static GunSettings Revolver => new
        (
            "Revolver",                                       //gunName
            25f,                                              //damage
            0.75f,                                            //fireRate
            150f,                                             //range
            1.4f,                                             //reloadSpeed
            6,                                                //maxAmmo
            false,                                            //fullAuto
            1.2f,                                             //moveSpeed
            new float[] { 0.85f, 0.7f, 0.6f, 0.45f },         //damageFalloffPercentage
            new float[] { 75f, 100f, 125f, 150f },            //damageFalloffRange
            true,                                             //damageFalloffLerp
            45f,                                              //pierceFalloff
            0,                                                //delayBetweenBullets
            1,                                                //bulletCount
            1,                                                //ammoPerShot
            new(-0.1f, -0.1f),                                //minSpread
            new(0.1f, 0.1f),                                  //maxSpread
            new(3f, 10f),                                     //recoilMagnitude
            0.75f,                                            //recoilMin
            1f,                                               //recoilMax
            30                                                //tracerPoolSize
        );
}