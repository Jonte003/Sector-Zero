using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundEnemyStats : EnemyStats
{
    [Space]
    [SerializeField] float baseDamage;
    [SerializeField, Tooltip("Multiplier for Damage")] float damageMultiplier;
    float damage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        float waveNumber = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>().CurrentWave;

        damage = baseDamage * Mathf.Pow(damageMultiplier, waveNumber); //Calculates Damage depending on wave number
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
    }


    public void DoDamageToTarget()
    {
        playerStats.DoDamageFixed(damage);
    }

}
