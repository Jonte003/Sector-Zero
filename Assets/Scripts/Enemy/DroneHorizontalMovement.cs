using UnityEngine;
using UnityEngine.AI;

public class DroneHorizontalMovement : EnemyAI
{
    NavMeshAgent agent;

    NavPointManager navPointManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        SnapToNavMesh();


        navPointManager = GameObject.FindWithTag("NavNodes").GetComponent<NavPointManager>();
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
    protected override void Update()
    {
        base.Update();


        if (CheckIfLineOfSight(transform.position, targetLocation, obstacles)) 
        {
            MoveTowardsTarget();
        }
        else
        {
            MoveToNode();
            
        }


        FaceTarget();
    } 

    private void MoveTowardsTarget()
    {
        Vector3 toDrone = transform.position - targetLocation;
        toDrone.y = 0;

        Vector3 dir = toDrone.normalized;

        Vector3 targetPos = targetLocation + dir * ShootDistance;
        targetPos.y = transform.position.y; // keep height to navmesh

        agent.SetDestination(targetPos);

    }

    private void MoveToNode()
    {
        Vector3 destination = navPointManager.GetClosestNode(transform.position);
        destination.y = transform.position.y; // keep height to navmesh
        agent.SetDestination(destination);
    }

    private void FaceTarget()
    {
        Vector3 lookDirection = target.transform.position - transform.position;
        lookDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }




}
