using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] Transform _feet;

    bool _isGrounded = true;

    public bool UpdateIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_feet.position, .1f, _groundLayerMask);
        return _isGrounded;
    }
}