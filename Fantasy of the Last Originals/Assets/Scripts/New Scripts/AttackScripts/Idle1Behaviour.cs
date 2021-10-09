using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle1Behaviour : StateMachineBehaviour
{
    private CombatManagerScript _combatManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatManager = animator.GetComponent<CombatManagerScript>();
        animator.SetBool("Attacking", false);
        animator.SetBool("Attack 2", false);
        animator.SetBool("Attack 3", false);
        _combatManager._inputCount = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance1)
        //{
        //    animator.CrossFade("Stance 1 Blend Tree", .1f, 0, 0f);
        //}
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance2)
        //{
        //    animator.CrossFade("Stance 2 Blend Tree", .1f, 0, 0f);
        //}
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance3)
        //{
        //    animator.CrossFade("Stance 3 Blend Tree", .1f, 0, 0f);
        //}
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance4)
        //{
        //    animator.CrossFade("Stance 4 Blend Tree", .1f, 0, 0f);
        //}

        if (_combatManager.inputReceived && _combatManager._inputCount >= 1)
        {
            animator.SetBool("Attacking", true);

            if (_combatManager._player.Stance == PlayerStance.Stance1)
            {
                animator.CrossFade("S1 Attack 1", 0f, 0, 0f);
            }
            if (_combatManager._player.Stance == PlayerStance.Stance2)
            {
                animator.CrossFade("S2 Attack 1", 0f, 0, 0f);
            }
            if (_combatManager._player.Stance == PlayerStance.Stance3)
            {
                animator.CrossFade("S3 Attack 1", 0f, 0, 0f);
            }
            if (_combatManager._player.Stance == PlayerStance.Stance4)
            {
                animator.CrossFade("S4 Attack 1", 0f, 0, 0f);
            }

            _combatManager.ReceiveInput();
            _combatManager.inputReceived = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack 1", false);
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
