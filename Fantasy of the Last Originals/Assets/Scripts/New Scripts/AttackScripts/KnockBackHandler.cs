using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBackHandler : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] Transform _feet;

    [Header("Gravity")] [SerializeField] float _startDownPull = .6f;
    [SerializeField] float _downPull = 2f;
    [SerializeField] float _weight = .5f;
    [SerializeField] float _fallAccelerationMultiplier;
    [SerializeField] float _fallDecelerationMultiplier;
    [SerializeField] float _fallDecelerationNormalizer;
    [SerializeField] float _fallAccelerationNormalizer = .5f;


    private SkillType _targetSkillTypeUsed = SkillType.LinkSkill;

    NavMeshAgent _navMesh;
    Rigidbody _rb;

    public GroundCheck _groundCheck;
    public GroundCheck GroundCheck => _groundCheck;
    
    EnemyDeathLogic _enemyDeathLogic;
    EnemyAI _enemyAI;
    Player _playerLogic;
    Animator _animator;

    Vector3 ContactPointLaunchLimiter;
    Vector3 KnockBackForce;

    public float AttackTimer;
    public float CurrentDownForce;
    public float HitStopDuration;
    public float AirStallDuration;
    public float AirBorneKnockUp;

    public bool _ApplyKnockBackForce;
    public bool AirStall;
    float _linkSkillKnockBack;
    public bool CanResetNavAgent;

    // Start is called before the first frame update
    void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _navMesh = GetComponent<NavMeshAgent>();
        _enemyAI = GetComponent<EnemyAI>();
        _playerLogic = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_groundCheck.UpdateIsGrounded())
        {
            _downPull = _startDownPull;
            if (_enemyAI != null)
                _enemyAI.SetIdle();
        }
    }

    void FixedUpdate()
    {
        LimitFallAccelerationMultiplier();
        ApplyKnockBackForce();
        if (!_groundCheck.UpdateIsGrounded())
        {
            ApplyAirStall();
            if (_targetSkillTypeUsed == SkillType.LinkSkill)
            {
                ApplyLinkSkillForces();
            }
            else if (_targetSkillTypeUsed != SkillType.LinkSkill)
            {
                if (_rb.velocity.y > 0)
                {
                    _fallAccelerationMultiplier += Time.deltaTime;

                    CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                                       ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
                }
                else if (_rb.velocity.y <= 0)
                {
                    if (_fallAccelerationMultiplier > 2f)
                    {
                        _fallDecelerationMultiplier = 100f;
                    }
                    else
                    {
                        _fallDecelerationMultiplier = 0f;
                    }

                    _fallDecelerationMultiplier += Time.deltaTime * _weight;
                    CurrentDownForce -= Time.deltaTime *
                                        ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                                         _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
                    if (CurrentDownForce < 0)
                        CurrentDownForce = 0f;
                    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
                }
            }
        }
        else
        {
            _fallAccelerationMultiplier = 0;
            _fallDecelerationMultiplier = 0;
            CurrentDownForce = 0;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
    }

    private void ApplyLinkSkillForces()
    {
        if (transform.position.y >= ContactPointLaunchLimiter.y)
        {
            transform.position =
                new Vector3(transform.position.x, ContactPointLaunchLimiter.y - .1f, transform.position.z);

            _fallAccelerationMultiplier += Time.deltaTime * 10f;

            CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

            if (_rb.velocity.z > 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            if (_rb.velocity.z < 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
        else if (transform.position.y < ContactPointLaunchLimiter.y)
        {
            if (_rb.velocity.y > 0)
            {
                _fallAccelerationMultiplier += Time.deltaTime;

                CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                                   ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            }
            else if (_rb.velocity.y <= 0)
            {
                if (_fallAccelerationMultiplier > 2f)
                {
                    _fallDecelerationMultiplier = 100f;
                }
                else
                {
                    _fallDecelerationMultiplier = 0f;
                }

                _fallDecelerationMultiplier += Time.deltaTime * _weight;
                CurrentDownForce -=
                    Time.deltaTime *
                    ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                     _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
                if (CurrentDownForce < 0)
                    CurrentDownForce = 0f;
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            }
        }
    }

    private void ApplyAirStall()
    {
        if (AirStall)
        {
            _fallAccelerationMultiplier = 0;
            _fallDecelerationMultiplier = 0;
            CurrentDownForce = 0;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            StartCoroutine(ResetAirStall(AirStallDuration));
        }
    }

    private void ApplyKnockBackForce()
    {
        if (_ApplyKnockBackForce)
        {
            StartCoroutine(KnockBackAfterHitStop(KnockBackForce));
        }
    }

    private void LimitFallAccelerationMultiplier()
    {
        if (_fallAccelerationMultiplier > 5f)
            _fallAccelerationMultiplier = 5f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _rb.useGravity = true;
        }
    }

    public void SetAirBorneKnockUp(float airBornKnockUp)
    {
        AirBorneKnockUp = airBornKnockUp;
    }

    public void SetLinkSkillKnockBack(float knockBack)
    {
        _linkSkillKnockBack = knockBack;
    }

    public void SetKnockBackPower(Vector3 attackForce)
    {
        KnockBackForce = attackForce;
        if (_targetSkillTypeUsed == SkillType.LinkSkill)
        {
            _linkSkillKnockBack = attackForce.z;
        }
    }

    public void ApplyKnockBack(float hitStopDuration)
    {
        _ApplyKnockBackForce = true;
        HitStopDuration = hitStopDuration;
    }

    public void SetContactPoint(SkillType skillType, Vector3 contactPoint)
    {
        _targetSkillTypeUsed = skillType;
        ContactPointLaunchLimiter = contactPoint;
    }

    public void SetNavMeshEnabled(bool enabled)
    {
        _navMesh.enabled = enabled;
    }

    public void SetAirStall(float airStallDuration)
    {
        AirStall = true;
        AirStallDuration = airStallDuration;
    }

    public void ResetDownForce()
    {
        CurrentDownForce = 0;
    }

    public void SetDownPull(float downPull)
    {
        _downPull = downPull;
    }

    public void DisableActions(bool disable)
    {
        _navMesh.enabled = disable;
    }

    IEnumerator ResetAirStall(float airStallDuration)
    {
        yield return new WaitForSeconds(airStallDuration);
        AirStall = false;
    }

    IEnumerator KnockBackAfterHitStop(Vector3 knockBack)
    {
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(HitStopDuration);
        _rb.velocity = KnockBackForce;

        _ApplyKnockBackForce = false;
    }
}