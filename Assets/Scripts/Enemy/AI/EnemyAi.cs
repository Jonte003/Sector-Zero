using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float reach;
    protected GameObject target;
    protected Vector3 targetLocation;

    [SerializeField] protected float speed;
    protected LayerMask obstacles;
    protected PlayerStats playerStats;

    protected bool isStunned;
    float stunTimer;

    protected virtual void Start()
    {
        
        target = GameObject.FindWithTag("Player");
        playerStats = target.GetComponent<PlayerStats>();
        obstacles = LayerMask.GetMask("obstacle");
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

    public virtual void CalculatePath() { }
    public virtual void Stun(float seconds) { }
    public virtual void Slow(float duration, float slowAmount) { }




}
