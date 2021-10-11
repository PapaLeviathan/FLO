using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyRestraintControl : MonoBehaviour
{

    [SerializeField] float _closestTargetingDistance = 4f;

    private ToggleDistanceColliders _toggleDistanceColliders;

    private void Start()
    {
        _toggleDistanceColliders = GetComponent<ToggleDistanceColliders>();
    }

    void Update()
    {
        var enemyTargeter = GetComponent<AutoTargetEnemy>();
        var rigidBody = GetComponent<Rigidbody>();
        var animator = GetComponent<Animator>();
        if (enemyTargeter.Enemy != null)
        {
            if (Vector3.Distance(transform.position, enemyTargeter.Enemy.transform.position) <= _closestTargetingDistance && enemyTargeter.Enemy.GetComponent<EnemyDeathLogic>().Died == false)
            {
                _toggleDistanceColliders.ToggleDistanceCollidersTrue();
                
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                {
                    rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                }
            }
            else if(enemyTargeter.Enemy.GetComponent<EnemyDeathLogic>().Died == true)
            {
                _toggleDistanceColliders.ToggleDistanceCollidersFalse();
                rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
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