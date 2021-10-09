using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public enum PlayerID
{
    _P1,
    _P2
}

public enum PlayerStance
{
    Stance1,
    Stance2,
    Stance3,
    Stance4
}

public class Player : MonoBehaviour
{
    [SerializeField] PlayerID _playerID;
    [SerializeField] LayerMask _layerMask;

    [SerializeField] bool _jump;
    [SerializeField] float _movementSpeed = 5.0f;
    [SerializeField] float _jumpHeight = 10f;
    [SerializeField] float downPull = 2f;
    [SerializeField] bool isGrounded;
    [SerializeField] float _turnSmoothTime = 2f;
    [SerializeField] float _turnSmoothVelocity = 2f;

    [SerializeField] private Camera _camera;
    CameraLogic _cameraLogic;

    Rigidbody _rb;
    Animator _animator;
    PlayerDash _playerDash;


    public Transform _camTransform;
    public PlayerStance Stance;

    public Animator Animator => _animator;

    Vector3 _movement;
    Vector3 _heightMovement;


    float _horizontalInput;
    float _verticalInput;

    public bool IsGrounded { get; private set; }
    public HealthLogic Health => GetComponent<HealthLogic>();

    public bool _isJumping = false;

    float fallTimer;

    private PhotonView _view;

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _playerDash = GetComponent<PlayerDash>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (_camera)
        {
            _cameraLogic = _camera.GetComponent<CameraLogic>();
        }

        _camTransform = _camera.transform;

        if (!_view.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineStateDrivenCamera>().gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_view.IsMine)
            return;
        float camHoriztonal = Input.GetAxis("Mouse X");
        
        ReadHorizontalAndVerticalInput();

        if (PlayerJumpedFromGround())
        {
            if (!IsAttacking())
            {
                _jump = true;
            }
        }

        if (_animator)
        {
            _animator.SetFloat("MovementInput",
                Mathf.Max(Mathf.Abs(_horizontalInput),
                    Mathf.Abs(_verticalInput))); //Mathf.Max returns whichever value is greater of the two paraments in (float a, float b)
        }


