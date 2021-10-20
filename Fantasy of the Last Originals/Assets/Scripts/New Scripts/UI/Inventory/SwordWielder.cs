using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWielder : MonoBehaviour
{
    [SerializeField] GameObject _sword;
    [SerializeField] GameObject _hand;

    public void GrabSword()
    {
        _sword.transform.position = _hand.transform.position;
    }
}
