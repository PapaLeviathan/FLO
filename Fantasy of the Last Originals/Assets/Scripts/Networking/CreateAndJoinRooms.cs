using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
   [SerializeField] private TMP_InputField _createInput;
   [SerializeField] private TMP_InputField _joinInput;

   public void CreateRoom()
   {
      PhotonNetwork.CreateRoom(_createInput.text);
   }

   public void JoinRoom()
   {
      PhotonNetwork.JoinRoom(_joinInput.text);
   }

   public override void OnJoinedRoom()
   {
      PhotonNetwork.LoadLevel("Level1");
   }
}
