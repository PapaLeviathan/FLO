using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnWhileAttacking : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] bool turnWithMouseMode = true;
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (turnWithMouseMode == false)
                turnWithMouseMode = true;
            else
                turnWithMouseMode = false;
        }
    }

    void LateUpdate()
    {
        if (turnWithMouseMode)
        {
            if (Input.GetButtonDown("Light Attack"))
                transform.rotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
            if (_animator.IsInTransition(0) || _animator.GetCurrentAnimatorStateInfo(0).IsTag("Transition"))
                transform.rotation = Quaternion.Euler(0f, _camera.transform.eulerAngles.y, 0f);
        }
    }
}
