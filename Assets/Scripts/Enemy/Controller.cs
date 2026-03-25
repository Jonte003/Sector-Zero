using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    PlayerStats playerStats;
    List<Transform> allEnemies;
    void Start()
    {
        allEnemies = new List<Transform>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTransformChildrenChanged()
    {
        AddAllChilds();
    }

    void AddAllChilds()
    {
        allEnemies.Clear();
        foreach (Transform child in transform)
        {
            allEnemies.Add(child);
        }
    }

    public void AddExperiece(float amount)
    {
        playerStats.AddExperience(amount);
    }
}
