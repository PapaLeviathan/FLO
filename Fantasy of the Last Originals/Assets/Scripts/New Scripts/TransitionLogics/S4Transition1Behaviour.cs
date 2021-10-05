using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S4Transition1Behaviour : StateMachineBehaviour
{
    private CombatManagerScript _combatManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatManager = animator.GetComponent<CombatManagerScript>();

        if (stateInfo.IsTag("Attack"))
        {
            var attackScreamHandler = animator.GetComponent<AttackScreamHandler>();
            var rand = Random.Range(0, 3);

            if (rand == 1)
            {
                var rand2 = Random.Range(0, attackScreamHandler.Clips.Length);
                attackScreamHandler.PlayScreamSound(rand2);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Play attack 3 as well if attack key was hit three times
        if (_combatManager._inputCount == 3)
        {
            animator.SetBool("Attack 3", true);
        }

        if (_combatManager.inputReceived && _combatManager._inputCount >= 2)
        {
            animator.SetBool("Attack 2", true);

            //Only transition to next attack if Attack 2 is true, you are in Transition 1 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (animator.GetBool("Attack 2") && stateInfo.IsName("S4 Transition 1"))
            {
                if (animator.GetBool("Stance1"))
                {
                    animator.CrossFade("S1 Attack 2", .25f, 0, 0f);
                }
                if (animator.GetBool("Stance2"))
                {
                    animator.CrossFade("S2 Attack 2", .25f, 0, 0f);
                }
                if (animator.GetBool("Stance3"))
                {
                    animator.CrossFade("S3 Attack 2", .25f, 0, 0f);
                }
                if (animator.GetBool("Stance4"))
                {
                    animator.CrossFade("S4 Attack 2", .25f, 0, 0f);
                }

                _combatManager.ReceiveInput();
                _combatManager.inputReceived = false;
            }
        }

        if (!animator.GetBool("Attack 2") && stateInfo.IsName("S4 Transition 1") && stateInfo.normalizedTime > _combatManager.ReturnTransitionSpeed1(_combatManager._player.Stance))
        {
            if (animator.GetBool("Stance1"))
                animator.CrossFadeInFixedTime("Stance 1 Blend Tree", .25f, 0);
            if (animator.GetBool("Stance2"))
                animator.CrossFadeInFixedTime("Stance 2 Blend Tree", .25f, 0);
            if (animator.GetBool("Stance3"))
                animator.CrossFadeInFixedTime("Stance 3 Blend Tree", .25f, 0);
            if (animator.GetBool("Stance4"))
                animator.CrossFadeInFixedTime("Stance 4 Blend Tree", .25f, 0);
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 

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
