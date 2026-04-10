using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using System.Collections;

public class DroneHorizontalMovement : EnemyAI
{
    Animator animator;
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    Transform droneBody;
    NavPointManager navPointManager;

    [SerializeField] float lineOfSightCheckYOffset;
    Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        agent.updateRotation = false;
        
        SnapToNavMesh();
        droneBody = transform.GetChild(0);
        if (transform.childCount != 1)
        {
            throw new System.Exception("Drone only accepts exactly 1 child");
        }
        animator = droneBody.GetComponent<Animator>();




navPointManager = GameObject.FindWithTag("NavNodes").GetComponent<NavPointManager>();
        destination = targetLocation;
    }

    public void SnapToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 40f, NavMesh.GetAreaFromName("DronePlane"))) //Snap to navmesh
        {
            agent.Warp(hit.position);
        }
    }

    // Update is called once per frame
    public override void CalculatePath()
    {
        targetLocation = target.transform.position;
        
        if (isStunned)
        {
            return;
        }
        if (CheckIfLineOfSight(droneBody.position, targetLocation, obstacles))
        {
            
            MoveTowardsTarget();
        }
        else
        {
            MoveToClosestNode();
        }
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



    private void MoveTowardsTarget()
    {
        Vector3 toDrone = transform.position - targetLocation;
        toDrone.y = 0;

        Vector3 dir = toDrone.normalized;

        Vector3 targetPos = targetLocation + dir * reach;
        targetPos.y = transform.position.y; // keep height to navmesh

        agent.SetDestination(targetPos);

    }

    private void MoveToClosestNode()
    {
        Vector3 destination = navPointManager.GetClosestNode(transform.position);
        destination.y = transform.position.y; // keep height to navmesh
        agent.SetDestination(destination);
    }

    private void MoveToRandomNode()
    {
        Vector3 destination = navPointManager.GetRandomAvaliableNode();
        destination.y = transform.position.y; // keep height to navmesh
        agent.SetDestination(destination);
    }

    private void FaceTarget()
    {
        Vector3 lookDirection = target.transform.position - transform.position;
        lookDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    public bool ReachedDestination()
    {
        
        Vector3 toDest = destination - transform.position;
        return toDest.sqrMagnitude < 50f;
    }




}
