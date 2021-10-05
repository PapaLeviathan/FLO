using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] float _knockBackStrength;
    [SerializeField] float _knockUpStrength;
    
    float _savedTargetID;
    float _newTargetID;
    void OnTriggerEnter(Collider collider)
    {
        var targetRigidbody = collider.GetComponent<Rigidbody>();
        var target = collider.GetComponent<HealthLogic>();

        if (target == null)
            return;
        if (targetRigidbody == null)
            return;

        Vector3 attackDirection = collider.gameObject.transform.position - transform.root.position;
        attackDirection.y = _knockUpStrength;
        target.TakeDamage(_damage);

        targetRigidbody.isKinematic = false;
        targetRigidbody.AddForce(attackDirection.normalized * _knockBackStrength, ForceMode.Impulse);
    }


}
