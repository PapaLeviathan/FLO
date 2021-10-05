using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Defend,
    Hitstun,
    Return,
    Death

}
public class KnockBackHandler : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] Transform _feet;

    [Header("Gravity")]
    [SerializeField] float _startDownPull = .6f;
    [SerializeField] float _downPull = 2f;
    [SerializeField] float _weight = .5f;
    [SerializeField] float _fallAccelerationMultiplier;
    [SerializeField] float _fallDecelerationMultiplier;
    [SerializeField] float _fallDecelerationNormalizer;
    [SerializeField] float _fallAccelerationNormalizer = .5f;

    [SerializeField] bool _isGrounded = true;
    [SerializeField] SkillType _skillType = SkillType.LinkSkill;

    public bool IsGrounded { get { return _isGrounded; } }

    NavMeshAgent _navMesh;
    Rigidbody _rb;

    EnemyDeathLogic _enemyDeathLogic;
    EnemyAI _enemyAI;
    Player _playerLogic;
    Animator _animator;

    Vector3 _contactPointLaunchLimiter;
    Vector3 _knockBackForce;

    float _attackTimer;
    float _currentDownForce;
    float _hitStopDuration;
    float _airStallDuration;
    float _airBorneKnockUp;

    bool _applyKnockBackForce;
    bool _airStall;
    float _linkSkillKnockBack;

    // Start is called before the first frame update
    void Start()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _enemyAI = GetComponent<EnemyAI>();
        _playerLogic = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateIsGrounded();
        if (_isGrounded)
        {
            _downPull = _startDownPull;
            if (_enemyAI != null)
                _enemyAI.SetIdle();
        }
    }

    void FixedUpdate()
    {
        if (_fallAccelerationMultiplier > 5f)
            _fallAccelerationMultiplier = 5f;
        if (_applyKnockBackForce)
        {
            StartCoroutine(KnockBackAfterHitStop(_knockBackForce));
        }
        if (!_isGrounded)
        {
            if (_airStall)
            {
                _fallAccelerationMultiplier = 0;
                _fallDecelerationMultiplier = 0;
                _currentDownForce = 0;
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);
                StartCoroutine(ResetAirStall(_airStallDuration));
            }
            if (_skillType == SkillType.LinkSkill)
            {
                if (transform.position.y >= _contactPointLaunchLimiter.y)
                {
                    transform.position = new Vector3(transform.position.x, _contactPointLaunchLimiter.y - .1f, transform.position.z);

                    _fallAccelerationMultiplier += Time.deltaTime * 10f;

                    _currentDownForce = _downPull * _fallAccelerationMultiplier * ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                    if(_rb.velocity.z > 0)
                    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);
                    if(_rb.velocity.z < 0)
                    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);

                }
                else if (transform.position.y < _contactPointLaunchLimiter.y)
                {
                    if (_rb.velocity.y > 0)
                    {
                        _fallAccelerationMultiplier += Time.deltaTime;

                        _currentDownForce = _downPull * _fallAccelerationMultiplier * ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);

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
                        _currentDownForce -= Time.deltaTime * ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) * _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
                        if (_currentDownForce < 0)
                            _currentDownForce = 0f;
                        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);

                    }
                }
            }
            else if (_skillType != SkillType.LinkSkill)
            {
                if (_rb.velocity.y > 0)
                {
                    _fallAccelerationMultiplier += Time.deltaTime;

                    _currentDownForce = _downPull * _fallAccelerationMultiplier * ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);

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
                    _currentDownForce -= Time.deltaTime * ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) * _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
                    if (_currentDownForce < 0)
                        _currentDownForce = 0f;
                    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);

                }
            }
        }
        else
        {
            _fallAccelerationMultiplier = 0;
            _fallDecelerationMultiplier = 0;
            _currentDownForce = 0;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _currentDownForce, _rb.velocity.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _rb.useGravity = true;
        }
    }

    public bool UpdateIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_feet.position, .1f, _groundLayerMask);
        return _isGrounded;
    }
    public void SetAirBorneKnockUp(float airBornKnockUp)
    {
        _airBorneKnockUp = airBornKnockUp;
    }

    public void SetLinkSkillKnockBack(float knockBack)
    {
        _linkSkillKnockBack = knockBack;
    }
    public void SetKnockBackPower(Vector3 attackForce)
    {
        _knockBackForce = attackForce;
        if(_skillType == SkillType.LinkSkill)
        {
            _linkSkillKnockBack = attackForce.z;
        }
    }

    public void ApplyKnockBack(float hitStopDuration)
    {
        _applyKnockBackForce = true;
        _hitStopDuration = hitStopDuration;
    }

    public void SetContactPoint(SkillType skillType, Vector3 contactPoint)
    {
        _skillType = skillType;
        _contactPointLaunchLimiter = contactPoint;
    }

    public void SetNavMeshEnabled(bool enabled)
    {
        _navMesh.enabled = enabled;
    }

    public void SetAirStall(float airStallDuration)
    {
        _airStall = true;
        _airStallDuration = airStallDuration;
    }

    public void ResetDownForce()
    {
        _currentDownForce = 0;
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
        _airStall = false;
    }
    IEnumerator KnockBackAfterHitStop(Vector3 knockBack)
    {
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(_hitStopDuration);
        _rb.velocity = _knockBackForce;

        _applyKnockBackForce = false;
    }
}
