using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    PlayerLevels playerLevels;
    public List<Transform> allEnemies;

    Queue<EnemyAgentAI> enemyQueue;
    void Start()
    {
        enemyQueue = new Queue<EnemyAgentAI>();
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
        enemyQueue.Enqueue(enemy.transform.GetComponent<EnemyAgentAI>());
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
}
