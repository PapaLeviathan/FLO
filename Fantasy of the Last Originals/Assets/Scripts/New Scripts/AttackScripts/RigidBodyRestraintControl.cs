using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyRestraintControl : MonoBehaviour
{

    [SerializeField] GameObject _enemyDistancingColliders;
    [SerializeField] float _closestTargetingDistance = 4f;

    void Update()
    {
        var enemyTargeter = GetComponent<AutoTargetEnemy>();
        var rigidBody = GetComponent<Rigidbody>();
        var animator = GetComponent<Animator>();
        if (enemyTargeter.Enemy != null)
        {
            if (Vector3.Distance(transform.position, enemyTargeter.Enemy.transform.position) <= _closestTargetingDistance && enemyTargeter.Enemy.GetComponent<EnemyDeathLogic>().Died == false)
            {
                _enemyDistancingColliders.SetActive(true);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                {
                    rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                }
            }
            else if(enemyTargeter.Enemy.GetComponent<EnemyDeathLogic>().Died == true)
            {
                _enemyDistancingColliders.SetActive(false);
                rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                    | RigidbodyConstraints.FreezeRotationZ;

            }
            else
            {
                _enemyDistancingColliders.SetActive(false);
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
