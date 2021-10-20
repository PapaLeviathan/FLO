using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TriggerStunAnimation : MonoBehaviour
{
    public abstract void TriggerAnimation(Collider collider);
}