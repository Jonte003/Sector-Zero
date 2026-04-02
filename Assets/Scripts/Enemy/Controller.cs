using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    PlayerLevels playerLevels;
    List<Transform> allEnemies;
    void Start()
    {
        allEnemies = new List<Transform>();
        playerLevels = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevels>();
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
            if (child.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                allEnemies.Add(child);
            }
        }

    }

    public void AddExperiece(float amount)
    {
        playerLevels.AddExperience(amount);
    }
}
