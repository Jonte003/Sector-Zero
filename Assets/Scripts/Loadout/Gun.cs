using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [HideInInspector] public GunSettings settings;

    [HideInInspector] public GunMod[] gunMods;

    public Transform Muzzle;

    public BulletTracer TracerPrefab;

    private LayerMask Mask = 72;

    private Queue<BulletTracer> TracerPool;

    private float timeSinceLastShot = 0f;
    private float reloadProgress = 0f;

    public float ReloadProgress => reloadProgress;
    public float ReloadSpeed => FinalReloadSpeed;

    private bool canShoot = false;

    private int currentAmmo;

    private GunMod finalModStats = new Assignable(0, 0, 0, 0, 0, 0, 0);

    public bool CanShoot => canShoot;

    public float FinalDamage => settings.Damage * (1 + finalModStats.WeaponDamage) * (1 + transform.parent.parent.GetComponent<PlayerStats>().damageBuffs / 100);
    public float FinalFireRate => settings.FireRate * (1 + finalModStats.FireRate);
    public float FinalMoveSpeed => settings.MoveSpeed * (1 + finalModStats.MoveSpeed);
    public int FinalMagSize => (int)(settings.MaxAmmo * (1 + finalModStats.MagSize));
    public float FinalReloadSpeed => settings.ReloadSpeed * (1 + finalModStats.ReloadSpeed);

    private GunMod AddMods()
    {
        float weaponDamage = 0f;
        float fireRate = 0f;
        float spread = 0f;
        float recoil = 0f;
        float moveSpeed = 0f;
        float magSize = 0f;
        float reloadSpeed = 0f;

        for(int i = 0; i < gunMods.Length; i++)
        {
            weaponDamage += gunMods[i].WeaponDamage;
            fireRate += gunMods[i].FireRate;
            spread += gunMods[i].Spread;
            recoil += gunMods[i].Recoil;
            moveSpeed += gunMods[i].MoveSpeed;
            magSize += gunMods[i].MagSize;
            reloadSpeed += gunMods[i].ReloadSpeed;
        }

        return new Assignable(weaponDamage, fireRate, spread, recoil, moveSpeed, magSize, reloadSpeed);
    }

    private void Update()
    {
        if (timeSinceLastShot < (1 / FinalFireRate))
            timeSinceLastShot += Time.deltaTime;

        canShoot = !isReloading && timeSinceLastShot >= (1 / FinalFireRate) && currentAmmo > 0 && !Pause.IsPaused;

        transform.localRotation = Quaternion.identity;

        if (currentAmmo == 0)
        {
            TryReload();
        }
    }
    private void Start()
    {
        settings.Muzzle = Muzzle;

        finalModStats = AddMods();
        transform.parent.parent.GetComponent<PlayerMovement>().CalcMoveSpeed();

        currentAmmo = FinalMagSize;

        TracerPool = new Queue<BulletTracer>();

        for (int i = 0; i < settings.TracerPoolSize; i++)
        {
            var t = Instantiate(TracerPrefab);
            t.gameObject.SetActive(false);
            TracerPool.Enqueue(t);
        }
    }

    #region Reloading
    private bool isReloading = false;
    public bool IsReloading => isReloading;

    public void TryReload()
    {
        if (!isReloading && currentAmmo < FinalMagSize)
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

        currentAmmo = FinalMagSize;
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

        float spreadX = Random.Range(settings.MinSpread.x, settings.MaxSpread.x) * (1 + AddMods().Spread);
        float spreadY = Random.Range(settings.MinSpread.y, settings.MaxSpread.y) * (1 + AddMods().Spread);

        Quaternion spreadRot = Quaternion.AngleAxis(spreadX, cam.up) * Quaternion.AngleAxis(spreadY, cam.right);

        Vector3 camDir = (spreadRot * cam.forward).normalized;

        Vector3 targetPoint = Vector3.zero;

        RaycastHit[] hits = Physics.RaycastAll(cam.position, camDir, settings.Range, Mask);
        hits = hits.OrderBy(hit => hit.distance).ToArray();

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Count(); i++)
            {
                if (!hits[i].transform.CompareTag("Enemy"))
                {
                    targetPoint = hits[i].point;
                    break;
                }   
                
                targetPoint = hits[i].point;
                float finalDamage = CalculateDamage(hits[i].distance) * (1 - settings.PierceFalloff / 100 * i);

                Debug.Log("Enemy number " + i + ", " + finalDamage + " damage");

                if (finalDamage <= 0)
                    break;
                
                hits[i].transform.GetComponent<EnemyStats>().DoDamageToEnemy(finalDamage);
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
        float baseDamage = FinalDamage;

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
        float magnitude = Random.Range(settings.RecoilMin, settings.RecoilMax) * (1 + AddMods().Recoil);

        float recoilX = magnitude * (Random.value < 0.5f ? -settings.RecoilMagnitude.x : settings.RecoilMagnitude.x);
        float recoilY = magnitude * settings.RecoilMagnitude.y;

        transform.parent.parent.GetComponent<PlayerLook>().AddRecoil(recoilX, recoilY);
    }
    #endregion
    #endregion

    #region Debug

    public string CurrentAmmo()
    {
        return currentAmmo + "/" + FinalMagSize;
    }
    public int CurrentAmmoInt()
    {
        return currentAmmo;
    }

    public int MaxAmmoInt()
    {
        return FinalMagSize;
    }
    #endregion
}