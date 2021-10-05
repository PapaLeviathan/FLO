using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEventCaller : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<KnockBackHandler>();
        if (target == null)
            return;

        if (CameraSwitcher._turnOnTargetCam == false)
        {
            CameraSwitcher._turnOnTargetCam = true;
        }
    }
}