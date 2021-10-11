using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlatformLogic : MonoBehaviour
{
    [SerializeField] private Transform _rootTransform;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Distance Collider"))
        {
            _rootTransform.position -= Vector3.back;
        }
    }
}