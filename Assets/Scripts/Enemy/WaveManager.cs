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

    int currentWave;


    void Start()
    {
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

        for (int i = spawnPointsDeactiveted.Count + 1; i >= 0; i--)
        {
            SpawnPoint sp = spawnPointsDeactiveted[i];

            if (sp.ActivateOnWave == currentWave)
            {
                sp.ActivateSpawnPoint();
                spawnPointsDeactiveted.RemoveAt(i);
                spawnPointsActive.Add(sp);
            }
        }

        LoadEnemyQueue();
    }


    void LoadEnemyQueue()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
