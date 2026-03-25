using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private string GunName;

    // Normal
    [SerializeField, Tooltip("Damage per bullet"), Header("Normal Stats")] private float Damage;
    [SerializeField, Tooltip("Amount of bullets fired per second")] private float FireRate;
    [SerializeField, Tooltip("The Range you can shoot in unity units")] private float Range;
    [SerializeField, Tooltip("The amount of seconds it takes to reload")] private float ReloadSpeed;
    [SerializeField, Tooltip("The amount of seconds it takes to reload")] private int MaxAmmo;
    [Tooltip("If you can hold to shoot multiple bullets")] public bool FullAuto;

    // Damage Falloff
    [SerializeField, Tooltip("Percentage of normal damage connected to the set range"), Header("Damage Falloff")] private float[] DamageFalloffPercentage;
    [SerializeField, Tooltip("Range where damage falloff will change connected to the percentage")] private float[] DamageFalloffRange;
    [SerializeField, Tooltip("If damage falloff will lerp or not")] private bool DamageFalloffLerp;

    // Multiple Bullets
    [SerializeField, Tooltip("Delay between each bullet in seconds"), Header("Multiple Bullets")] private float DelayBetweenBullets;
    [SerializeField, Tooltip("Amount of bullets fired per shot")] private int BulletCount;
    [SerializeField, Tooltip("Ammo consumed per shot")] private int AmmoPerShot;
    [SerializeField, Tooltip("Minimum amount of spread")] private Vector2 MinSpread;
    [SerializeField, Tooltip("Maximum amount of spread")] private Vector2 MaxSpread;

    // Recoil
    [SerializeField, Tooltip("How much recoil shooting will cause"), Header("Recoil")] private Vector2 RecoilMagnitude;
    [SerializeField, Tooltip("Minimum amount of recoil shooting will cause, has to be lower than Recoil Max"), Range(0, 1)] private float RecoilMin;
    [SerializeField, Tooltip("Maximum amount of recoil shooting will cause, has to be higher than Recoil Min"), Range(0, 1)] private float RecoilMax;

    // Bullet Tracers
    [SerializeField, Tooltip("How many tracers can be active at a time"), Header("Bullet Tracers")] private int TracerPoolSize = 30;
    [SerializeField, Tooltip("Where the tracers spawn")] private Transform Muzzle;
    [SerializeField, Tooltip("The prefab for the tracer")] private BulletTracer TracerPrefab;

    private LayerMask Mask = 72;

    private Queue<BulletTracer> TracerPool;

    private float timeSinceLastShot = 0f;
    private float reloadProgress = 0f;

    private bool canShoot = false;

    private int currentAmmo;

    public bool CanShoot => canShoot;


    private void Awake()
    {
        currentAmmo = MaxAmmo;
    }

    private void Update()
    {
        if (timeSinceLastShot < (1 / FireRate))
            timeSinceLastShot += Time.deltaTime;

        canShoot = !isReloading && timeSinceLastShot >= (1 / FireRate) && currentAmmo > 0;

        transform.rotation = Quaternion.Euler(transform.parent.GetComponent<PlayerLook>().xRotation, transform.parent.GetComponent<PlayerLook>().yRotation, 0);
    }
    private void Start()
    {
        TracerPool = new Queue<BulletTracer>();

        for (int i = 0; i < TracerPoolSize; i++)
        {
            var t = Instantiate(TracerPrefab);
            t.gameObject.SetActive(false);
            TracerPool.Enqueue(t);
        }
    }

    #region Reloading

    private bool isReloading = false;

    public void TryReload()
    {
        if (!isReloading && currentAmmo < MaxAmmo)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        reloadProgress = 0f;

        while (reloadProgress < ReloadSpeed)
        {
            reloadProgress += Time.deltaTime;
            yield return null;
        }

        currentAmmo = MaxAmmo;
        isReloading = false;
    }

    #endregion

    #region Shooting
    public void TryShoot()
    {
        if (canShoot)
        {
            Shoot();
        }
    }

    private IEnumerator ShootRoutine()
    {
        bool applyPerBullet = DelayBetweenBullets > 0f;

        if (!applyPerBullet)
            ApplyRecoil();

        for (int i = 0; i < BulletCount; i++)
        {
            Vector3 direction = GetBulletDirection();

            RaycastHit hit;
            Vector3 start = Muzzle.position;
            Vector3 end;

            if (Physics.Raycast(start, direction, out hit, Range, Mask))
            {
                end = hit.point;

                if (hit.transform.CompareTag("Enemy"))
                {
                    float finalDamage = CalculateDamage(direction, out hit);
                    // Apply damage here
                    hit.transform.GetComponent<EnemyStats>().DoDamageToEnemy(finalDamage);
                }
            }
            else
            {
                end = start + direction * Range;
            }

            SpawnTracer(start, end);

            if (applyPerBullet)
                ApplyRecoil();

            if (DelayBetweenBullets > 0f)
                yield return new WaitForSeconds(DelayBetweenBullets);
        }
    }

    private void Shoot()
    {
        timeSinceLastShot = 0f;
        currentAmmo -= AmmoPerShot;
        StartCoroutine(ShootRoutine());
    }
    private void SpawnTracer(Vector3 start, Vector3 end)
    {
        BulletTracer tracer = TracerPool.Dequeue();

        tracer.GetComponent<TrailRenderer>().enabled = false;

        tracer.gameObject.SetActive(true);

        Vector3 dir = (end - start).normalized;
        tracer.transform.rotation = Quaternion.LookRotation(dir);

        tracer.Init(start, end, ReturnTracerToPool);
    }

    private void ReturnTracerToPool(BulletTracer tracer)
    {
        TracerPool.Enqueue(tracer);
    }

    #region Shooting Calulations

    private Vector3 GetBulletDirection()
    {
        Transform cam = Camera.main.transform;

        float spreadX = Random.Range(MinSpread.x, MaxSpread.x);
        float spreadY = Random.Range(MinSpread.y, MaxSpread.y);

        Quaternion spreadRot = Quaternion.AngleAxis(spreadX, cam.up) * Quaternion.AngleAxis(spreadY, cam.right);

        return (spreadRot * cam.forward).normalized;
    }

    private float CalculateDamage(Vector3 direction, out RaycastHit hit)
    {
        float maxDistance = Range;
        float baseDamage = Damage;

        Physics.Raycast(Muzzle.position, direction, out hit, maxDistance, Mask);

        if (hit.collider == null)
            return 0f;

        float distance = hit.distance;

        if (DamageFalloffPercentage.Length == 0 || DamageFalloffRange.Length == 0)
            return baseDamage;

        if (DamageFalloffPercentage.Length != DamageFalloffRange.Length)
        {
            Debug.LogError($"{GunName}: Damage falloff arrays must be the same length.");
            return baseDamage;
        }

        for (int i = 0; i < DamageFalloffRange.Length; i++)
        {
            if (distance <= DamageFalloffRange[i])
            {
                float pct = DamageFalloffPercentage[i];

                if (!DamageFalloffLerp || i == 0)
                    return baseDamage * pct;

                float prevRange = DamageFalloffRange[i - 1];
                float prevPct = DamageFalloffPercentage[i - 1];

                float t = Mathf.InverseLerp(prevRange, DamageFalloffRange[i], distance);
                float lerpedPct = Mathf.Lerp(prevPct, pct, t);

                return baseDamage * lerpedPct;
            }
        }

        return baseDamage * DamageFalloffPercentage[^1];
    }

    private void ApplyRecoil()
    {
        float magnitude = Random.Range(RecoilMin, RecoilMax);

        float recoilX = magnitude * RecoilMagnitude.x;
        float recoilY = magnitude * RecoilMagnitude.y;

        // transform.Find("Camera").GetComponent<CameraControl>.AddRecoil(recoilX, recoilY);
        // Class not yet created
    }
    #endregion
    #endregion

    #region Debug

    public string CurrentAmmo()
    {
        return currentAmmo + "/" + MaxAmmo;
    }

    #endregion
}