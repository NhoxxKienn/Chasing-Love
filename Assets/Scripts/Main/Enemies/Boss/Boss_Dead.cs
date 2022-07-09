using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Dead : StateMachineBehaviour
{
    private BossController bossController;
    private float deadTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossController = animator.gameObject.GetComponent<BossController>();
        deadTimer = 2f;
        bossController.bossDeathSFX.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        deadTimer -= Time.deltaTime;
        animator.gameObject.transform.Translate(Vector2.down * 2 * Time.deltaTime);
        if (deadTimer < 0)
        {
            
            Destroy(animator.gameObject);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

}
