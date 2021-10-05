using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlatformLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Sliding player back");

        if (collider.CompareTag("Enemy"))
        {
                transform.parent.parent.position -= Vector3.back;
        }
    }
}
