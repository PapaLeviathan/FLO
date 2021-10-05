using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Attacking,
    HitStun,
    KnockBack,
    Dead
}
public class PlayerStateControl : MonoBehaviour
{
    Player _player;
    KnockBackHandler _knockBackHandler;

    Rigidbody _rigidbody;
    public PlayerState _playerState;
    PlayerState _currentState;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStateLogicHandler();
    }
    public void SetPlayerState(PlayerState state)
    {
        if (_currentState == state)
            return;
        _currentState = state;
        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Attacking:
                break;
            case PlayerState.Running:
                break;
            case PlayerState.HitStun:
                break;
            case PlayerState.KnockBack:
                break;
            case PlayerState.Dead:
                break;
        }
    }

    void PlayerStateLogicHandler()
    {
        if (_playerState != PlayerState.Dead)
        {
            if (_playerState == PlayerState.Idle)
            {
                TogglePlayerControl(true);
            }
            if (_playerState == PlayerState.Running)
            {
                TogglePlayerControl(true);
            }
            if (_playerState == PlayerState.Attacking)
            {
                TogglePlayerControl(true);
            }
            if (_playerState == PlayerState.HitStun)
                TogglePlayerControl(false);
            if (_playerState == PlayerState.KnockBack)
                TogglePlayerControl(false);
        }
    }

    void TogglePlayerControl(bool toggle)
    {
        _player.enabled = toggle;
    }
}
