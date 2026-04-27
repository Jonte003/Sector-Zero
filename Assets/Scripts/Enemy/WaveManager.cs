using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class WaveManager : MonoBehaviour
{

    List<GameObject> gameObjectInQueue;
    List<SpawnPoint> spawnPointsActive;
    List<SpawnPoint> spawnPointsDeactiveted;

    List<Wave> allWaves;
    Wave currentW;
    Wave nextWave;


    int currentWave;
    [SerializeField, InspectorName("Force start next wave")] bool startNextWave;

    [SerializeField, Tooltip("Applies a multipler to the spawntimer each new wave, 0.9 increases spawn rate by 10% each wave")] float spawnTimeMultiplier;
    [SerializeField] float currentTimerMultiplier;

    Controller enemyController;

    [SerializeField] bool waveIsSpawning;

    private float timeToNextWave = 0;

    void Start()
    {
        gameObjectInQueue = new List<GameObject>();
        spawnPointsDeactiveted = new List<SpawnPoint>();
        spawnPointsActive = new List<SpawnPoint>();
        allWaves = new List<Wave>();
        enemyController = GameObject.FindWithTag("EnemyController").GetComponent<Controller>();

        foreach (Transform child in transform.Find("SpawnPoints")) //Add all spawnpoints of child to list
        {
            spawnPointsDeactiveted.Add(child.gameObject.GetComponent<SpawnPoint>());
        }

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

        currentTimerMultiplier *= spawnTimeMultiplier;
        currentWave++;
        waveIsSpawning = true;

        currentW = allWaves[currentWave - 1];
        nextWave = allWaves[currentWave];

        for (int i = spawnPointsDeactiveted.Count - 1; i >= 0; i--)
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

        if (gameObjectInQueue.Count > 0)
        {
            foreach (SpawnPoint sp in spawnPointsActive)
            {
                if (sp.ReadyToSpawn)
                {
                    sp.SpawnEnemy(gameObjectInQueue[0], currentTimerMultiplier);
                    gameObjectInQueue.RemoveAt(0);
                }
            }
        }
        else
        {
            waveIsSpawning = false;
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

    
}

