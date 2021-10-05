using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBackCurveSetter : MonoBehaviour
{
    [SerializeField] float _knockBackStrength;
    [SerializeField] float _knockUpStrength;
    [SerializeField] float _damage = 10f;
    [SerializeField] AnimationCurve _trajectoryCurve;
    [SerializeField] Vector3 _trajectoryOffset;

    public float _hitStun = .3f;

    HealthLogic _targetHealthLogic;
    float _savedTargetID;
    float _newTargetID;
    Rigidbody _rigidBody;
    KnockBackCurveHandler _knockBackHandler;
    //bool _isOnCooldown;

    void OnDisable()
    {
        _savedTargetID = 0;    
    }
    void OnTriggerEnter(Collider collider)
    {
        _knockBackHandler = collider.GetComponent<KnockBackCurveHandler>();
        _rigidBody = collider.GetComponent<Rigidbody>();
        _targetHealthLogic = collider.GetComponent<HealthLogic>();
        var targetSound = collider.GetComponent<ImpactSoundHandler>();
        var targetNavMesh = collider.GetComponent<NavMeshAgent>();

        if (_targetHealthLogic == null)
            return;
            _newTargetID = _targetHealthLogic.GetInstanceID();
        //if the new enemy equals the saved enemy, return;
        if (_newTargetID == _savedTargetID)
        {
            return;
        }
        else
        {
            Vector3 attackDirection = collider.gameObject.transform.position - transform.root.position;
            attackDirection.y = _knockUpStrength;
            _targetHealthLogic.TakeDamage(_damage);
            targetSound.PlayHitStunSound();

            Debug.Log("hit new enemy");
            _rigidBody.isKinematic = false;

            if(targetNavMesh != null)
            {
                targetNavMesh.enabled = false;
            }

            //_rigidBody.AddForce(attackDirection.normalized * _knockBackStrength, ForceMode.Impulse);
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _knockBackStrength, _rigidBody.velocity.z);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        _savedTargetID = _newTargetID;
    }

}
