using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private string GunName;

    [SerializeField] private Transform Muzzle;

    // Normal
    [SerializeField, Tooltip("Damage per bullet"), Header("Normal Stats")] private float Damage;
    [SerializeField, Tooltip("Amount of bullets fired per second")] private float FireRate;
    [SerializeField, Tooltip("The Range you can shoot in unity units")] private float Range;
    [SerializeField, Tooltip("The amount of seconds it takes to reload")] private float ReloadSpeed;
    [SerializeField, Tooltip("The amount of seconds it takes to reload")] private int MaxAmmo;

    // Damage Falloff
    [SerializeField, Tooltip("Percentage of normal damage connected to the set range"), Header("Damage Falloff")] private float[] DamageFalloffPercentage;
    [SerializeField, Tooltip("Range where damage falloff will change connected to the percentage")] private float[] DamageFalloffRange;
    [SerializeField, Tooltip("If damage falloff will lerp or not")] private bool DamageFalloffLerp;

    // Multiple Bullets
    [SerializeField, Tooltip("Delay between each bullet in seconds"), Header("Multiple Bullets")] private float DelayBetweenBullets;
    [SerializeField, Tooltip("Amount of bullets fired per shot")] private int BulletCount;
    [SerializeField, Tooltip("Minimum amount of spread")] private Vector2 MinSpread;
    [SerializeField, Tooltip("Maximum amount of spread")] private Vector2 MaxSpread;

    // Recoil
    [SerializeField, Tooltip("How much recoil shooting will cause"), Header("Recoil")] private Vector2 RecoilMagnitude;
    [SerializeField, Tooltip("Minimum amount of recoil shooting will cause, has to be lower than Recoil Max"), Range(0, 1)] private float RecoilMin;
    [SerializeField, Tooltip("Maximum amount of recoil shooting will cause, has to be higher than Recoil Min"), Range(0, 1)] private float RecoilMax;

    // QOL
    [SerializeField, Tooltip("Amount if time you can try to shoot before the game allows you and the shot will buffer"), Header("QOL")] private float BufferWindow;

    private LayerMask Mask = 0;

    private float timeSinceLastShot = 0f;
    private float reloadProgress = 0f;

    private bool canShoot = false;
    private bool bufferedShot = false;

    private int currentAmmo;


    private void Awake()
    {
        currentAmmo = MaxAmmo;
    }
    private void Update()
    {
        if (timeSinceLastShot < (1 / FireRate))
            timeSinceLastShot += Time.deltaTime;

        canShoot = !isReloading && timeSinceLastShot >= (1 / FireRate) && currentAmmo > 0;

        if (bufferedShot && canShoot)
        {
            bufferedShot = false;
            Shoot();
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
        else if ((1f / FireRate) - timeSinceLastShot <= BufferWindow || (isReloading && ReloadSpeed - reloadProgress <= BufferWindow))
        {
            bufferedShot = true;
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
            float finalDamage = CalculateDamage(direction, out RaycastHit hit);

            if (hit.transform != null && hit.transform.CompareTag("Enemy"))
            {
                // Apply damage
            }

            if (applyPerBullet)
                ApplyRecoil(); 

            if (DelayBetweenBullets > 0f)
                yield return new WaitForSeconds(DelayBetweenBullets);
        }
    }

    private void Shoot()
    {
        timeSinceLastShot = 0f;
        currentAmmo--;
        StartCoroutine(ShootRoutine());
    }
    #region Shooting Calulations

    private Vector3 GetBulletDirection()
    {
        float spreadX = Random.Range(MinSpread.x, MaxSpread.x);
        float spreadY = Random.Range(MinSpread.y, MaxSpread.y);

        Quaternion spreadRot = Quaternion.Euler(spreadY, spreadX, 0f);
        return (spreadRot * transform.forward).normalized;
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
}