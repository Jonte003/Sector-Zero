using UnityEngine;
using System.Collections;




public class Attack : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EnemyShoot>().ShootWithDelay();
    }
}


