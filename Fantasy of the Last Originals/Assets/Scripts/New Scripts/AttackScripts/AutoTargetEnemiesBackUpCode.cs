using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetEnemiesBackUpCode : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _targetDistance = 4f;
    [SerializeField] float _rotationSpeed = 1f;

    Animator _animator;
    GameObject[] _enemiesInWorld;
    GameObject _targetedEnemy;
    Vector3 _rayRotation;
    Vector3 _direction;
    RaycastHit _rayHit;
    Transform _camTransform;

    float _enemyDistance;
    public bool _canTargetNearestEnemy;
    bool _hit;
    float _turnSmoothVelocity;

    public GameObject Enemy { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        _camTransform = Camera.main.transform;
        _animator = GetComponent<Animator>();
        _enemiesInWorld = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        FindEnemies();
        Enemy = _targetedEnemy;
        if (_targetedEnemy != null)
        {
            Debug.DrawLine(transform.position, _targetedEnemy.transform.position, Color.red);
            TurnTowardEnemy();
            _canTargetNearestEnemy = false;
        }
        if (_targetedEnemy == null)
        {
            _canTargetNearestEnemy = true;
        }
    }


    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal_P1");
        float vertical = Input.GetAxis("Vertical_P1");
        _direction = new Vector3(horizontal, 0f, vertical);

        if (_direction.magnitude >= .1f)
        {
            FindEnemyWithRayCast();
        }
        else if (_canTargetNearestEnemy)
            FindNearestEnemy();

        float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(targetAngle, targetAngle, ref _turnSmoothVelocity, 2);
        _rayRotation = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), _rayRotation * 3, Color.blue);
    }

    void FindEnemyWithRayCast()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), _rayRotation, out _rayHit, 3f, _layerMask))
        {
            _targetedEnemy = _rayHit.collider.gameObject;
        }

    }
    void TurnTowardEnemy()
    {
        if (Vector3.Distance(transform.position, _targetedEnemy.transform.position) <= _targetDistance)
            if (_animator.GetBool("Attacking"))
            {
                Vector3 direction;
                Quaternion lookRotation;
                direction = (_targetedEnemy.transform.position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }
    }

    private void FindEnemies()
    {
        _enemiesInWorld = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void FindNearestEnemy()
    {
        var nearestDistance = float.MaxValue;
        _targetedEnemy = null;
        foreach (GameObject enemy in _enemiesInWorld)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                _targetedEnemy = enemy;
            }
        }
    }
}
