using UnityEngine;
using System.Collections;
public class ParticleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject rippleEffect;
    [SerializeField] GameObject speedEffect;
    [SerializeField] GameObject jumpEffect;
    [SerializeField] GameObject trailEffect;

    [SerializeField] bool playRipple;
    [SerializeField] bool playSpeedLines;
    [SerializeField] bool playJumpLines;

    ParticleSystem trailEffectSystem;
    ParticleSystem speedLinesEffect;
    ParticleSystem jumpLinesEffect;


    [SerializeField] LayerMask layerMask;
    Transform playerTransform;
    Transform cameraTransform;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        speedLinesEffect = speedEffect.GetComponent<ParticleSystem>();
        jumpLinesEffect = jumpEffect.GetComponent<ParticleSystem>();

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
}
