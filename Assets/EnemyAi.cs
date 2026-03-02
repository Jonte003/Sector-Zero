using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAi : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float ShootDistance;
    [SerializeField] float distanceToSetNewDestination;
    public Vector3 destination;
    private NavMeshAgent agent;
    private GameObject target;
    private EnemyController enemyControllerScript;

    public CurrentActivity currentActivity;
    bool targetInSight;

    [SerializeField] float speed;

    [SerializeField] float enemyFOV;
    [SerializeField] float viewDistance;
    private LayerMask obstacles;


    static bool HasReachedDestionation(Vector3 destionation, Vector3 position, float distanceToCompare)
    {
        return (position-destionation).sqrMagnitude < distanceToCompare * distanceToCompare;
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
            if (currentActivity == CurrentActivity.Patroling || currentActivity == CurrentActivity.MovingToLastSeenLocation)
            {
                ChangeActivity(currentActivity, CurrentActivity.MovingToLastSeenLocation);
                destination = location;

                agent.SetDestination(destination);
                agent.destination = destination;
            }
        }
    }
    


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player");
        obstacles = LayerMask.GetMask("obstacle");
        agent.speed = speed;
        enemyControllerScript = transform.parent.GetComponent<EnemyController>();
    }

    public void Update()
    {
        targetInSight = CheckIfTargetInSight(transform, target.transform, viewDistance, enemyFOV) && CheckIfLineOfSight(transform, target.transform, obstacles);

        if (currentActivity == CurrentActivity.Patroling)
        {

            if (targetInSight)
            {
                Debug.Log("Target in sight");
                if (CheckIfPositionsInRange(target.transform.position, transform.position, ShootDistance)) //Enemy is spotted and whitin shoot distance
                {
                    Debug.Log("Target whitin shoot distance");
                    ChangeActivity(currentActivity, CurrentActivity.Shooting);
                }
                else //Enemy is spotted but not whitin shooting distance
                {
                    Debug.Log("Target NOT whitin shoot distance");
                    ChangeActivity(currentActivity, CurrentActivity.TargetInSight);
                }
            }
            else if (!agent.hasPath || HasReachedDestionation(destination, transform.position, distanceToSetNewDestination)) //Patrol Destintion Reached
            {
                destination = enemyControllerScript.GetRandomNavNodePos();
                agent.SetDestination(destination);
                agent.destination = destination;
            }
            else
            {

            }
        }


        else if (currentActivity == CurrentActivity.MovingToLastSeenLocation)
        {
            if (targetInSight)
            {
                if (CheckIfPositionsInRange(target.transform.position, transform.position, ShootDistance)) //Enemy is spotted and whitin shoot distance
                {
                    ChangeActivity(currentActivity, CurrentActivity.Shooting);
                }
                else //Enemy is spotted but not whitin shooting distance
                {
                    ChangeActivity(currentActivity, CurrentActivity.TargetInSight);
                }
            }
            else if (!agent.hasPath || HasReachedDestionation(destination, transform.position, distanceToSetNewDestination)) //Last Seen Location Reached
            {
                ChangeActivity(currentActivity, CurrentActivity.Patroling);
                destination = enemyControllerScript.GetRandomNavNodePosForwards(transform.forward, transform.position, 45f);
                agent.SetDestination(destination);
                agent.destination = destination;
            }
        }


        else if (currentActivity == CurrentActivity.TargetInSight)
        {
            if (targetInSight)
            {
                if (CheckIfPositionsInRange(transform.position, target.transform.position, ShootDistance)) //Eneny whitin ShootDistance
                {
                    ChangeActivity(currentActivity, CurrentActivity.Shooting);
                }
            }
            else //Target no longer in sight
            {
                ChangeActivity(currentActivity, CurrentActivity.MovingToLastSeenLocation);
            }
            SetDestionationToPlayer(); //Set destination to target regardless of outcome
        }


        else if (currentActivity == CurrentActivity.Shooting)
        {
            if (targetInSight && CheckIfPositionsInRange(transform.position, target.transform.position, ShootDistance)) //Continue Shooting
            {
                //Code to turn towards target
                Vector3 lookDirection = target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
            else //Enemy no longer in Shooting Range
            {
                if (targetInSight) //Target is still visible
                    ChangeActivity(currentActivity, CurrentActivity.TargetInSight);
                else //Target is not visible
                    ChangeActivity(currentActivity, CurrentActivity.MovingToLastSeenLocation);

              
            }
            SetDestionationToPlayer(); //Set destination to target regardless of outcome
        }
    }

    private void ChangeActivity(CurrentActivity currentActivity, CurrentActivity setActivity)
    {
        if (setActivity == CurrentActivity.Shooting)
            agent.isStopped = true;
        else if(currentActivity == CurrentActivity.Shooting)
            agent.isStopped = false;

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


    private void SetDestionationToPlayer()
    {
        destination = target.transform.position;
        agent.SetDestination(destination);
        agent.destination = destination;
    }

    public enum CurrentActivity
    {
        Patroling, MovingToLastSeenLocation, TargetInSight, Shooting
    }
}
