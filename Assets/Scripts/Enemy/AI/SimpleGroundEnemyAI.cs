using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class SimpleEnemyAI : EnemyAI
{

    private NavMeshAgent agent;
    [SerializeField] private State currentState;
    private GroundEnemyStats enemyStats;
    Animator animator;

    [SerializeField] float agentRadius;
    [SerializeField] protected bool whitinHitDistance;


    protected override void Start()
    {
        
        base.Start();

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        currentState = State.walking;
        agent.radius = agentRadius;


        enemyStats = gameObject.GetComponent<GroundEnemyStats>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        whitinHitDistance = CheckIfPositionsInRange(transform.position, target.transform.position, reach);


        if (currentState == State.walking)
        {
            agent.SetDestination(targetLocation);




            if (whitinHitDistance)
            {
                ChangeCurrentState(State.deelingDamage);
                animator.SetBool("Attacking", true);
                agent.isStopped = true;
                agent.SetDestination(transform.position);
            }
        }

        else if (currentState == State.deelingDamage)
        {

            if (!whitinHitDistance) 
            {
                ChangeCurrentState(State.walking);
                animator.SetBool("Attacking", false);

                agent.isStopped = false;
                agent.SetDestination(targetLocation);

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
