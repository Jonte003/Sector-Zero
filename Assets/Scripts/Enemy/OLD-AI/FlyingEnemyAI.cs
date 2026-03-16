using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class FlyingEnemyAI : EnemyAi
{
    Rigidbody rigidbody;
    [SerializeField] GameObject flyingNavNodes;
    NavNodeController navNodeController;

    [SerializeField] GameObject destinationNode;
    private NodeConnections nodeConnections;
    private GameObject currentPathStartNode;
    private GameObject previousPathStartNode;

    private float currentSpeed;
    private Vector3 direction;
    private Vector3 searchDirection;

    [SerializeField] float searchDirectionAngle;
    

    public override void Start()
    {
        navNodeController = flyingNavNodes.GetComponent<NavNodeController>();
        rigidbody = GetComponent<Rigidbody>();
        nodeConnections = destinationNode.GetComponent<NodeConnections>();
        currentSpeed = speed;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + searchDirection);
    }

    // Update is called once per frame
    public override void Update()
    {

        searchDirection = transform.rotation * Quaternion.Euler(searchDirectionAngle, 0, 0) * Vector3.forward * 5;


        if (currentActivity == CurrentActivity.Patroling)
        {
            direction = destinationNode.transform.position - transform.position;

            RotateTowardsTarget(destinationNode.transform.position, 5f);
            if (HasReachedDestionation(destinationNode.transform.position, transform.position, distanceToSetNewDestination))
            {
                previousPathStartNode = currentPathStartNode;
                currentPathStartNode = destinationNode;
                

                destinationNode = nodeConnections.GetRandomConnectedNode(previousPathStartNode);
                nodeConnections = destinationNode.GetComponent<NodeConnections>();
            }
            rigidbody.AddForce(direction.normalized * currentSpeed);
        }
    }
    private void RotateTowardsTarget(Vector3 target, float rotationSpeed)
    {
        Vector3 lookDirection = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    protected override void ChangeActivity(CurrentActivity currentActivity, CurrentActivity setActivity)
    {
        if (setActivity == CurrentActivity.Shooting)
            currentSpeed = 0;
        else if (currentActivity == CurrentActivity.Shooting)
            currentSpeed = speed;

        this.currentActivity = setActivity;

        if (setActivity == CurrentActivity.Patroling)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (setActivity == CurrentActivity.MovingToLastSeenLocation)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (setActivity == CurrentActivity.TargetInSight)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (setActivity == CurrentActivity.Shooting)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

}
