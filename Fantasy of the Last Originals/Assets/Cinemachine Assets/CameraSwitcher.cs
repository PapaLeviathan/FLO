using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] CameraLogic _camLogic;
    [SerializeField] AutoTargetEnemy _targeter;
    
    Animator _animator;
    
    public static bool TurnOnTargetCam = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        SwitchState();
    }

    void SwitchState()
    {
        if (TurnOnTargetCam)
        {
            _camLogic._cameraZoom = 7f;
            _animator.Play("Multi Target Cam");
            TurnOnTargetCam = false;
        }

        if (_targeter && _targeter.TargetedEnemy == null)
        {
            _animator.Play("Main Cam");
        }
    }
}