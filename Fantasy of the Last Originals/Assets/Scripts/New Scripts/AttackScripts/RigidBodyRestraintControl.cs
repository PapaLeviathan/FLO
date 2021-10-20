using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyRestraintControl : MonoBehaviour
{

    [SerializeField] float _closestTargetingDistance = 4f;

    private ToggleDistanceColliders _toggleDistanceColliders;
    private AutoTargetEnemy _enemyTargeter;
    private Rigidbody _rigidBody;
    private Animator _animator;
    

    private void Start()
    {
        _toggleDistanceColliders = GetComponent<ToggleDistanceColliders>();
        _enemyTargeter = GetComponent<AutoTargetEnemy>();
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (_enemyTargeter.Enemy != null)
        {
            if (_enemyTargeter.EnemyInRange && _enemyTargeter.Enemy.GetComponent<EnemyDeathLogic>().Died == false)
            {
                _toggleDistanceColliders.ToggleDistanceCollidersTrue();
                
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                {
                    _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                }
            }
            else if(_enemyTargeter.Enemy.GetComponent<EnemyDeathLogic>().Died == true)
            {
                _toggleDistanceColliders.ToggleDistanceCollidersFalse();
                _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                             | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                _toggleDistanceColliders.ToggleDistanceCollidersFalse();
            }
        }

    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && GetComponent<Player>().IsGrounded)
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}