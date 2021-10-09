using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColliderLogic : MonoBehaviour
{
    //PlayerLogic m_playerLogic;

    private void Start()
    {
        //m_playerLogic = GetComponentInParent<PlayerLogic>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //m_playerLogic.DetectGroundCollision();
    }
}
