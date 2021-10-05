using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDeathLogic : MonoBehaviour
{
    [SerializeField] GameObject _deathCollider;
    Animator _animator;
    SkinnedMeshRenderer _meshRenderer;
    HealthLogic _healthLogic;
    ImpactSoundHandler _enemySoundHandler;
    DropExperience _dropExperienceHandler;
    KnockBackHandler _enemyLogic;

    bool _died = false;
    public bool Died { get { return _died; } }


    // Start is called before the first frame update
    void Start()
    {
        _enemyLogic = GetComponent<KnockBackHandler>();
        _dropExperienceHandler = GetComponent<DropExperience>();
        _enemySoundHandler = GetComponent<ImpactSoundHandler>();
        _healthLogic = GetComponent<HealthLogic>();
        _animator = GetComponent<Animator>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthLogic.CurrentHealthValue <= 0f && _died == false)
        {
            DropExperience();
            PlayDeathAnimation();
            PlayDeathSound();
            StartDestroyObjectCoroutine();
            DeactivateCollider();
            _deathCollider.SetActive(true);
            _died = true;
        }
    }

    void DropExperience()
    {
        _dropExperienceHandler.DropExperienceParticles();
    }

    void PlayDeathSound()
    {
        _enemySoundHandler.PlayDeathSound();
    }

    void DeactivateCollider()
    {
        GetComponent<Collider>().enabled = false;
    }

    void StartDestroyObjectCoroutine()
    {
        //Fade Mesh Renderer
        //StartCoroutine(FadeMeshRenderer());
        StartCoroutine(DisableObject());

    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(9f);
        Destroy(gameObject);
    }

    IEnumerator FadeMeshRenderer()
    {
        //var fadeTime = 0f;
        //while (fadeTime < 3f)
        //{
        //    //decrease alpha
        //    fadeTime += Time.deltaTime;
        yield return null;
        //}
        _meshRenderer.enabled = false;
    }

    void PlayDeathAnimation()
    {
        _animator.SetTrigger("Death");
    }

}

