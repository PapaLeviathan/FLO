using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StunHandler : MonoBehaviour
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


    private SkillType _targetSkillTypeUsed;

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

    public float StunDuration { get; set; }
    public float AttackTimer { get; set; }
    public float CurrentDownForce { get; set; }
    public float HitStopDuration { get; set; }
    public float AirStallDuration { get; set; }
    public float AirBorneKnockUp { get; set; }

    public bool _ApplyKnockBackForce;
    public bool AirStall { get; set; }
    float _linkSkillKnockBack;
    public bool CanResetNavAgent;
    private bool IsRaising => _rb.velocity.y > 0;

    private bool IsFalling => _rb.velocity.y <= 0;

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
                ApplyHookSkillForces();
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

    private void ApplyHookSkillForces()
    {
        if (IsRaising)
        {
            _fallAccelerationMultiplier += Time.deltaTime * 4f;

            CurrentDownForce = _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
        else if (IsFalling)
        {
            _fallDecelerationMultiplier += Time.deltaTime * _weight;

            CurrentDownForce += Time.deltaTime *
                                ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                                 _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
            if (CurrentDownForce > 8)
                CurrentDownForce = 8;

            if (CurrentDownForce < 0)
                CurrentDownForce = 0f;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
    }


    private void ApplyLinkSkillForces()
    {
        if (transform.position.y >= ContactPointLaunchLimiter.y)
        {
            transform.position =
                new Vector3(transform.position.x, ContactPointLaunchLimiter.y - .1f, transform.position.z);

            _fallAccelerationMultiplier += Time.deltaTime * 3f;

            CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

            if (_rb.velocity.z > 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            if (_rb.velocity.z < 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
        else if (transform.position.y < ContactPointLaunchLimiter.y)
        {
            if (IsRaising)
            {
                _fallAccelerationMultiplier += Time.deltaTime;

                CurrentDownForce = _fallAccelerationMultiplier *
                                   ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            }
            else if (IsFalling)
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

    public void DisableNavMesh()
    {
        if (_navMesh)
            _navMesh.enabled = false;
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
        }
    }

    public void AllowKnockBackToApply(Vector3 attackForce)
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