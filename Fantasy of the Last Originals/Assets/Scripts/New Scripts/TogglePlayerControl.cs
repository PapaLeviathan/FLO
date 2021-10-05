using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlayerControl : MonoBehaviour
{
    Player _player;
    Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit Stun"))
        {
            _player.enabled = false;
        }
        else
        {
            _player.enabled = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            //_playerLogic.enabled = true;
        }
    }
}
