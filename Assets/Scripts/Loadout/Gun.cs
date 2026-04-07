using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunSettings))]
public class Gun : MonoBehaviour
{
    [HideInInspector] public GunSettings settings;

    private LayerMask Mask = 72;

    private Queue<BulletTracer> TracerPool;

    private float timeSinceLastShot = 0f;
    private float reloadProgress = 0f;

    public float ReloadProgress => reloadProgress;
    public float ReloadSpeed => settings.reloadSpeed;

    private bool canShoot = false;

    private int currentAmmo;

    public bool CanShoot => canShoot;


    private void Awake()
    {
        settings = GetComponent<GunSettings>();
        currentAmmo = settings.MaxAmmo;
    }

    private void Update()
    {
        if (timeSinceLastShot < (1 / settings.FireRate))
            timeSinceLastShot += Time.deltaTime;

        canShoot = !isReloading && timeSinceLastShot >= (1 / settings.FireRate) && currentAmmo > 0;

        transform.rotation = Quaternion.Euler(transform.parent.GetComponent<PlayerLook>().xRotation, transform.parent.GetComponent<PlayerLook>().yRotation, 0);
    }
    private void Start()
    {
        TracerPool = new Queue<BulletTracer>();

        for (int i = 0; i < settings.TracerPoolSize; i++)
        {
            var t = Instantiate(settings.TracerPrefab);
            t.gameObject.SetActive(false);
            TracerPool.Enqueue(t);
        }
    }

    #region Reloading
    private bool isReloading = false;
    public bool IsReloading => isReloading;

    public void TryReload()
    {
        if (!isReloading && currentAmmo < settings.MaxAmmo)
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

        currentAmmo = settings.MaxAmmo;
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
        else
        {
            TryReload();
        }
    }

    private IEnumerator ShootRoutine()
    {
        bool applyPerBullet = settings.DelayBetweenBullets > 0f;

        if (!applyPerBullet)
            ApplyRecoil();


        for (int i = 0; i < settings.BulletCount; i++)
        {
            Vector3 end = SimulateShot();

            Vector3 start = settings.Muzzle.position;

            SpawnTracer(start, end);

            if (applyPerBullet)
                ApplyRecoil();

            if (settings.DelayBetweenBullets > 0f)
                yield return new WaitForSeconds(settings.DelayBetweenBullets);
        }
    }

    private void Shoot()
    {
        timeSinceLastShot = 0f;
        currentAmmo -= settings.AmmoPerShot;
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

    private Vector3 SimulateShot()
    {
        Transform cam = Camera.main.transform;

        float spreadX = Random.Range(settings.MinSpread.x, settings.MaxSpread.x);
        float spreadY = Random.Range(settings.MinSpread.y, settings.MaxSpread.y);

        Quaternion spreadRot = Quaternion.AngleAxis(spreadX, cam.up) * Quaternion.AngleAxis(spreadY, cam.right);

        Vector3 camDir = (spreadRot * cam.forward).normalized;

        Vector3 targetPoint;

        if (Physics.Raycast(cam.position, camDir, out RaycastHit hit, settings.Range, Mask))
        {
            targetPoint = hit.point;

            if (hit.transform.CompareTag("Enemy"))
            {
                float finalDamage = CalculateDamage(hit.distance);
                hit.transform.GetComponent<EnemyStats>().DoDamageToEnemy(finalDamage);
            }
        }
        else
        {
            targetPoint = cam.position + camDir * settings.Range;
        }

        return targetPoint;
    }

    private float CalculateDamage(float distance)
    {
        float baseDamage = settings.Damage;

        if (settings.DamageFalloffPercentage.Length == 0 || settings.DamageFalloffRange.Length == 0)
            return baseDamage;

        if (settings.DamageFalloffPercentage.Length != settings.DamageFalloffRange.Length)
        {
            Debug.LogError($"{settings.GunName}: Damage falloff arrays must be the same length.");
            return baseDamage;
        }

        for (int i = 0; i < settings.DamageFalloffRange.Length; i++)
        {
            if (distance <= settings.DamageFalloffRange[i])
            {
                float pct = settings.DamageFalloffPercentage[i];

                if (!settings.DamageFalloffLerp || i == 0)
                    return baseDamage * pct;

                float prevRange = settings.DamageFalloffRange[i - 1];
                float prevPct = settings.DamageFalloffPercentage[i - 1];

                float t = Mathf.InverseLerp(prevRange, settings.DamageFalloffRange[i], distance);
                float lerpedPct = Mathf.Lerp(prevPct, pct, t);

                return baseDamage * lerpedPct;
            }
        }

        return baseDamage * settings.DamageFalloffPercentage[^1];
    }

    private void ApplyRecoil()
    {
        float magnitude = Random.Range(settings.RecoilMin, settings.RecoilMax);

        float recoilX = magnitude * (Random.value < 0.5f ? -settings.RecoilMagnitude.x : settings.RecoilMagnitude.x);
        float recoilY = magnitude * settings.RecoilMagnitude.y;

        transform.parent.GetComponent<PlayerLook>().AddRecoil(recoilX, recoilY);
    }
    #endregion
    #endregion

    #region Debug

    public string CurrentAmmo()
    {
        return currentAmmo + "/" + settings.MaxAmmo;
    }
    public int CurrentAmmoInt()
    {
        return currentAmmo;
    }

    public int MaxAmmoInt()
    {
        return settings.MaxAmmo;
    }
    #endregion
}