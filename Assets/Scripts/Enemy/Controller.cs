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
            if (child.gameObject.layer == LayerMask.NameToLayer("enemy") || child.GetChild(0).gameObject.layer == LayerMask.NameToLayer("enemy")
)
            {
                allEnemies.Add(child);
            }
        }

    }

    public void SetDestinations()
    {
        foreach (Transform enemy in allEnemies)
        {
            if (enemy.TryGetComponent<SimpleEnemyAI>(out var ai))
            {
                ai.ConfirmDestination(); //Confirm destination for ground enemy
            }
            else
            {
                enemy.GetComponent<DroneHorizontalMovement>().ConfirmDestination(); //Confirm destination for drone
            }

        }

    }

    public void AddExperiece(float amount)
    {
        playerLevels.AddExperience(amount);
    }
}