        ChangeStance();
        IsGrounded = isGrounded;
    }

    private bool IsAttacking()
    {
        return _animator.GetBool("Attacking");
    }

    private bool PlayerJumpedFromGround()
    {
        return Input.GetButtonDown("Jump" + _playerID) && _rb && isGrounded;
    }

    private void ReadHorizontalAndVerticalInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal" + _playerID); //inputs WITH smoothing (GetAxis, not GetAxisRaw)
        _verticalInput = Input.GetAxisRaw("Vertical" + _playerID); //m_playerID for determining if player 1 or 2
    }

    private void FixedUpdate()
    {
        if (!_view.IsMine)
            return;
        
        Vector3 movePos = transform.right * _horizontalInput + transform.forward * _verticalInput;
        Vector3 newMovePos = new Vector3(movePos.x, _rb.velocity.y, movePos.z);

        //Grounding
        isGrounded =
            Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z),
                .4f, _layerMask);

        if (!isGrounded)
        {
            fallTimer += Time.deltaTime;
            var downForce = downPull * fallTimer * fallTimer;
            if (downForce > 10f)
            {
                downForce = 10f;
            }

            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - downForce, _rb.velocity.z);

            _animator.SetBool("Airborne", true);
        }
        else
        {
            fallTimer = 0;
        }

        //Now I can have animations turn "Attacking" boolean off and on during a specific number of frames, turning movement ability on and off
        if (!_animator.GetBool("Attacking"))
        {
            if (_jump)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                       | RigidbodyConstraints.FreezeRotationZ;
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpHeight, _rb.velocity.z);
                _isJumping = true; //for landing
                _jump = false;
                _animator.SetBool("Landing", false);
                _animator.CrossFadeInFixedTime("Jumping", .2f, 0);
            }

            _movement = new Vector3(_horizontalInput, 0, _verticalInput);
        }

        //have player face the direction the camera is facing only if they are moving
        //Rotate toward movement direction
        if (_movement.magnitude >= .3f && !_animator.GetBool("Attacking") &&
            !_playerDash.Dashing) //only set transform.forward when m_movement vector is diff from vector3.zero
        {
            //rotate movement direction based on camera rotation
            float targetAngle = Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
            if (Mathf.Abs(Input.GetAxisRaw("Mouse X")) > 0)
            {
                _turnSmoothTime = .3f;
            }
            else
            {
                _turnSmoothTime = .075f;
            }

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                _turnSmoothTime);
            Vector3 moveDir;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            //the angle that the character is moving * the actual movement itself
            moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            var moving = moveDir.normalized * _movementSpeed;
            //if (!m_animator.GetBool("Attacking") && !jumping)
            //{
            //    //cancel fighting animations (return to idle state)
            //    //THIS IS ALSO CANCELING JUMPING. MOVEMENT INPUTS ARE CAUSING ANIMATOR.CROSSFADE TO BE CALLED
            //    if (m_stance == PlayerStance.Stance1)
            //    {
            //        m_animator.CrossFade("Stance 1 Blend Tree", 0f, 0);
            //    }
            //    if (m_stance == PlayerStance.Stance2)
            //    {
            //        m_animator.CrossFade("Stance 2 Blend Tree", 0f, 0);
            //    }
            //    if (m_stance == PlayerStance.Stance3)
            //    {
            //        m_animator.CrossFade("Stance 3 Blend Tree", 0f, 0);
            //    }
            //    if (m_stance == PlayerStance.Stance4)
            //    {
            //        m_animator.CrossFade("Stance 4 Blend Tree", 0f, 0);
            //    }
            //}

            if (!_animator.GetBool("Attacking"))
            {
                //how to have character face direction you are moving


                if (!_isJumping)
                {
                    _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, 5f);
                    //m_rb.AddForce(m_movementSpeed * moveDir, ForceMode.VelocityChange);
                    _rb.velocity = new Vector3(moving.x, _rb.velocity.y, moving.z);
                }
                else if (_isJumping)
                {
                    _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, 10f);
                    //aerial mobility
                    _rb.AddForce(.1f * moveDir, ForceMode.VelocityChange);
                }
            }
        }
        else
        {
            //let character jump while stopping sliding
            //character only stops completely when grounded
            //set airborne false whenver grounded
            if (isGrounded)
            {
                if (!_playerDash.Dashing)
                {
                    _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
                }
            }
        }
    }

    void ChangeStance()
    {
        switch (Stance)
        {
            case PlayerStance.Stance1:
                Stance1();
                break;
            case PlayerStance.Stance2:
                Stance2();
                break;
            case PlayerStance.Stance3:
                Stance3();
                break;
            case PlayerStance.Stance4:
                Stance4();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Stance = PlayerStance.Stance1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Stance = PlayerStance.Stance2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Stance = PlayerStance.Stance3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Stance = PlayerStance.Stance4;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Ground")
        {
            _isJumping = false;
            _animator.SetBool("Airborne", false);
            _animator.SetTrigger("Landing");
        }
    }

    void Stance1()
    {
        _animator.SetBool("Stance1", true);
        _animator.SetBool("Stance2", false);
        _animator.SetBool("Stance3", false);
        _animator.SetBool("Stance4", false);
    }

    void Stance2()
    {
        _animator.SetBool("Stance1", false);
        _animator.SetBool("Stance2", true);
        _animator.SetBool("Stance3", false);
        _animator.SetBool("Stance4", false);
    }

    void Stance3()
    {
        _animator.SetBool("Stance1", false);
        _animator.SetBool("Stance2", false);
        _animator.SetBool("Stance3", true);
        _animator.SetBool("Stance4", false);
    }

    void Stance4()
    {
        _animator.SetBool("Stance1", false);
        _animator.SetBool("Stance2", false);
        _animator.SetBool("Stance3", false);
        _animator.SetBool("Stance4", true);
    }
}