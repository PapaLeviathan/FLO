using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySlashEffect : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;

    private ParticleSystem _particles;

    // Start is called before the first frame update
    void Start()
    {
        _particles = _particlePrefab.GetComponent<ParticleSystem>();
    }

    public void PlayParticle()
    {
        _particles.Play();
    }
}