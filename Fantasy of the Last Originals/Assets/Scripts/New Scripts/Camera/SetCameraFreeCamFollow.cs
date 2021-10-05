using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class SetCameraFreeCamFollow : SetFollow
{
    private IEnumerator Start()
    {
        if (!_view.IsMine)
            yield break;

        var vcam = GetComponent<CinemachineFreeLook>();
        
        yield return new WaitForSeconds(.1f);
        var player = FindObjectOfType<Player>();
        if (!player)
            yield break;

        vcam.LookAt = player.gameObject.transform.Find("CamLookAtAndFollow");
        vcam.Follow = player.gameObject.transform.Find("CamLookAtAndFollow");
    }
}