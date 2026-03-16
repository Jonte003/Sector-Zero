using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class SimpleEnemyAI : EnemyAI
{

    private NavMeshAgent agent;
    private bool isPathStraight;
    [SerializeField] float maxAngleToConsiderTurn;
    [SerializeField] private State currentState = State.walking;
    [SerializeField] bool isStopped = false;

    [SerializeField] bool pathStraight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    static bool IsPathStraight(NavMeshAgent agent, float maxAngleToConsiderTurn) //Returns true if path has any turns that is not in Y dimension
    {
        if (agent.path.corners.Length <= 2)
        {
            return true;
        }
        else
        {

            for (int i = 1; i < agent.path.corners.Length - 1; i++)
            {
                Vector3 prevCorner = agent.path.corners[i - 1];
                Vector3 currentCorner = agent.path.corners[i];
                Vector3 nextCorner = agent.path.corners[i+1];

                Vector3 vectorIntoTurn = new Vector3(currentCorner.x - prevCorner.x, 0, currentCorner.z - prevCorner.z).normalized; 
                Vector3 vectorOutFromTurn = new Vector3(nextCorner.x - currentCorner.x, 0, nextCorner.z - currentCorner.z).normalized;

                float angle = Vector3.Angle(vectorIntoTurn, vectorOutFromTurn);

                if (angle > maxAngleToConsiderTurn) return false;
                
            }
            return true;
        }
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        agent.SetDestination(targetLocation);

        
        if (currentState == State.walking)
        {
            if (whitinShootDistance && clearLineOfSight)
            {
                if (whitinStopDistance)
                {
                    ChangeCurrentState(State.shootingWhitinStopDistance);
                }
                else
                {
                    ChangeCurrentState(State.shootingWalking);
                }


            }
        }
        
        else if (currentState == State.shootingWalking)
        {
            if (!clearLineOfSight) //Target no longer in sight, start walking towards target
            {
                ChangeCurrentState(State.walking);
            }
            else if (whitinStopDistance) //Stop and continue shooting
            {
                ChangeCurrentState(State.shootingWhitinStopDistance);
            }
        }
        else if (currentState == State.shootingWhitinStopDistance)
        {
            if (!whitinStopDistance && clearLineOfSight) //no longer whitin stopdistance but still line of sight
            {
                ChangeCurrentState(State.shootingWalking);
            }
            else if (!clearLineOfSight) //no longer whitin stopdistance AND no line of sight
            {
                ChangeCurrentState(State.walking);
            }
            else
            {
                //Code to turn towards target
                Vector3 lookDirection = target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }

        pathStraight = IsPathStraight(agent, maxAngleToConsiderTurn);
    }

    private void ChangeCurrentState(State newState)
    {

        bool flipStoppedState =
            (newState == State.shootingWhitinStopDistance) ^
            (currentState == State.shootingWhitinStopDistance);


        currentState = newState;

        if (flipStoppedState)
        {
            agent.isStopped = !agent.isStopped;
            isStopped = !agent.isStopped;
        }


        
    }

    protected enum State
    {
        shootingWalking, shootingWhitinStopDistance, walking
    }


}
