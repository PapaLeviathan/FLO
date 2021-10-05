using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthLogic : MonoBehaviour
{
    [SerializeField] Image _healthBar;
    [SerializeField] float _maxHealth = 150f;
    [SerializeField] float _currentHealth;


    public float MAXHealthValue => _maxHealth;
    public float CurrentHealthValue => _currentHealth;
    public event Action OnHealthUpdate;

    private EnemyAI _enemy;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<EnemyAI>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _animator.SetTrigger("Knock Back");
        _currentHealth -= damage;
        if (_enemy)
            _healthBar.fillAmount = _currentHealth / _maxHealth;
        
        OnHealthUpdate?.Invoke();
    }
}