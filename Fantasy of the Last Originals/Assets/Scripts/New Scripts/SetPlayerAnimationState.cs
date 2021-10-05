using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerAnimationState : MonoBehaviour
{
    Animator _animator;

    public Animator Animator { get { return _animator; } }
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

}
