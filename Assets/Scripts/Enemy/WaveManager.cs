using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class WaveManager : MonoBehaviour
{

    List<GameObject> gameObjectInQueue;


    List<Wave> allWaves;
    Wave currentW;
    Wave nextWave;

    [SerializeField, Tooltip("Start corner of spawnarea")] Transform pos1;
    [SerializeField, Tooltip("End corner of spawnarea")] Transform pos2;
    [SerializeField] int downwardsRayDistance;
    [SerializeField, Tooltip("The minimum distance a enemy can spawn from player")] int enemySpawnDistance;

    [Space]

    int currentWave;
    [SerializeField, InspectorName("Force start next wave")] bool startNextWave;

    Controller enemyController;

    [SerializeField] bool waveIsSpawning;

    private float timeToNextSpawn;
    private float currentTimeIntervalBetweenSpawns;
    private float timeToNextWave = 0;


    void Start()
    {
        gameObjectInQueue = new List<GameObject>();
        allWaves = new List<Wave>();
        enemyController = GameObject.FindWithTag("EnemyController").GetComponent<Controller>();



        foreach (Transform child in transform.Find("Waves")) //Add all waves of child to list
        {
            allWaves.Add(child.gameObject.GetComponent<Wave>());
        }

        currentW = allWaves[0];
        nextWave = allWaves[1];
    }


    public void LoadNextWave()
    {

        Debug.Log($"Wave {currentWave} finished, loading next wave");

        currentWave++;
        waveIsSpawning = true;

        currentW = allWaves[currentWave - 1];
        nextWave = allWaves[currentWave];
        currentTimeIntervalBetweenSpawns = currentW.SpawnRate;

        
        LoadEnemyQueue();
        timeToNextWave = allWaves[currentWave - 1].TimeToNextWave;
    }


    void LoadEnemyQueue()
    {
        Wave wave = allWaves[currentWave - 1];
        gameObjectInQueue = wave.GetWave();        
    }

    void Update()
    {
        timeToNextWave -= Time.deltaTime;

        if (currentW.IsBossWave || nextWave.IsBossWave)
        {
            if (CheckIfAllEnemiesDead())
                startNextWave = true;
        }
        else
        {
            if (timeToNextWave <= 0 || CheckIfAllEnemiesDead())
                startNextWave = true;
        }

        if (startNextWave)
        {
            startNextWave = false;

            if (currentWave >= allWaves.Count) //If last wave
            {
                if (CheckIfAllEnemiesDead())
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    SceneManager.LoadScene("Title Screen");
                }
                return;
            }

            LoadNextWave();
        }

        if (gameObjectInQueue.Count > 0 && timeToNextSpawn <= 0)
        {
            timeToNextSpawn = currentTimeIntervalBetweenSpawns;
            SpawnEnemy(gameObjectInQueue[0]);
            
        }
        else
        {
            timeToNextSpawn -= Time.deltaTime;
        }

    }

    bool CheckIfAllEnemiesDead()
    {
        return enemyController.AllEnemies.Count == 0 && gameObjectInQueue.Count == 0;
    }


    public int CurrentWave
    {
        get { return currentWave; }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = GetRandomPosition();

            RaycastHit hit;
            NavMeshHit navHit;

            if (Physics.Raycast(randomPos, Vector3.down, out hit, downwardsRayDistance))
            {
                if (NavMesh.SamplePosition(hit.point, out navHit, 5f, NavMesh.AllAreas))
                {
                    if(!EnemyAI.CheckIfPositionsInRange(navHit.position, GameObject.FindWithTag("Player").transform.position, enemySpawnDistance))
                    {
                        GameObject e = Instantiate(enemy, navHit.position, Quaternion.identity, enemyController.transform);
                        enemyController.AddEnemy(e);
                        gameObjectInQueue.RemoveAt(0);
                        return;
                    }
                }
            }
        }

        Debug.LogWarning("Failed to find valid spawn position after multiple attempts.");
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position;
        position.x = Random.Range(pos1.position.x, pos2.position.x);
        position.z = Random.Range(pos1.position.z, pos2.position.z);
        position.y = pos1.position.y;

        return position;
    }


}

