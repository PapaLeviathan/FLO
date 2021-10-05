using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveTriggerTest : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Transform pointAToB;

    float interpolateAmount;
    bool _hit = false;
    GameObject _target;

    private void Update()
    {
        interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        if (_hit)
        {
            _target.transform.position =  Vector3.Lerp(transform.position, pointB.position, interpolateAmount);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        _target = other.gameObject;
        _hit = true;
    }
}
