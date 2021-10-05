using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TriggerStunAnimation : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        TriggerAnimation(other);
    }
    protected abstract void TriggerAnimation(Collider collider);
}