using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStats : MonoBehaviour
{

    [SerializeField] float health = 100;
    [SerializeField] float timeToSpawn = 1;
    [SerializeField] float DPS = 5;

    GameObject target;
    PlayerStats playerStats;


    void Start()
    {
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this);
        }
    }

    public void DoDamage()
    {
        playerStats.DoDamage(DPS);
    }

    public float TimeToSpawn
    {
        get { return timeToSpawn; }
    }
}
