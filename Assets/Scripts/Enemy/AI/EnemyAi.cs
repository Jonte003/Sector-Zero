using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float reach;
    private EnemyType enemyType;
    protected Rigidbody rigidbody;
    protected GameObject target;
    protected Vector3 targetLocation;
    
    protected LayerMask obstacles;
    protected PlayerStats playerStats;


    protected virtual void Start()
    {

        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
        rigidbody = GetComponent<Rigidbody>();
        obstacles = LayerMask.GetMask("obstacle");

        if (CompareTag("Drone"))
        {
            enemyType = EnemyType.Drone;
        }
        else if (CompareTag("GroundEnemy"))
        {
            enemyType = EnemyType.GroundEnemy;
        }
    }

    public static bool CheckIfTargetInSight(Transform transform, Transform target, float detectionDistance, float maxAngle, Vector3 forward)
    {
        Vector3 targetDir = target.position - transform.position;

        bool targetWithinVision = Vector3.Angle(forward, targetDir) <= maxAngle;
        bool targetWithinRange = targetDir.sqrMagnitude <= detectionDistance * detectionDistance;

        return targetWithinRange && targetWithinVision;
    }
    public static bool CheckIfPositionsInRange(Vector3 position1, Vector3 position2, float distance)
    {
        return (position1 - position2).sqrMagnitude < distance * distance;
    }
    public static bool CheckIfLineOfSight(Vector3 from, Vector3 target, LayerMask layerMask)
    {
        Vector3 direction = target - from;
        float distance = direction.magnitude;

        return !Physics.Raycast(from, direction.normalized, distance, layerMask);
    }
    
    public virtual void Attack() { }
}

public enum EnemyType
{
    Drone,
    GroundEnemy
}

