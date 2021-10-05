using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;

public class SetTargetGroupFollow : SetFollow
{
    CinemachineTargetGroup _targetGroup;

    private IEnumerator Start()
    {

        if(!_view.IsMine)
            yield break;
        
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        yield return new WaitForSeconds(.1f);
        var player = FindObjectOfType<Player>();
        
        if (!player)
            yield break;
        
        _targetGroup.m_Targets[0].target = player.gameObject.transform.Find("CamLookAtAndFollow");
    }
}
