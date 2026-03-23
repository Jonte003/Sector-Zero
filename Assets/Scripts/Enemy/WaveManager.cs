using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class WaveManager : MonoBehaviour
{

    List<GameObject> gameObjectInQueue;
    List<SpawnPoint> spawnPointsActive;
    List<SpawnPoint> spawnPointsDeactiveted;

    List<Wave> allWaves;

    [SerializeField] int currentWave;
    [SerializeField] bool startNextWave;

    public bool waveIsSpawning;

    private float timeToNextWave = 0;

    void Start()
    {
        gameObjectInQueue = new List<GameObject>();
        spawnPointsDeactiveted = new List<SpawnPoint>();
        spawnPointsActive = new List<SpawnPoint>();
        allWaves = new List<Wave>();

        foreach (Transform child in transform.Find("SpawnPoints")) //Add all spawnpoints of child to list
        {
            spawnPointsDeactiveted.Add(child.gameObject.GetComponent<SpawnPoint>());
        }

        foreach (Transform child in transform.Find("Waves")) //Add all waves of child to list
        {
            allWaves.Add(child.gameObject.GetComponent<Wave>());
        }
    }


    public void LoadNextWave()
    {
        currentWave++;
        waveIsSpawning = true;
        for (int i = spawnPointsDeactiveted.Count - 1; i >= 0; i--)
        {
            SpawnPoint sp = spawnPointsDeactiveted[i];
            if (sp.ActivateOnWave == currentWave)
            {
                Debug.Log("Acticated spawnpoint");
                sp.ActivateSpawnPoint();
                spawnPointsDeactiveted.RemoveAt(i);
                spawnPointsActive.Add(sp);
            }
        }

        LoadEnemyQueue();
        timeToNextWave = allWaves[currentWave - 1].TimeToNextWave;
    }


    void LoadEnemyQueue()
    {
        Wave wave = allWaves[currentWave - 1];
        gameObjectInQueue = wave.GetWave();        
    }

    // Update is called once per frame
    void Update()
    {
    timeToNextWave -= Time.deltaTime;
    if (timeToNextWave <= 0)
        {
            LoadNextWave();
        }



    if (startNextWave)
        {
            startNextWave = false;
            LoadNextWave();
        }



    if (gameObjectInQueue.Count > 0)
        {
            foreach (SpawnPoint sp in spawnPointsActive)
            {
                if (gameObjectInQueue.Count > 0)
                    if (sp.ReadyToSpawn == true)
                    {

                        sp.SpawnEnemy(gameObjectInQueue[0]);
                        gameObjectInQueue.RemoveAt(0);
                    }
                else
                    {
                        waveIsSpawning = false;
                    }
            }
        }

    }

    
}
