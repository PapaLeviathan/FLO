using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamLogic : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook _vCam;

    GameObject _player;
    Animator _animator;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = _player.GetComponent<Animator>();
    }

    private void Update()
    {
        if(_animator.GetBool("Attacking"))
        {
            var freeLook = _vCam.GetComponent<CinemachineFreeLook>();
        }
    }
}
