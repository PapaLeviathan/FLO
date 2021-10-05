using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] CinemachineManager _cinemachineManager;
    [SerializeField] CameraLogic _camLogic;
    [SerializeField] AutoTargetEnemy _targeter;
    
    Animator _animator;
    
    public static bool _turnOnTargetCam = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        SwitchState();
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Main Cam"))
        {
            
        }
    }

    void SwitchState()
    {
        if (_turnOnTargetCam)
        {
            
            _camLogic._cameraZoom = 7f;
            _animator.Play("Multi Target Cam");
            _turnOnTargetCam = false;
        }

        if (_targeter && _targeter.TargetedEnemy == null)
        {
            _animator.Play("Main Cam");
        }
    }
}