using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.XR;
using System.Collections;
using System.Threading;
public class SimpleEnemyAI : EnemyAgentAI
{
    NavMeshObstacle obstacle;
    [SerializeField] private State currentState;
    private GroundEnemyStats enemyStats;
    Animator animator;

    [SerializeField] protected bool whitinHitDistance;

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

        ChangeCurrentState(State.walking);
        isStunned = true;
        agent.enabled = false;
        rigidbody.isKinematic = false;

        StartCoroutine(KnockbackRecovery());

    }

    private IEnumerator KnockbackRecovery()
    {
        float timer = 1;

        while (isGrounded && timer > 0)
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

        Vector3 navPos = GetClosestPositionOnMesh(transform.position);
        transform.position = navPos;

        agent.enabled = true;
        agent.Warp(navPos);

        isStunned = false;
        CalculatePath();

    }

    protected void CheckSpeed()
    {
        if (!CheckIfPositionsInRange(targetLocation, transform.position, distanceStartRun))
        {
            ChangeCurrentState(State.running);
        }
    }

    protected override void Start()
    {
        
        base.Start();

        animator = GetComponent<Animator>();

        currentState = State.walking;
    

        enemyStats = gameObject.GetComponent<GroundEnemyStats>();
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
            CheckSpeed();

            if (whitinHitDistance)
            {
                ChangeCurrentState(State.deelingDamage);
                animator.SetBool("Attacking", true);
                agent.isStopped = true;
            }
        }

        else if (currentState == State.running)
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

            Vector3 lookDirection = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

    }

    private void ChangeCurrentState(State newState)
    {
        currentState = newState;

        if (newState == State.walking)
        {
            agent.isStopped = false;
            agent.speed = walkSpeed;
        }
        else if (newState == State.running)
        {
            agent.isStopped = false;
            agent.speed = runSpeed;
        }
        else if (newState == State.deelingDamage)
        {
            agent.isStopped = true;
            agent.speed = 0;
        }
    }

    public override void Slow(float duration, float slowAmount)
    {
        StartCoroutine(SlowAgent(duration, slowAmount));
    }

    private IEnumerator SlowAgent(float duration, float slowAmount)
    {
        float currentSpeed = agent.speed;
        agent.speed = currentSpeed * slowAmount;
        yield return new WaitForSeconds(duration);
        agent.speed = currentSpeed;
    }

    private Vector3 GetClosestPositionOnMesh(Vector3 pos)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(pos,out hit,10f,   NavMesh.AllAreas
        );

        if (hit.position == Vector3.zero) //If no position is found search by Vector3.zero
        {
            GetClosestPositionOnMesh(hit.position);
        }
        return hit.position;
    }

    public override void Attack()
    {
        enemyStats.DoDamageToTarget();
        Debug.Log("Attack ran");
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
         deelingDamage, walking, running
    }


}
