using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CombatManagerScript : MonoBehaviour
{

    public int _inputCount = 0;

    public Player _player;

    public bool canReceiveInput;
    public bool inputReceived;
    public int m_currentLayer = 0;

    Animator _animator;

    //Transition Speeds for Stance 1 Transitions
    public float m_S1Transition1Speed = .5f;
    public float m_S1Transition2Speed = .5f;

    //Transition Speeds for Stance 2 Transitions
    public float m_S2Transition1Speed = .5f;
    public float m_S2Transition2Speed = .5f;

    //Transition Speeds for Stance 3 Transitions
    public float m_S3Transition1Speed = .5f;
    public float m_S3Transition2Speed = .5f;

    //Transition Speeds for Stance 4 Transitions
    public float m_S4Transition1Speed = .5f;
    public float m_S4Transition2Speed = .5f;

    private PhotonView _view;
    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_view.IsMine)
        {
            Attack();
            ReceiveInput();
            if (!_player._isJumping)
            {
                if (Input.GetButtonDown("Light Attack"))
                {
                    canReceiveInput = true;
                    _inputCount++;
                }
            }
        }
    }

    public float ReturnTransitionSpeed1(PlayerStance stance)
    {
        float transitionSpeed = 0;

        if (stance == PlayerStance.Stance1)
        {
            transitionSpeed = m_S1Transition1Speed;
        }
        if (stance == PlayerStance.Stance2)
        {
            transitionSpeed = m_S2Transition1Speed;
        }
        if (stance == PlayerStance.Stance3)
        {
            transitionSpeed = m_S3Transition1Speed;
        }
        if (stance == PlayerStance.Stance4)
        {
            transitionSpeed = m_S4Transition1Speed;
        }

        return transitionSpeed;
    }
    public float ReturnTransitionSpeed2(PlayerStance stance)
    {
        float transitionSpeed = 0;

        if (stance == PlayerStance.Stance1)
        {
            transitionSpeed = m_S1Transition2Speed;
        }
        if (stance == PlayerStance.Stance2)
        {
            transitionSpeed = m_S2Transition2Speed;
        }
        if (stance == PlayerStance.Stance3)
        {
            transitionSpeed = m_S3Transition2Speed;
        }
        if (stance == PlayerStance.Stance4)
        {
            transitionSpeed = m_S4Transition2Speed;
        }

        return transitionSpeed;
    }

    public void Attack()
    {
        if (canReceiveInput) //On button press, inputReceived turns true if canReceiveInput is true.
        {
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }

    public void ReceiveInput()
    {
        if (!canReceiveInput) //On button press, canReceiveInput will turn true if canReceiveInput is false
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false; //When attack button is pressed, canReceiveInput will turn false if canReceiveInput is already true. If canReceiveInput is false, inputReceived cannot turn true
        }
    }

    public void AttackCancelPoint()
    {
        _animator.SetBool("Attacking", false);
    }

    public void AttackLockPoint()
    {
        _animator.SetBool("Attacking", true);
    }
    public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        return currentTime;
    }

}
