using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerInput playerInput;
    private Loadout loadout;
    private Controller enemyController;

    private List<Transform> AllEnemies => enemyController.AllEnemies; 

    void Start()
    {
        loadout = GetComponent<Loadout>();

        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<Controller>();

        playerInput = new();
        playerInput.Enable();

        playerInput.PlayerActions.Ability1.performed += ctx => loadout.Ability1.Run(gameObject, AllEnemies, this);
        playerInput.PlayerActions.Ability2.performed += ctx => loadout.Ability2.Run(gameObject, AllEnemies, this);
        playerInput.PlayerActions.Ability3.performed += ctx => loadout.Ability3.Run(gameObject, AllEnemies, this);
    }
}