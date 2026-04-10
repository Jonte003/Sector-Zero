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

    Vector3 destination;
    protected override void Start()
    {
        
        base.Start();

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        currentState = State.walking;
        agent.radius = agentRadius;


        enemyStats = gameObject.GetComponent<GroundEnemyStats>();
        destination = targetLocation;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        whitinHitDistance = CheckIfPositionsInRange(transform.position, target.transform.position, reach);


        if (currentState == State.walking)
        {
            SetDestination(targetLocation);



            if (whitinHitDistance)
            {
                ChangeCurrentState(State.deelingDamage);
                animator.SetBool("Attacking", true);
                agent.isStopped = true;


                //if (destination != targetLocation)
                //    agent.SetDestination(transform.position);


            }
        }

        else if (currentState == State.deelingDamage)
        {

            if (!whitinHitDistance) 
            {
                ChangeCurrentState(State.walking);
                animator.SetBool("Attacking", false);

                agent.isStopped = false;
                SetDestination(targetLocation);

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

    void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    public void ConfirmDestination()
    {
        if (agent == null || target == null) return;
        agent.SetDestination(destination);
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
