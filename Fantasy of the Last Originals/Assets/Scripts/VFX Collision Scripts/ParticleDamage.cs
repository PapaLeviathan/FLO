using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    private float _damagePerCollision = 10f;

    private void OnParticleCollision(GameObject gameObject)
    {
        var target = gameObject.GetComponent<EnemyAI>();
        if (target == null)
            return;
        target.GetComponent<HealthLogic>().TakeDamage(_damagePerCollision);
    }
}
