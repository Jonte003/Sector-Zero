using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.XR;
using System.Collections;
using System.Threading;
public class SimpleEnemyAI : EnemyAI
{
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    [SerializeField] private State currentState;
    private GroundEnemyStats enemyStats;
    Animator animator;

    [SerializeField] protected bool whitinHitDistance;

    Vector3 destination;

    [SerializeField] float gravityMultiplier = 1.5f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundCheckDistance = 0.2f;

    float verticalVelocity = 0f;
    bool isGrounded = false;

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundMask);
    }

    private void ApplyGravity()
    {
        verticalVelocity -= 9.81f * gravityMultiplier * Time.deltaTime;
        Vector3 pos = transform.position;
        pos.y += verticalVelocity * Time.deltaTime;
        transform.position = pos;
    }

    public override void ApplyKnockback()
    {
        isGrounded = CheckGrounded();
        if (!isGrounded)
        {
            return;
        }

        agent.enabled = false;
        rigidbody.isKinematic = false;

        StartCoroutine(KnockbackRecovery());

    }

    private IEnumerator KnockbackRecovery()
    {
        float timer = 1;

        while (isGrounded || timer > 0)
        {
            timer -= Time.deltaTime;
            ApplyGravity();
            isGrounded = CheckGrounded();
            yield return null;
        }

        while (!isGrounded)
        {
            ApplyGravity();
            isGrounded = CheckGrounded();
            yield return null;

        }

        //Code runs when object has left the ground then landed again
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.isKinematic = true;
        agent.enabled = true;

        agent.Warp(GetClosestPositionOnMesh(transform.position));

    }


    protected override void Start()
    {
        
        base.Start();

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        agent.speed = speed;
        currentState = State.walking;
    

        enemyStats = gameObject.GetComponent<GroundEnemyStats>();
        destination = targetLocation;

    }

    // Update is called once per frame
    public override void CalculatePath()
    {

        if (isStunned)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        targetLocation = target.transform.position;

        whitinHitDistance = CheckIfPositionsInRange(transform.position, target.transform.position, reach);


        if (currentState == State.walking)
        {
            agent.SetDestination(targetLocation);



            if (whitinHitDistance)
            {
                ChangeCurrentState(State.deelingDamage);
                animator.SetBool("Attacking", true);
                agent.isStopped = true;




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

            }
        }



        







    }


    private void Update() 
    {
        if (currentState == State.deelingDamage)
        {
            enemyStats.DoDamageToTarget();

            Vector3 lookDirection = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
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

    public override void Slow(float duration, float slowAmount)
    {
        StartCoroutine(SlowAgent(duration, slowAmount));
    }

    private IEnumerator SlowAgent(float duration, float slowAmount)
    {
        agent.speed = speed * slowAmount;
        yield return new WaitForSeconds(duration);
        agent.speed = speed;
    }

    private Vector3 GetClosestPositionOnMesh(Vector3 pos)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(pos,out hit,10f,   NavMesh.AllAreas
        );
        return hit.position;
    }

    public override void Stun(float seconds)
    {
        StartCoroutine(StunAgent(seconds));
    }

    private IEnumerator StunAgent(float duration)
    {
        isStunned = true;

        agent.isStopped = true;
        agent.updateRotation = false;
        animator.speed = 0;

        yield return new WaitForSeconds(duration);

        agent.Warp(transform.position);

        animator.speed = 1;
        isStunned = false;
        agent.isStopped = false;
        agent.updateRotation = true;

        CalculatePath();
    }






    protected enum State
    {
         deelingDamage, walking
    }


}
