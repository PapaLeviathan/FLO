using UnityEngine;

public abstract class HandleFallingAnimation : MonoBehaviour
{
    [SerializeField] float _sphereYOffSet;
    [SerializeField] float _landingRadius;

    protected Animator _animator;
    protected Rigidbody _rb;
    protected LayerMask _layerMask;
    protected StunHandler StunHandler;
    protected GroundCheck _groundCheck;

    public Animator Animator => _animator;

    // Start is called before the first frame update
    void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _layerMask = LayerMask.GetMask("Ground");
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        StunHandler = GetComponent<StunHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Grounded", StunHandler._groundCheck.IsGrounded);

        if (!_animator.GetBool("Grounded"))
            _animator.SetFloat("Falling", _rb.velocity.y);
        else
            _animator.SetFloat("Falling", 0);

        if (!_groundCheck.IsGrounded && _animator.GetFloat("Falling") < .1)
        {
            var hit = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + _sphereYOffSet, transform.position.z), _landingRadius, _layerMask);
            if (hit)
                PlayFallAnimation();
        }
        else
            StopFallAnimation();
    }

    protected abstract void PlayFallAnimation();
    protected abstract void StopFallAnimation();
}