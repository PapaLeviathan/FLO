using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] EnemyState _enemyState = EnemyState.Idle;
    [SerializeField] Transform _playerTarget;
    [SerializeField] float _targetDetectRange = 5f;
    [SerializeField] float _attackDelay = 2f;
    [SerializeField] float _attackRange = 2f;


    Vector3 _initialPosition;
    NavMeshAgent _navMeshAgent;

    StunHandler _stunHandler;
    Animator _animator;
    EnemyDeathLogic _enemyDeathLogic;

    float _attackTimer;
    private bool _canResetNavMesh;

    IEnumerator Start()
    {
        _initialPosition = transform.position;
        _enemyDeathLogic = GetComponent<EnemyDeathLogic>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _stunHandler = GetComponent<StunHandler>();
        _animator = GetComponent<Animator>();
        yield return new WaitForSeconds(1f);
        _playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (_navMeshAgent.isActiveAndEnabled && _playerTarget)
        {
            SetEnemyState();
            EnemyStateLogicHandler();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Foot") || collider.CompareTag("Hand"))
        {
            _enemyState = EnemyState.Hitstun;
        }
    }


    public void SetIdle()
    {
        _enemyState = EnemyState.Idle;
    }

    void SetEnemyState()
    {
        float enemyToPlayerDistance = Vector3.Distance(transform.position, _playerTarget.position);
        var startPosition = new Vector3(_initialPosition.x, transform.position.y, _initialPosition.z);

        if (_enemyDeathLogic.Died)
        {
            _enemyState = EnemyState.Death;
        }

        if (_enemyState != EnemyState.Death)
        {
            if (!_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _enemyState = EnemyState.Attack;
            }

            if (_enemyState != EnemyState.Hitstun)
            {
                if (enemyToPlayerDistance <= _targetDetectRange && enemyToPlayerDistance > _attackRange)
                    _enemyState = EnemyState.Chase;
                else if (enemyToPlayerDistance < _targetDetectRange && enemyToPlayerDistance <= _attackRange)
                    _enemyState = EnemyState.Attack;
                else if (transform.position != startPosition && enemyToPlayerDistance > _targetDetectRange)
                    _enemyState = EnemyState.Return;
                else if (transform.position == startPosition)
                    _enemyState = EnemyState.Patrol;
            }
        }
    }

    void EnemyStateLogicHandler()
    {
        if (_enemyState != EnemyState.Death)
        {
            if (_enemyState == EnemyState.Chase)
                FollowPlayer();
            if (_enemyState == EnemyState.Attack)
                AttackPlayer();
            if (_enemyState == EnemyState.Patrol)
                Patrol();
            if (_enemyState == EnemyState.Idle)
                PlayIdleAnimations();
            if (_enemyState == EnemyState.Defend)
                RunDefenseAI();
            if (_enemyState == EnemyState.Return)
                ReturnToStartPosition();
        }
    }

    void RunDefenseAI()
    {
        throw new NotImplementedException();
    }

    void ReturnToStartPosition()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = _initialPosition;
    }

    void PlayIdleAnimations()
    {
        throw new NotImplementedException();
    }

    void Patrol()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetBool("Running", false);
    }

    void AttackPlayer()
    {
        if (_attackTimer < _attackDelay)
            _attackTimer += Time.deltaTime;

        _navMeshAgent.isStopped = true;
        _animator.SetBool("Running", false);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(_playerTarget.position - transform.position), 5f * Time.deltaTime);

        if (_attackTimer >= _attackDelay)
        {
            _animator.SetBool("Attacking", true);
            StartCoroutine(ResetAttackTimer());
        }
        else
            _animator.SetBool("Attacking", false);
    }

    void FollowPlayer()
    {
        if (_playerTarget)
        {
            _navMeshAgent.isStopped = false;
            _attackTimer = 0;
            _animator.SetBool("Attacking", false);
            _animator.SetBool("Running", true);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(_playerTarget.position - transform.position), 5f * Time.deltaTime);
            _navMeshAgent.SetDestination(_playerTarget.position);
        }
    }

    IEnumerator ResetAttackTimer()
    {
        yield return new WaitForSeconds(1f);
        _attackTimer = 0f;
    }
}