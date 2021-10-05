using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogicNoRigidBody : MonoBehaviour
{
    Animator anim;
    Rigidbody _rigidBody;
    Vector3 _pointOfPlayerAttackImpact;
    Vector3 _playerAttackImpactDirection;
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
            PlayLightHitstunAnim();
        if (other.CompareTag("Foot"))
            PlayHeavyHitstunAnim();
    }

    void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponentInParent<Player>();

        if (collision.collider.CompareTag("Foot"))
        {
            var contact = collision.contacts[0];
            _pointOfPlayerAttackImpact = contact.normal;
            _playerAttackImpactDirection = new Vector3(0, 0, contact.normal.z);

            //Debug.Log($"Normal = {_pointOfPlayerAttackImpact}");
            //PlayHeavyHitstunAnim();
        }
    }

    void PlayHeavyHitstunAnim()
    {
        _rigidBody.AddForceAtPosition(_playerAttackImpactDirection, _pointOfPlayerAttackImpact, ForceMode.Impulse);
    }

    void PlayLightHitstunAnim()
    {
        transform.Translate(-Vector3.forward);
    }
}
