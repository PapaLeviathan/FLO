using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveTest : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Transform pointAToB;

    float interpolateAmount;
    
    private void Update()
    {
        interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        pointAToB.position = Vector3.Lerp(pointA.position, pointB.position, interpolateAmount);
    }
}
