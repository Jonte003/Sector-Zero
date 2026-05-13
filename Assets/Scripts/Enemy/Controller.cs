using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    PlayerLevels playerLevels;
    public List<Transform> allEnemies; //May contain wrong y value in position
    List<Transform> allEnemiesTruePosition; //Always include correct y value
    List<Transform> bossesSpawned;
    Queue<EnemyAgentAI> enemyQueue;

    Transform enemyController;
    void Start()
    {
        enemyQueue = new Queue<EnemyAgentAI>();
        allEnemies = new List<Transform>();
        playerLevels = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevels>();
        bossesSpawned = new List<Transform>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").transform;
    }
    void Update()
    {
        UpdatePaths();
    }

    public void AddEnemy(GameObject enemy, bool isBoss)
    {
        if (enemy.CompareTag("Drone"))
        {
            allEnemies.Add(enemy.transform.GetChild(0));
        }
        else if (enemy.CompareTag("GroundEnemy"))
        {
            allEnemies.Add(enemy.transform);
        }
        else
        {
            Debug.LogWarning("Enemy does not have a valid tag");
        }
        enemyQueue.Enqueue(enemy.transform.GetComponent<EnemyAgentAI>());

        if(isBoss)
        {
            bossesSpawned.Add(enemy.transform);
        }
    } 

    public void RemoveEnemy(GameObject gameObject)
    {
        if(gameObject.CompareTag("Drone"))
        {
            allEnemies.Remove(gameObject.transform.GetChild(0));
        }
        else
        {
            allEnemies.Remove(gameObject.transform);
        }
        foreach(Transform t in bossesSpawned)
        {
            if (gameObject.transform == t)
            {
                BossDead(gameObject.transform);
                break;
            }
        }
    }

    private void UpdatePaths()
    {
        if (enemyQueue.Count == 0)
        {
            return; //No enemys spawned
        }

        EnemyAgentAI firstenemy = enemyQueue.Dequeue();

        if (firstenemy == null)
        {
            return; //Enemy has died
        }

        firstenemy.CalculatePath();

        enemyQueue.Enqueue(firstenemy);

            
    }

    public void AddExperiece(float amount)
    {
        playerLevels.AddExperience(amount);
    }

    public List<Transform> AllEnemies
    {
        get { return allEnemies; }
    }

    

    private void BossDead(Transform enemy)
    {
        bossesSpawned.Remove(enemy);
        if (bossesSpawned.Count == 0)
        {
            TriggerVictory();
        }
    }

    private void TriggerVictory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Title Screen");
    }
}
