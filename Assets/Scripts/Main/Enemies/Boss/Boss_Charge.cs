using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Charge : StateMachineBehaviour
{
    private BossController bossController;
    private float leftBorder = -3f;
    private float rightBorder = 7.5f;
    private float chargeSpeed = 19f;
    private bool chargeAttack = true;
    private bool stopMoving;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stopMoving = false;
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
            if (animator.gameObject.transform.position.x <= rightBorder && !stopMoving)
            {
                animator.gameObject.transform.Translate(Vector2.right * chargeSpeed * 1.5f  * Time.deltaTime);
            }
            else
            {
                stopMoving=true;
                animator.gameObject.transform.position = new Vector3(7.5f, -1.5f, 0);
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
