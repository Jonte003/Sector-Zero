using UnityEngine;

public class GameController : MonoBehaviour
{

    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
