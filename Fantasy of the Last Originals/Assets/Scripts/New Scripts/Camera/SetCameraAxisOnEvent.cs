using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraAxisOnEvent : MonoBehaviour
{
    CinemachineManager _cinemachineManager;
    // Start is called before the first frame update
    void Start()
    {
        _cinemachineManager = GetComponent<CinemachineManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //set camera x axis upon entering attack mode
    }
}
