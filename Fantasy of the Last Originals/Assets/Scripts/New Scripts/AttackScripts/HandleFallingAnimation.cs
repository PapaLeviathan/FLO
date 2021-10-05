using UnityEngine;

public abstract class HandleFallingAnimation : MonoBehaviour
{
    [SerializeField] float _sphereYOffSet;
    [SerializeField] float _landingRadius;

    protected Animator _animator;
    protected Rigidbody _rb;
    protected LayerMask _layerMask;
    protected KnockBackHandler _knockBackHandler;

    public Animator Animator => _animator;

    // Start is called before the first frame update
    void Start()
    {
        _layerMask = LayerMask.GetMask("Ground");
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _knockBackHandler = GetComponent<KnockBackHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Grounded", _knockBackHandler.UpdateIsGrounded());

        if (!_animator.GetBool("Grounded"))
            _animator.SetFloat("Falling", _rb.velocity.y);
        else
            _animator.SetFloat("Falling", 0);

        if (!_animator.GetBool("Grounded") && _animator.GetFloat("Falling") < .1)
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