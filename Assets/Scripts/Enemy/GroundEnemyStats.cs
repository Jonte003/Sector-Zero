using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundEnemyStats : EnemyStats
{
    [Space]
    [SerializeField] float baseDamage;
    [SerializeField, Tooltip("Multiplier for Damage")] float damageMultiplier;
    GroundEnemyAI movmentAI;
    float damage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        float waveNumber = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>().CurrentWave;
        movmentAI = GetComponent<GroundEnemyAI>();
        damage = baseDamage * Mathf.Pow(damageMultiplier, waveNumber); //Calculates Damage depending on wave number
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
    }

    public override void DoDamageToEnemy(float amount)
    {
        base.DoDamageToEnemy(amount);
        movmentAI.OnDamageTaken(); 
    }


    public void DoDamageToTarget()
    {
        playerStats.DoDamageFixed(damage);
    }

}
