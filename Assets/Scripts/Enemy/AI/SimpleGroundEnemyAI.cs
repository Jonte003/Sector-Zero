using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class SimpleEnemyAI : EnemyAI
{

    private NavMeshAgent agent;
    [SerializeField] private State currentState;
    private EnemyStats enemyStats;

    [SerializeField] float agentRadius;


    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        currentState = State.walking;
        agent.radius = agentRadius;


        enemyStats = gameObject.GetComponent<EnemyStats>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


        if (currentState == State.walking)
        {
            agent.SetDestination(targetLocation);


            if (whitinHitDistance)
            {
                ChangeCurrentState(State.deelingDamage);
            }
        }

        else if (currentState == State.deelingDamage)
        {

            if (!whitinHitDistance) 
            {
                ChangeCurrentState(State.walking);
            }
            else
            {
                enemyStats.DoDamageToTarget();

                //Code to turn towards target
                Vector3 lookDirection = target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }



        







    }

    private void ChangeCurrentState(State newState)
    {
        currentState = newState;

        if (currentState == State.walking)
        {
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
        }
    }


    protected enum State
    {
         deelingDamage, walking
    }


}
