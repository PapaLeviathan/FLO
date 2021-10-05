using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class SetFollow : MonoBehaviour
{
    protected PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }
}