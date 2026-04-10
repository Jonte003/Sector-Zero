using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    PlayerLevels playerLevels;
    List<Transform> allEnemies;

    Queue<EnemyAI> enemyQueue;
    void Start()
    {
        enemyQueue = new Queue<EnemyAI>();
        allEnemies = new List<Transform>();
        playerLevels = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevels>();
    }
    void Update()
    {
        UpdatePaths();
    }

    public void AddEnemy(GameObject enemy)
    {
        allEnemies.Add(enemy.transform);
        enemyQueue.Enqueue(enemy.transform.GetComponent<EnemyAI>());
    } 

    public void RemoveEnemy(GameObject gameObject)
    {
        allEnemies.Remove(gameObject.transform);
    }

    private void UpdatePaths()
    {
        if (enemyQueue.Count == 0)
        {
            return; //No enemys spawned
        }

        EnemyAI firstenemy = enemyQueue.Dequeue();

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
}
