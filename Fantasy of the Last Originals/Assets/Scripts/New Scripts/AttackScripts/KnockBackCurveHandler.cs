using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBackCurveHandler : MonoBehaviour
{
    [SerializeField] float _trajectorySpeed = 3f;
    [SerializeField] float _enemyGravityResetValue = 0.1f;

    AnimationCurve _trajectorySpeedCurve;
    AnimationCurve _trajectoryCurve;
    Vector3 _trajectoryDirection;

    Vector3 _currentPosition;
    Vector3 _targetPosition;
    
    StunHandler _stunHandler;
    Rigidbody _rb;
    NavMeshAgent _nav;

    float _timeInTrajectory;
    bool _isInKnockBack;
    float _knockBackDuration;

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _stunHandler = GetComponent<StunHandler>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_isInKnockBack) //if _stunTime is 2 seconds, this code while run for 2 seconds
        {
            if (!_stunHandler._groundCheck.UpdateIsGrounded())
                _stunHandler.SetDownPull(_enemyGravityResetValue);

            _timeInTrajectory += Time.deltaTime;
            _trajectorySpeed = _trajectorySpeedCurve.Evaluate(_timeInTrajectory);

            float trajectoryRatio = _timeInTrajectory / _trajectorySpeed;
            // _trajectoryInterpolationValue = (_trajectoryInterpolationValue + _trajectorySpeed) % 1f;

            Vector3 trajectoryCurveVector = _trajectoryCurve.Evaluate(trajectoryRatio) * _trajectoryDirection;

            //make a curve that makes trajectoryRatio go from 0 to 1 more or less gradually
            _nav.enabled = false;
            _rb.MovePosition(Vector3.Lerp(_currentPosition, _targetPosition, trajectoryRatio) + trajectoryCurveVector);
            StartCoroutine(ResetIsInKnockBack(_knockBackDuration));
        }
        else
        {
            _timeInTrajectory = 0;
        }
    }

    IEnumerator ResetIsInKnockBack(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        _isInKnockBack = false;
    }

    public void SetTargetPosition(Vector3 vector)
    {
        _targetPosition = vector;
    }
    public void SetCurrentPosition(Vector3 vector)
    {
        _currentPosition = vector;
    }

    public void SetStun(bool stun, float duration)
    {
        _isInKnockBack = stun;
        _knockBackDuration = duration;
    }

    public void SetTrajectoryCurve(AnimationCurve curve)
    {
        _trajectoryCurve = curve;
    }

    public void SetTrajectorySpeed(AnimationCurve trajectorySpeedCurve)
    {
        _trajectorySpeedCurve = trajectorySpeedCurve;
    }

    public void SetTrajectoryVector(Vector3 trajectoryVector)
    {
        _trajectoryDirection = trajectoryVector;
    }
}
