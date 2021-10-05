using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S4Transition2Behaviour : StateMachineBehaviour
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
        // transition to attack 3 if input key was pressed in attack 1 state
        //update input count and preemptively activate attack 3
        if (_combatManager._inputCount >= 3)
        {
            animator.SetBool("Attack 3", true);
        }

        //play attack 3 when input count hits 3
        if (animator.GetBool("Attack 3") && stateInfo.IsName("S4 Transition 2"))
        {
            animator.CrossFade("S4 Attack 3", .25f, 0, 0f);
            animator.SetBool("Attack 3", false);
        }

        //transition to attack 3 if input key is pressed while in attack 2 state
        if (_combatManager.inputReceived && _combatManager._inputCount >= 3)
        {
            animator.SetBool("Attack 3", true);

            //Only transition to next attack if Attack 3 is true, you are in Transition 2 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (animator.GetBool("Attack 3") && stateInfo.IsName("S4 Transition 2"))
            {
                if (animator.GetBool("Stance1"))
                {
                    animator.CrossFade("S1 Attack 3", .25f, 0, 0f);
                }

                if (animator.GetBool("Stance2"))
                {
                    animator.CrossFade("S2 Attack 3", .25f, 0, 0f);
                }

                if (animator.GetBool("Stance3"))
                {
                    animator.CrossFade("S3 Attack 3", .25f, 0, 0f);
                }

                if (animator.GetBool("Stance4"))
                {
                    animator.CrossFade("S4 Attack 3", .25f, 0, 0f);
                }

                _combatManager.ReceiveInput();
                _combatManager.inputReceived = false;
            }
        }

        if (!animator.GetBool("Attack 3") && stateInfo.IsName("S4 Transition 2") && stateInfo.normalizedTime >
            _combatManager.ReturnTransitionSpeed2(_combatManager._player.Stance))
        {
            if (animator.GetBool("Stance1"))
                animator.CrossFadeInFixedTime("Stance 1 Blend Tree", .25f, 0);
            if (animator.GetBool("Stance2"))
                animator.CrossFadeInFixedTime("Stance 2 Blend Tree", .25f, 0);
            if (animator.GetBool("Stance3"))
                animator.CrossFadeInFixedTime("Stance 3 Blend Tree", .25f, 0);
            if (animator.GetBool("Stance4"))
                animator.CrossFadeInFixedTime("Stance 4 Blend Tree", .25f, 0);

            animator.SetBool("Attack 3", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("S4 Transition 2"))
        {
            animator.SetBool("Attack 3", false);
        }
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