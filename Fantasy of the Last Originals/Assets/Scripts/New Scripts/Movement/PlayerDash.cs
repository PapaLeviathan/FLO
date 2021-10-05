using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] String _playerMask;
    [SerializeField] String _enemyMask;
    [SerializeField] float _dashForce = 50f;
    [SerializeField] float _dashDuration = 2f;

    Rigidbody _rb;
    Animator _animator;
    bool _dashing = false;
    private int _enemyMaskInt;
    private int _playerMaskInt;

    public bool Dashing { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _enemyMaskInt = LayerMask.NameToLayer(_enemyMask);
        _playerMaskInt = LayerMask.NameToLayer(_playerMask);
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            _dashing = true;
            StartCoroutine(Dash());
        }
        Dashing = _dashing;
    }

    IEnumerator Dash()
    {
        _animator.SetBool("Dashing", true);
        Physics.IgnoreLayerCollision(_playerMaskInt, _enemyMaskInt, true);
        _rb.AddForce(transform.forward * _dashForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(.1f);
        Physics.IgnoreLayerCollision(_playerMaskInt, _enemyMaskInt, false);
        yield return new WaitForSeconds(_dashDuration);
        
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, 0);
        _dashing = false;
        
        _animator.SetBool("Dashing", false);
    }
}
