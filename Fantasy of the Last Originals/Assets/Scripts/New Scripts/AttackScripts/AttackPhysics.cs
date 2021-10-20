using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPhysics : MonoBehaviour
{
    [SerializeField] float _stoppingDistance = 4f;

    GameObject m_player;

    CharacterController m_controller;
    Rigidbody m_rb;

    [SerializeField] float _attackSlideDistance = 5f;

    bool m_sliding = false;
    private AutoTargetEnemy _enemyTargeter;

    private bool EnemyIsTooClose => Vector3.Distance(transform.position, _enemyTargeter.Enemy.transform.position) <
                                    _stoppingDistance;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_controller = GetComponent<CharacterController>();
        _enemyTargeter = GetComponent<AutoTargetEnemy>();
    }

    private void Update()
    {
        if (m_sliding && !EnemyIsTooClose) //applies forward movement to character controller when m_sliding is true
        {
            AttackSlide(AttackSlideDistance(_attackSlideDistance));
        }
    }

    void AttackSlide(float distance)
    {
        transform.position += transform.forward * Time.deltaTime * distance;
    }

    public float AttackSlideDistance(float distance)
    {
        return distance;
    }

    void ReadAttackDistanceValue()
    {
        _attackSlideDistance = AttackSlideDistance(_attackSlideDistance);
    }

    IEnumerator SlideDuration(float slideDuration)
    {
        if (EnemyIsTooClose)
            yield break;

        m_sliding = true;
        yield return new WaitForSeconds(slideDuration);
        m_sliding = false;
    }
}