using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    PlayerLevels playerLevels;
    public List<Transform> allEnemies;
    List<Transform> bossesSpawned;
    Queue<EnemyAgentAI> enemyQueue;
    void Start()
    {
        enemyQueue = new Queue<EnemyAgentAI>();
        allEnemies = new List<Transform>();
        playerLevels = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevels>();
        bossesSpawned = new List<Transform>();
    }
    void Update()
    {
        UpdatePaths();
    }

    public void AddEnemy(GameObject enemy,bool isBoss)
    {
        allEnemies.Add(enemy.transform);
        enemyQueue.Enqueue(enemy.transform.GetComponent<EnemyAgentAI>());

        if(isBoss)
        {
            bossesSpawned.Add(enemy.transform);
        }
    } 

    public void RemoveEnemy(GameObject gameObject)
    {
        allEnemies.Remove(gameObject.transform);
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
