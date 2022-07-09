using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Charge : StateMachineBehaviour
{
    private BossController bossController;
    private float leftBorder = -3f;
    private float rightBorder = 7f;
    private float chargeSpeed = 20f;
    private bool chargeAttack = true;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chargeAttack = true;
        bossController = animator.gameObject.GetComponent<BossController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (chargeAttack && animator.gameObject.transform.position.x > leftBorder)
        {
            animator.gameObject.transform.Translate(Vector2.left * chargeSpeed * Time.deltaTime);
        }
        else
        {
            chargeAttack = false;
            if (animator.gameObject.transform.position.x <= rightBorder)
            {
                animator.gameObject.transform.Translate(Vector2.right * chargeSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetTrigger("isIdle");  
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossController.isIdle = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
