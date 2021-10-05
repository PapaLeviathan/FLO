using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
 
    public override void OnJoinedLobby()
    {
        SceneManager.LoadSceneAsync("Menu");
        SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
    }
}
