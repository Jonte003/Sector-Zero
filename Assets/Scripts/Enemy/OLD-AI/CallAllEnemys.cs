using UnityEngine;

public class CallAllEnemys : MonoBehaviour
{
    GameObject enemyController;
    public float rangeToCall;
    public bool enemiesWasCalled = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = GameObject.FindGameObjectWithTag("EnemyController");
    }

    void OnCallAllEnemys()
    {
        if (enemyController == null)
        {
            Debug.Log("enemyController is null");
        }
        else
        {
            Debug.Log("enemyController is not null");

        }
        enemiesWasCalled = true;
        enemyController.GetComponent<EnemyController>().SetDestinationToAllChilds(transform.position, rangeToCall);
    }
}
