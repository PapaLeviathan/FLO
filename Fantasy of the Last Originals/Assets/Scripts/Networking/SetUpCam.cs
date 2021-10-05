using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Player = Photon.Realtime.Player;

public class SetUpCam : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _cam;
    [SerializeField] private GameObject _virtualCams;

    private PhotonView _view;
    private Photon.Realtime.Player[] _playersOnline;

    private void Awake()
    {
        _playersOnline = PhotonNetwork.PlayerListOthers;
        if (_view.IsMine)
        {
            _cam.SetActive(true);
            _virtualCams.SetActive(true);
        }
    }
}
