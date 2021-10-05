using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSoundHandler : MonoBehaviour
{
    [SerializeField] bool _isPlayer;

    [SerializeField] AudioClip impact1, impact2, impact3;
    [SerializeField] AudioClip _deathGrunt1;

    AudioSource _audioSource;
    HealthLogic _healthLogic;
    EnemyDeathLogic _enemyDeathLogic;
    PlayerDeathLogic _playerDeathLogic;

    int _randomImpactSound;

    public int RandomImpactSound { get { return _randomImpactSound; } }

    // Start is called before the first frame update
    void Start()
    {
        _healthLogic = GetComponent<HealthLogic>();
        _audioSource = GetComponent<AudioSource>();

        if (!_isPlayer)
        {
            _enemyDeathLogic = GetComponent<EnemyDeathLogic>();
        }
        else if (_isPlayer)
        {
            _playerDeathLogic = GetComponent<PlayerDeathLogic>();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_healthLogic == null)
            return;
        var impact = collider.GetComponent<HitBox>();
        if (impact == null)
            return;

    }
    void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlayHitStunSound()
    {
        _randomImpactSound = Random.Range(1, 4);
        if (_randomImpactSound == 1)
            _audioSource.PlayOneShot(impact1);
        if (_randomImpactSound == 2)
            _audioSource.PlayOneShot(impact2);
        if (_randomImpactSound == 3)
            _audioSource.PlayOneShot(impact3);

    }


    public void PlayDeathSound()
    {
        _audioSource.PlayOneShot(_deathGrunt1);
    }
}
