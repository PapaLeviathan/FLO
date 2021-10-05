using System.Collections;
using Cinemachine;
using UnityEngine;
using Photon.Pun;

public class SetCameraTargetGroupFollow : SetFollow
{
    private IEnumerator Start()
    {

        if(!_view.IsMine)
            yield break;

        var vcam = GetComponent<CinemachineFreeLook>();
        
        yield return new WaitForSeconds(.1f);
        var player = FindObjectOfType<Player>();
        
        if (!player)
            yield break;

        vcam.Follow = player.gameObject.transform.Find("CamLookAtAndFollow");
    }
}