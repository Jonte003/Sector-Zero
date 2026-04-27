using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class Wave : MonoBehaviour
{
    [SerializeField,Tooltip("Enemys in wave")] List<EnemyWithAmount> enemiesWithAmount;
    [SerializeField,Tooltip("Time to force next wave to start (does not affect bosswaves or waves just before a bosswave)")] float timeToNextWave;
    [SerializeField,Tooltip("If true, wave will not load or complete unless all enemys are dead")] bool isBossWave;


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

    public float TimeToNextWave
    {
        get { return timeToNextWave; }
    }

    public bool IsBossWave
    {
        get { return isBossWave; }
    }
}


[System.Serializable]
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
