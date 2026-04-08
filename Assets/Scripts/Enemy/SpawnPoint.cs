using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] int activateOnWave = 1;
    bool active = false;

    bool readyToSpawn = true;
    GameObject enemyBeingSpawned;
    float timeSinceLastSpawn;
    float timerForSpawn;
    float droneSpawnHeight;

    private void OnDrawGizmos()
    {
        if (active)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject plane = GameObject.FindWithTag("DronePlane");

        if (plane != null)
        {
            droneSpawnHeight = plane.transform.position.y;
        }
        else
        {
            droneSpawnHeight = 0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (readyToSpawn == false)
        {
            if (timeSinceLastSpawn > timerForSpawn)
            {
                readyToSpawn = true;
                timeSinceLastSpawn = 0;
                if (enemyBeingSpawned.GetComponent<DroneHorizontalMovement>() != null) 
                {
                    //enemy being spawned is a drone
                    Instantiate(enemyBeingSpawned, new Vector3(transform.position.x, droneSpawnHeight, transform.position.z), transform.rotation, GameObject.FindGameObjectWithTag("EnemyController").transform);

                }
                else 
                {
                    Instantiate(enemyBeingSpawned, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("EnemyController").transform);

                }
            }
        }
    }


    public void SpawnEnemy(GameObject enemy, float timerMultiplier)
    {
        readyToSpawn = false;
        EnemyStats stats = enemy.GetComponent<EnemyStats>() ?? enemy.GetComponentInChildren<EnemyStats>();

        timerForSpawn = stats.TimeToSpawn * timerMultiplier;
        enemyBeingSpawned = enemy;
    }

    public void ActivateSpawnPoint()
    {
        active = true;
    }
    
    public int ActivateOnWave
    {
        get { return activateOnWave; }
    }
    public bool ReadyToSpawn
    {
        get { return readyToSpawn; }
    }
}
