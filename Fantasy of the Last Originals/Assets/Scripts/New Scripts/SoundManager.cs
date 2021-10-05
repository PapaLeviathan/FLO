using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource m_audioSource;

    [Header("Stance 1 Sounds")]
    [SerializeField]
    AudioClip m_s1_Attack1;
    [SerializeField]
    AudioClip m_s1_Attack2;
    [SerializeField]
    AudioClip m_s1_Attack3;

    [Header("Stance 2 Sounds")]
    [SerializeField]
    AudioClip m_s2_Attack1;
    [SerializeField]
    AudioClip m_s2_Attack2;
    [SerializeField]
    AudioClip m_s2_Attack3;

    [Header("Stance 3 Sounds")]
    [SerializeField]
    AudioClip m_s3_Attack1;
    [SerializeField]
    AudioClip m_s3_Attack2;
    [SerializeField]
    AudioClip m_s3_Attack3;

    [Header("Stance 4 Sounds")]
    [SerializeField]
    AudioClip m_s4_Attack1;
    [SerializeField]
    AudioClip m_s4_Attack2;
    [SerializeField]
    AudioClip m_s4_Attack3;

    public void HandleAnimationSoundPass (string attackSound)
    {
        if(attackSound == "S1 Attack 1")
        {
            PlaySound(m_s1_Attack1);
        }
        if (attackSound == "S1 Attack 2")
        {
            PlaySound(m_s1_Attack2);
        }
        if (attackSound == "S1 Attack 3")
        {
            PlaySound(m_s1_Attack3);
        }

        if (attackSound == "S2 Attack 1")
        {
            PlaySound(m_s2_Attack1);
        }
        if (attackSound == "S2 Attack 2")
        {
            PlaySound(m_s2_Attack2);
        }
        if (attackSound == "S2 Attack 3")
        {
            PlaySound(m_s2_Attack3);
        }

        if (attackSound == "S3 Attack 1")
        {
            PlaySound(m_s3_Attack1);
        }
        if (attackSound == "S3 Attack 2")
        {
            PlaySound(m_s3_Attack2);
        }
        if (attackSound == "S3 Attack 3")
        {
            PlaySound(m_s3_Attack3);
        }

        if (attackSound == "S4 Attack 1")
        {
            PlaySound(m_s4_Attack1);
        }
        if (attackSound == "S4 Attack 2")
        {
            PlaySound(m_s4_Attack2);
        }
        if (attackSound == "S4 Attack 3")
        {
            PlaySound(m_s4_Attack3);
        }

        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance1)
        //{
        //    if (clipNumber == 1)
        //    {
        //        PlaySound(m_s1_Attack1);
        //    }
        //    if (clipNumber == 2)
        //    {
        //        PlaySound(m_s1_Attack2);
        //    }
        //    if (clipNumber == 3)
        //    {
        //        PlaySound(m_s1_Attack3);
        //    }
        //}
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance2)
        //{
        //    if (clipNumber == 1)
        //    {
        //        PlaySound(m_s2_Attack1);
        //    }
        //    if (clipNumber == 2)
        //    {
        //        PlaySound(m_s2_Attack2);
        //    }
        //    if (clipNumber == 3)
        //    {
        //        PlaySound(m_s2_Attack3);
        //    }
        //}
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance3)
        //{
        //    if (clipNumber == 1)
        //    {
        //        PlaySound(m_s3_Attack1);
        //    }
        //    if (clipNumber == 2)
        //    {
        //        PlaySound(m_s3_Attack2);
        //    }
        //    if (clipNumber == 3)
        //    {
        //        PlaySound(m_s3_Attack3);
        //    }
        //}
        //if (CombatManagerScript.instance.m_playerLogic.m_stance == PlayerStance.Stance4)
        //{
        //    if (clipNumber == 1)
        //    {
        //        PlaySound(m_s4_Attack1);
        //    }
        //    if (clipNumber == 2)
        //    {
        //        PlaySound(m_s4_Attack2);
        //    }
        //    if (clipNumber == 3)
        //    {
        //        PlaySound(m_s4_Attack3);
        //    }
        //}
    }

    private void PlaySound(AudioClip clip)
    {
        m_audioSource.PlayOneShot(clip);
    }
}
