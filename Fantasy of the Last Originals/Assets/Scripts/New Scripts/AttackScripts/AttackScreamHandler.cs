using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScreamHandler : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;
    AudioSource _audioSource;
    
    public AudioClip[] Clips {get {return _clips; } }
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayScreamSound(int random)
    {
        _audioSource.PlayOneShot(_clips[random]);
    }
}