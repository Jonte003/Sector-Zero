using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.XR;
using System.Collections;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
public class GroundEnemyAI : EnemyAgentAI
{
    NavMeshObstacle obstacle;
    [SerializeField] private State currentState;
    private GroundEnemyStats enemyStats;
    Animator animator;
    [SerializeField] float onLinkSpeed;
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

        isStunned = true;
        ChangeCurrentState(State.walking);
        agent.enabled = false;
        rigidbody.isKinematic = false;
        

        StartCoroutine(KnockbackRecovery());

    }

    private IEnumerator KnockbackRecovery()
    {
        float timer = 1;
        float timer2 = 4;

        while (isGrounded && timer > 0)
        {
            timer -= Time.deltaTime;
            ApplyGravity();
            isGrounded = CheckGrounded();
            yield return null;
        }

        while (!isGrounded && timer2 > 0)
        {
            timer2 -= Time.deltaTime;
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
            }
        }

        else if (currentState == State.running)
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

            }
        }
    }

    IEnumerator OnLink(OffMeshLinkData data) //Smoothen transition from wall to ground
    {
        Vector3 startpos = data.startPos;
        Vector3 endpos = data.endPos;
        float distance = Vector3.Distance(startpos, endpos);
        float speed = distance / onLinkSpeed;

        Vector3 direction = (endpos - startpos).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);



        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / speed;
            agent.transform.position = Vector3.Lerp(startpos, endpos, time);
            agent.transform.rotation = Quaternion.Slerp(
                agent.transform.rotation,
                targetRotation,
                Time.deltaTime * 5); //rotationspeed
            yield return null;
        }

        agent.CompleteOffMeshLink();
    }

    private void Update()
    {
        if (currentState == State.deelingDamage)
        {

            Vector3 lookDirection = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(OnLink(agent.currentOffMeshLinkData));
        }

    }

    private void ChangeCurrentState(State newState)
    {
        currentState = newState;

        if (newState == State.walking)
        {
            agent.isStopped = false;
            agent.speed = walkSpeed;
            animator.SetBool("Attacking", false);
            agent.SetDestination(targetLocation);


        }
        else if (newState == State.running)
        {
            agent.isStopped = false;
            agent.speed = runSpeed;
            animator.SetBool("Attacking", false);
            agent.SetDestination(targetLocation);

        }
        else if (newState == State.deelingDamage)
        {
            agent.isStopped = true;
            agent.speed = 0;
            animator.SetBool("Attacking", true);

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
        NavMesh.SamplePosition(pos, out hit, 10f, NavMesh.AllAreas
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