using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] Transform _feet;

    bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    private void FixedUpdate()
    {
        UpdateIsGrounded();
    }

    public bool UpdateIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_feet.position, .2f, _groundLayerMask);
        return _isGrounded;
    }
}