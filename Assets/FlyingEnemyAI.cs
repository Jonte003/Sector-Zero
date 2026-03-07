using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class FlyingEnemyAI : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] GameObject flyingNavNodes;
    NavNodeController navNodeController;

    [SerializeField] GameObject destinationNode;
    private NodeConnections nodeConnections;
    private GameObject currentPathStartNode;
    private GameObject previousPathStartNode;

    [SerializeField] CurrentActivity currentActivity = CurrentActivity.Patroling;

    private Vector3 direction;
    private Vector3 searchDirection;
    [SerializeField] float searchDirectionAngle;
    
    [SerializeField] float speed = 5f;

    void Start()
    {
        navNodeController = flyingNavNodes.GetComponent<NavNodeController>();
        rigidbody = GetComponent<Rigidbody>();
        nodeConnections = destinationNode.GetComponent<NodeConnections>();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + searchDirection);
    }

    // Update is called once per frame
    void Update()
    {

        searchDirection = Quaternion.Euler(0, 0, searchDirectionAngle) * transform.forward * 5;


        if (currentActivity == CurrentActivity.Patroling)
        {
            direction = destinationNode.transform.position - transform.position;

            RotateTowardsTarget(destinationNode.transform.position, 5f);
            if (HasReachedDestionation(destinationNode.transform.position, transform.position, 1))
            {
                previousPathStartNode = currentPathStartNode;
                currentPathStartNode = destinationNode;
                

                destinationNode = nodeConnections.GetRandomConnectedNode(previousPathStartNode);
                nodeConnections = destinationNode.GetComponent<NodeConnections>();
            }
            rigidbody.AddForce(direction.normalized * speed);
        }
    }
    private void RotateTowardsTarget(Vector3 target, float rotationSpeed)
    {
        Vector3 lookDirection = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    static bool HasReachedDestionation(Vector3 destionation, Vector3 position, float distanceToCompare)
    {
        return (position - destionation).sqrMagnitude < distanceToCompare * distanceToCompare;
    }
    public enum CurrentActivity
    {
        Patroling, MovingToLastSeenLocation, TargetInSight, Shooting
    }
}
