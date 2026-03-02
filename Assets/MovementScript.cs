using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class MovementScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField] float ShootDistance;
    [SerializeField] float distanceToSetNewDestination;
    private Vector3 destination;
    private NavMeshAgent agent;
    private bool destinationReached = true;
    private GameObject target;
    public DestinationType destinationType;
    public bool isPatroling;
    [SerializeField] bool targetInSight; //Should be private

    [SerializeField] float patrolSpeed;
    [SerializeField] float targetInSightSpeed;

    [SerializeField] float enemyFOV;
    [SerializeField] float viewDistance;
    private bool HasLineOfSight;
    private LayerMask obstacles;
    static Vector3 GetRandomDestination(Vector3 pos1, Vector3 pos2)
    {
        return new Vector3(
            Random.Range(pos1.x, pos2.x),
            Random.Range(pos1.y, pos2.y),
            Random.Range(pos1.z, pos2.z)
            );
    }
    static bool HasReachedDestionation(Vector3 destionation, Vector3 position, float distanceToCompare)
    {
        Vector3 destionationWithoutY = new Vector3(destionation.x, 0, destionation.z);
        Vector3 positionWithoutY = new Vector3(position.x, 0, position.z);
        Vector3 delta = positionWithoutY - destionationWithoutY;
        return delta.sqrMagnitude < distanceToCompare * distanceToCompare;
    }

    static bool CheckIfTargetInSight(Transform transform, Transform target, float detectionDistance, float maxAngle)
    {
        Vector3 forward = transform.forward;
        Vector3 targetDir = target.position - transform.position;

        bool targetWithinVision = Vector3.Angle(forward, targetDir) <= maxAngle;
        bool targetWithinRange = targetDir.sqrMagnitude <= detectionDistance * detectionDistance;

        return targetWithinRange && targetWithinVision;
    }
    static bool CheckIfPositionsInRange(Vector3 position1, Vector3 position2, float distance)
    {
        return (position1 - position2).sqrMagnitude < distance * distance;
    }
    
    static bool CheckIfLineOfSight(Transform from, Transform target, LayerMask layerMask)
    {
        Vector3 direction = target.position - from.position;
        float distance = direction.magnitude;

        return !Physics.Raycast(from.position, direction.normalized, distance, layerMask);
    } 

    public void SetDestinationFromOutside(Vector3 location, float ifWhitinThisRange) //set destination to location if location is in whitin range
    {
        if (CheckIfPositionsInRange(transform.position, location, ifWhitinThisRange)) 
        {
            destination = location;

            agent.SetDestination(destination);
            agent.destination = destination;

            destinationType = DestinationType.LastSeenLocation;

        }
    } 

    void SetDestination(Vector3 destination)
    {
        this.destination = destination;

        agent.SetDestination(destination);
        agent.destination = destination;

        destinationType = DestinationType.TargetLocation;
    }

    void SetPatrolDestination(Vector3 destination)
    {
        this.destination = destination;

        agent.SetDestination(destination);
        agent.destination = destination;
        destinationType = DestinationType.RandomLocation;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        obstacles = LayerMask.GetMask("obstacle");
        agent.speed = patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (destinationReached)
        {
            SetPatrolDestination(GetRandomDestination(pos1, pos2));

            destinationReached = false;
        }

        if (HasReachedDestionation(destination, transform.position, distanceToSetNewDestination) || !agent.hasPath)
        {
            destinationReached = true;
        }

        targetInSight = CheckIfTargetInSight(transform, target.transform, viewDistance, enemyFOV * 0.5f);
        
 
        if (targetInSight && CheckIfPositionsInRange(transform.position, target.transform.position, ShootDistance) && (CheckIfLineOfSight(transform, target.transform, obstacles))) //If target is in sight AND target is whitin fire range
        {
            destination = target.transform.position;

            
            SetDestination(destination);

            //Code to turn towards target
            Vector3 lookDirection = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            agent.isStopped = true;

        }
        else if (targetInSight && (CheckIfLineOfSight(transform, target.transform, obstacles))) //If target is in sight
        {
            destination = target.transform.position;

            SetDestination(destination);
            agent.isStopped = false;


        }
        else if (!targetInSight && destinationType != DestinationType.RandomLocation) 
        {
            destinationType = DestinationType.LastSeenLocation;
            agent.isStopped = false;

        }
        else //Patroling
        {

            agent.isStopped = false;

        }


        if (destinationType == DestinationType.RandomLocation) //Patrol
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (destinationType == DestinationType.LastSeenLocation) //Walking to last seen location
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (destinationType == DestinationType.TargetLocation && CheckIfPositionsInRange(transform.position, target.transform.position, ShootDistance) && (CheckIfLineOfSight(transform, target.transform, obstacles))) //Whitin Shooting range && target in sight
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (destinationType == DestinationType.TargetLocation) //target in sight
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }


    }
}

public enum DestinationType
{
    RandomLocation, LastSeenLocation, TargetLocation
}