using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class EnemyAgentAI : EnemyAI
{
    [Space]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField,Tooltip("The distance the enemy has to be from player to start running, running will stop after doing damage to player")] protected float distanceStartRun;

    protected NavMeshAgent agent;

    protected bool isStunned;
    float stunTimer;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;


    }



    public virtual void CalculatePath() { }
    public virtual void ApplyKnockback() { }
    public virtual void Stun(float duration) { }
    public virtual void Slow(float duration, float slowAmount) { }

}
