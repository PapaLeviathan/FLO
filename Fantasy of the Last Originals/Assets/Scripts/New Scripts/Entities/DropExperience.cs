using UnityEngine;

public class DropExperience : MonoBehaviour
{
    [SerializeField] ParticleSystem _experienceParticles;

    EnemyDeathLogic _enemyDeathLogic;
    HealthLogic _healthLogic;


    // Start is called before the first frame update
    void Start()
    {
        _experienceParticles = GetComponent<ParticleSystem>();
        _healthLogic = GetComponent<HealthLogic>();
        _enemyDeathLogic = GetComponent<EnemyDeathLogic>();

    }

    public void DropExperienceParticles()
    {
        _experienceParticles.Play();
    }

}

