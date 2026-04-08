using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundEnemyStats : EnemyStats
{
    [Space]
    [SerializeField] float baseDPS;
    [SerializeField, Tooltip("Multiplier for DPS")] float DPSMultiplier;
    float DPS;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        float waveNumber = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>().CurrentWave;

        DPS = baseDPS * Mathf.Pow(DPSMultiplier, waveNumber); //Calculates DPS depending on wave number
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
    }


    public void DoDamageToTarget()
    {
        playerStats.DoDamage(DPS);
    }

}
