using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] int activateOnWave = 1;
    bool active = false;

    bool readyToSpawn;
    GameObject enemyBeingSpawned;
    float timerForSpawn;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnEnemy(GameObject enemy)
    {
        timerForSpawn = enemy.GetComponent<EnemyStats>().TimeToSpawn;
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
