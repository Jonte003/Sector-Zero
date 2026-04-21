using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;




public class Attack : StateMachineBehaviour
{
    EnemyAI enemyAi;
    int lastAttackMade;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //Attack on state enter
    {
        if (enemyAi == null)
            enemyAi = animator.GetComponent<EnemyAI>();

        enemyAi.Attack();
        lastAttackMade = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //Attack on evry following anination loop
    {
        if (lastAttackMade + 1 == (int)stateInfo.normalizedTime)
        {
            lastAttackMade++;
            enemyAi.Attack();
        }
    }
}


