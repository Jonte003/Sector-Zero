using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class Wave : MonoBehaviour
{
    [SerializeField] List<EnemyWithAmount> enemiesWithAmount;
    [SerializeField] float timeToNextWave;

    public List<GameObject> GetWave()
    {
        List<GameObject> wave = new List<GameObject>();

        foreach (EnemyWithAmount enemyWithAmount in enemiesWithAmount) //loop through every enemytype in wave
        {
            for (int i = 0; i < enemyWithAmount.Amount; i++) //add amount of enemy of enemytype
            {
                wave.Add(enemyWithAmount.Prefab);
            }
        }
        return wave;
    }
}

[System.Serializable] //edit class in inspector while in list
public class EnemyWithAmount
{
    [SerializeField] GameObject prefab;
    [SerializeField] int amount;

    public GameObject Prefab
    {
        get { return prefab; }
    }

    public int Amount
    {
        get { return amount; }
    }
}
