using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerInput playerInput;
    private Loadout loadout;
    private Controller enemyController;
    void Start()
    {
        loadout = GetComponent<Loadout>();

        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<Controller>();

        List<GameObject> allEnemies = TransformListToGameObjects(enemyController.AllEnemies);

        playerInput = new();
        playerInput.Enable();

        playerInput.PlayerActions.Ability1.performed += ctx => loadout.Ability1.Run(gameObject, allEnemies, this);
        playerInput.PlayerActions.Ability2.performed += ctx => loadout.Ability2.Run(gameObject, allEnemies, this);
        playerInput.PlayerActions.Ability3.performed += ctx => loadout.Ability3.Run(gameObject, allEnemies, this);
    }

    private List<GameObject> TransformListToGameObjects(List<Transform> transforms)
    {
        List<GameObject> gameObjects = new();
        foreach (Transform t in transforms)
        {
            gameObjects.Add(t.gameObject);
        }
        return gameObjects;
    }
}