using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] CinemachineFreeLook _cinemachineFreeLook;
    public CinemachineFreeLook _cinemachineTargetGroupFreeLook;

    public Transform PlayerTransform => _playerTransform;
    
    CameraLogic _cam;

    private void Start()
    {
        _cam = GetComponent<CameraLogic>();
    }

    public void SetCameraOffsetX(float offset)
    {
        _cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = offset;
        _cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = offset;
        _cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = offset;
    }

    public void SetCameraOffsetY(float offset)
    {
        _cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = offset;
        _cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = offset;
        _cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = offset;

    }
    public void SetCameraOffsetZ(float offset)
    {
        if (_cinemachineFreeLook)
        {
            _cinemachineFreeLook.m_Orbits[0].m_Radius = offset;
            _cinemachineFreeLook.m_Orbits[1].m_Radius = offset;
            _cinemachineFreeLook.m_Orbits[2].m_Radius = offset;
        }
        if (_cinemachineTargetGroupFreeLook)
        {
        _cinemachineTargetGroupFreeLook.m_Orbits[0].m_Radius = offset;
        _cinemachineTargetGroupFreeLook.m_Orbits[1].m_Radius = offset;
        _cinemachineTargetGroupFreeLook.m_Orbits[2].m_Radius = offset;
        }
    }

    public void SetCameraXAxis(float axis)
    {
        _cinemachineTargetGroupFreeLook.m_XAxis.m_InputAxisValue = axis;
    }
}
