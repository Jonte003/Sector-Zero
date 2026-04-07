using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStats : MonoBehaviour
{
    [SerializeField, Tooltip("Time for enemy to be spawned")] float timeToSpawn;
    [SerializeField, Tooltip("Experience Drop On Death")] float expDrop;
    [Space]
    [SerializeField, Tooltip("Multiplier for increasing/decrease health per wave, if 1.1 health will increase by 10% evry wave. If spawned on wave 5, the health will be:  Base Health * 1.1^5")] float healthMultiplier;
    [SerializeField, Tooltip("Multiplier for DPS")] float DPSMultiplier;
    [Space]
    [SerializeField] float baseHealth;
    [SerializeField] float baseDPS;
    [Space]
    [SerializeField] private float health;
    [SerializeField] private float DPS;

    private float maxHealth;

    GameObject target;
    PlayerStats playerStats;


    void Start()
    {
        float waveNumber = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>().CurrentWave;
        health = baseHealth * Mathf.Pow(healthMultiplier, waveNumber); //Calculates health depending on wave number
        maxHealth = health;
        DPS = baseDPS * Mathf.Pow(DPSMultiplier, waveNumber); //Calculates DPS depending on wave number
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
    }

    public void DoDamageToEnemy(float amount)
    {

        health -= amount;

        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("EnemyController").GetComponent<Controller>().AddExperiece(expDrop);
            Destroy(gameObject);

        }
    }

    public void DoDamageToTarget()
    {
        playerStats.DoDamage(DPS);
    }

    public float TimeToSpawn
    {
        get { return timeToSpawn; }
    }

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
}
