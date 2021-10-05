using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [SerializeField] float _cameraRotationXOffset = 1f;
    [SerializeField] float _cameraRotationYOffset = 1.0f;
    [SerializeField] float _cameraTargetXOffset = 1f;
    [SerializeField] float _cameraTargetYOffset = 1.0f;

    public float _cameraZoom = 5.0f;

    Vector3 m_cameraTarget;
    GameObject m_player;

    CinemachineManager _cinemachineManager;

    public float DistanceZ { get { return _cameraZoom; } }

    //Store our rotations
    float m_rotationX;
    float m_rotationY;

    const float MIN_X = -20.0f;
    const float MAX_X = 20.0f;

    //Camera lookat and camera zooming
    //transform.LookAt(m_cameraTarget
    //Variables used to Mathf.Clamp distanceZ
    const float MIN_Z = 2.0f;
    const float MAX_Z = 9f;

    bool _lockedOn;
    Transform _lockOnTarget;

    // Start is called before the first frame update
    void Start()
    {
        _cinemachineManager = GetComponent<CinemachineManager>();
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(_lockedOn && _lockOnTarget != null)
        {
            Vector3 camToTarget = _lockOnTarget.position - transform.position;
            Vector3 planerCamToTarget = Vector3.ProjectOnPlane(camToTarget, Vector3.up);
            //planarDirection = planerCamToTarget != Vector3.zero ? planerCamToTarget.normalized : planarDirection;
        }
        UpdateCursorLockMode();
        //SetCameraTarget();
        //ReadMouseAxisInput();
        RotateWhenCursorIsLocked();
        ReadScrollWheelInput();

        //transform.rotation = Quaternion.LookRotation(_lockOnTarget.position - transform.position, Vector3.up);
    }

    private void ReadScrollWheelInput()
    {
        _cameraZoom -= Input.GetAxis("Mouse ScrollWheel");
        _cameraZoom = Mathf.Clamp(_cameraZoom, MIN_Z, MAX_Z);
    }

    private void RotateWhenCursorIsLocked()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            m_rotationY += Input.GetAxis("Mouse X"); //rotating around the Y axis with the Mouse X input
    }

    private void ReadMouseAxisInput()
    {
        if (Input.GetButton("Fire2"))
        {
            m_rotationY += Input.GetAxis("Mouse X"); //rotating around the Y axis with the Mouse X input
            m_rotationX -= Input.GetAxis("Mouse Y"); //rotating around the X axis with the Mouse Y Input

            m_rotationX = Mathf.Clamp(m_rotationX, MIN_X, MAX_X); //Restricts camera movement to the values of MIN and MAX X
        }
    }

    private void SetCameraTarget()
    {
        m_cameraTarget = m_player.transform.position;
        m_cameraTarget.y += _cameraTargetYOffset;
        m_cameraTarget.x += _cameraTargetXOffset;
    }

    private static void UpdateCursorLockMode()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void LateUpdate() //LateUpdate happens after rendering is done. Gives better effect for camera transforms
    {
        Quaternion cameraRotation = Quaternion.Euler(m_rotationX, m_rotationY, 0);
        Vector3 cameraOffset = new Vector3(_cameraRotationXOffset, _cameraRotationYOffset, -_cameraZoom);
        transform.position = m_cameraTarget + cameraRotation * cameraOffset;

        transform.LookAt(m_cameraTarget);
        _cinemachineManager.SetCameraOffsetZ(_cameraZoom);
    }

    public Vector3 GetForwardVector() //Call this in PlayerLogic so player can face this direction
    {
        // Get forward vector of the camera without the rotations
        Quaternion rotation = Quaternion.Euler(0, m_rotationY, 0);
        return rotation * Vector3.forward;
    }
}

