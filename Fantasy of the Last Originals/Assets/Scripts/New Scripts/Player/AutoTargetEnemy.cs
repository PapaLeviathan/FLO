using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetEnemy : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _targetDistance = 4f;
    [SerializeField] float _rotationSpeed = 1f;

    Animator _animator;
    GameObject[] _enemiesInWorld;
    GameObject _nearestEnemy;
    GameObject _targetedEnemy;
    Vector3 _rayRotation;
    Vector3 _direction;
    RaycastHit _rayHit;
    Transform _camTransform;

    public GameObject NearestEnemy { get { return _nearestEnemy; } }
    public GameObject TargetedEnemy { get { return _targetedEnemy; } }

    public bool EnemyInRange => Vector3.Distance(transform.position, _targetedEnemy.transform.position) <= _targetDistance;
    public bool _canTargetNearestEnemy;
    float _enemyDistance;
    bool _targetedWithRay;
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
        if (_targetedEnemy != null)
            Debug.DrawLine(transform.position, _targetedEnemy.transform.position, Color.yellow);
        
        StoreEnemiesInArray();
        FindNearestEnemy();
        CreateEnemyDetectingRay();
        Enemy = _nearestEnemy;

        Debug.DrawLine(transform.position, _nearestEnemy.transform.position, Color.red);

    }
    void FixedUpdate()
    {
        ToggleTargetedEnemy();
        TurnTowardEnemy();
    }

    void StoreEnemiesInArray()
    {
        _enemiesInWorld = GameObject.FindGameObjectsWithTag("Enemy");
    }
    void CreateEnemyDetectingRay()
    {
        float horizontal = Input.GetAxis("Horizontal_P1");
        float vertical = Input.GetAxis("Vertical_P1");
        _direction = new Vector3(horizontal, 0f, vertical);


        float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
        _rayRotation = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    void ToggleTargetedEnemy()
    {
        if (_targetedWithRay == false)
        {
            if (_nearestEnemy != null)
                TargetNearestEnemy();
        }
        if (_direction.magnitude >= .1)
        {
            TargetEnemyWithRayCast();
        }

        if (_targetedEnemy != null)
            if (Vector3.Distance(transform.position, _targetedEnemy.transform.position) > _targetDistance)
                _targetedEnemy = null;

        if (_targetedEnemy == null)
            _targetedWithRay = false;

    }

    void TargetNearestEnemy()
    {
        if (Vector3.Distance(transform.position, _nearestEnemy.transform.position) <= _targetDistance)
        {
            _targetedEnemy = _nearestEnemy;
        }
        else
            _targetedEnemy = null;
    }

    void TargetEnemyWithRayCast()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), _rayRotation, out _rayHit, 3f, _layerMask))
        {
            var enemy = _rayHit.collider.gameObject;

            if (Vector3.Distance(transform.position, enemy.transform.position) <= _targetDistance)
            {
                Debug.Log("Targeted with ray");
                _targetedEnemy = _rayHit.collider.gameObject;
                _targetedWithRay = true;
            }
        }

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), _rayRotation * 3, Color.blue);
    }

    void TurnTowardEnemy()
    {
        if (_animator.GetBool("Attacking"))
        {
            if (_targetedEnemy != null)
            {
                Vector3 direction;
                Quaternion lookRotation;
                direction = (_targetedEnemy.transform.position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }
        }

    }


    void FindNearestEnemy()
    {
        var nearestDistance = float.MaxValue;
        foreach (GameObject enemy in _enemiesInWorld)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                _nearestEnemy = enemy;
            }
        }
    }
}

