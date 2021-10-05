using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    private void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            Vector3 newPosition = transform.forward * animator.GetFloat("RootMotionZ") * Time.deltaTime;

            transform.position += newPosition;
        }
    }
}
