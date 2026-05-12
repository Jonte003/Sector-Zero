using UnityEngine;
using System.Collections;
public class ParticleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject rippleEffect;
    [SerializeField] GameObject speedEffect;
    [SerializeField] GameObject jumpEffect;
    [SerializeField] GameObject healthEffect;
    [SerializeField] GameObject eruptionEffect;
    [SerializeField] GameObject growingSphere;

    [SerializeField] GameObject ExplosionEffect;

    [SerializeField] GameObject trailEffect;

    [SerializeField] bool playRipple;
    [SerializeField] bool playSpeedLines;
    [SerializeField] bool playJumpLines;
    [SerializeField] bool playHeathEffect;
    [SerializeField] bool playExplosion;
    [SerializeField] bool playEruption;


    ParticleSystem trailEffectSystem;
    ParticleSystem speedLinesEffect;
    ParticleSystem jumpLinesEffect;
    ParticleSystem healthEffectSystem;


    [SerializeField] LayerMask layerMask;
    Transform playerTransform;
    Transform cameraTransform;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        speedLinesEffect = speedEffect.GetComponent<ParticleSystem>();
        jumpLinesEffect = jumpEffect.GetComponent<ParticleSystem>();
        healthEffectSystem = healthEffect.GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        if (playRipple)
        {
            PlayRippleEffect(playerPos,1);
            playRipple = false;
        }

        if (playSpeedLines)
        {
            PlaySpeedLines(1);
            playSpeedLines = false;
        }

        if (playJumpLines)
        {
            PlayJumpLines(1);
            playJumpLines = false;
        }
        if (playHeathEffect)
        {
            PlayHealthEffect(5);
            playHeathEffect = false;
        }
        if (playExplosion)
        {
            PlayExplosionEffect(new Vector3(0,7,0), 1);
            playExplosion = false;
        }
        if (playEruption)
        {
            PlayEruptionEffect(1, transform.position, 1.5f);
            playEruption = false;
        }


    }

    public void PlayRippleEffect(Vector3 player, int abilityLevel)
    {
        float yDelta = 0.2f; //The distance from the ground to spawn rippleEffect
        RaycastHit hit;

        if (Physics.Raycast(player, Vector3.down, out hit, 5f, layerMask))
        {
            Vector3 spawnPoint = new Vector3(hit.point.x, hit.point.y + yDelta, hit.point.z);
            GameObject effect = Instantiate(rippleEffect, spawnPoint, Quaternion.Euler(90,0,0));
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            ps.startSize = abilityLevel;
            Destroy(effect, 2f);
        }

    }
    public void PlaySpeedLines(float duration)
    {
        StartCoroutine(PlaySpeedLinesRoutine(duration));

    }

    private IEnumerator PlaySpeedLinesRoutine(float duration)
    {
        speedLinesEffect.Play();
        yield return new WaitForSeconds(duration); 
        speedLinesEffect.Stop();
    }

    public void PlayJumpLines(float duration)
    {
        StartCoroutine(PlayJumpLinesRoutine(duration));

    }

    private IEnumerator PlayJumpLinesRoutine(float duration)
    {
        jumpLinesEffect.Play();
        yield return new WaitForSeconds(duration);
        jumpLinesEffect.Stop();
    }

    public void PlayHealthEffect(float duration)
    {
        StartCoroutine(PlayHealthEffectRoutine(duration));
        Debug.Log("HeathEffectPlayer");
    }


    private IEnumerator PlayHealthEffectRoutine(float duration)
    {
        healthEffectSystem.Play();
        yield return new WaitForSeconds(duration);
        healthEffectSystem.Stop();
    }

    public void PlayExplosionEffect(Vector3 position, float scale)
    {
        StartCoroutine(PlayExplosionEffectRoutine(position, scale));
    }

    private IEnumerator PlayExplosionEffectRoutine(Vector3 position, float scale)
    {
        GameObject e = Instantiate(ExplosionEffect, position, Quaternion.identity);
        e.transform.localScale = new Vector3(scale, scale, scale);
        yield return new WaitForSeconds(4);
        Destroy(e);
    }

    public void PlayEruptionEffect(float explosionScale, Vector3 position, float timer)
    {
        GameObject e = Instantiate(eruptionEffect, position, Quaternion.identity);
        e.GetComponent<EruptionAbility>().StartCountdown(gameObject, timer, explosionScale, growingSphere);
    }
}
