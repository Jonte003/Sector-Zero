using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float ShootDistance;
    protected GameObject target;
    protected Vector3 targetLocation;
    [SerializeField] protected bool whitinHitDistance;

    [SerializeField] protected bool clearLineOfSight;
    [SerializeField] protected float speed;
    protected LayerMask obstacles;
    protected PlayerStats playerStats;



    protected virtual void Start()
    {
        
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
        obstacles = LayerMask.GetMask("obstacle");
    }

    protected static bool CheckIfTargetInSight(Transform transform, Transform target, float detectionDistance, float maxAngle, Vector3 forward)
    {
        Vector3 targetDir = target.position - transform.position;

        bool targetWithinVision = Vector3.Angle(forward, targetDir) <= maxAngle;
        bool targetWithinRange = targetDir.sqrMagnitude <= detectionDistance * detectionDistance;

        return targetWithinRange && targetWithinVision;
    }
    protected static bool CheckIfPositionsInRange(Vector3 position1, Vector3 position2, float distance)
    {
        return (position1 - position2).sqrMagnitude < distance * distance;
    }
    protected static bool CheckIfLineOfSight(Transform from, Transform target, LayerMask layerMask)
    {
        Vector3 direction = target.position - from.position;
        float distance = direction.magnitude;

        return !Physics.Raycast(from.position, direction.normalized, distance, layerMask);
    }

    protected virtual void Update()
    {
        clearLineOfSight = CheckIfLineOfSight(transform, target.transform, obstacles);
        whitinHitDistance = CheckIfPositionsInRange(transform.position, target.transform.position, ShootDistance);
        targetLocation = target.transform.position;
    }
}
