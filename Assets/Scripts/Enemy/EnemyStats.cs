using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyStats : MonoBehaviour
{
    [SerializeField, Tooltip("Time for enemy to be spawned")] float timeToSpawn;
    [SerializeField, Tooltip("Experience Drop On Death")] float expDrop;
    [Space]
    [SerializeField, Tooltip("Multiplier for increasing/decrease health per wave, if 1.1 health will increase by 10% evry wave. If spawned on wave 5, the health will be:  Base Health * 1.1^5")] float healthMultiplier;
    [SerializeField] float baseHealth;

    private bool isBoss;
    private float maxHealth;
    private float health;

    protected GameObject target;
    protected PlayerStats playerStats;


    protected virtual void Start()
    {
        float waveNumber = GameObject.FindWithTag("WaveManager").GetComponent<WaveManager>().CurrentWave;
        health = baseHealth * Mathf.Pow(healthMultiplier, waveNumber); //Calculates health depending on wave number
        maxHealth = health;
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
    }

    public void DoDamageToEnemy(float amount)
    {

        health -= amount;

        if (health <= 0)
        {
            health = 0;

            GameObject.FindGameObjectWithTag("EnemyController").GetComponent<Controller>().AddExperiece(expDrop);
            expDrop = 0; //Required to stop exp drop multiple times when using shotgun

            if (transform.parent.tag != "EnemyController")
            {
                GameObject.FindWithTag("EnemyController").GetComponent<Controller>().RemoveEnemy(transform.parent.gameObject);
            }
            else
            {
                GameObject.FindWithTag("EnemyController").GetComponent<Controller>().RemoveEnemy(gameObject);

            }

            Animator animator = GetComponent<Animator>();

            if (animator != null )
            {
                animator.SetTrigger("OnDeath");

                NavMeshAgent agent = GetComponent<NavMeshAgent>();

                if (agent == null)
                    agent = GetComponentInParent<NavMeshAgent>();

                agent.velocity = Vector3.zero;
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.isStopped = true;

            }
            else
            {
                SelfDestroyGameObject();
            }


        }
    }

    public void SelfDestroyGameObject()
    {

        
        if (transform.parent.GetComponent<NavMeshAgent>() != null) //If gameobject is a drone destoy its parent otherwise only destoy current gameobject
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
    }

    public void DestroyCollider()
    {
        Destroy(GetComponent<Collider>());
    }

    public float TimeToSpawn
    {
        get { return timeToSpawn; }
    }

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
}
