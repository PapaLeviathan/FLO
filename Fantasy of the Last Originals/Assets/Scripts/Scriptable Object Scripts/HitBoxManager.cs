using System;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    private HitBox _hitBox;
    public HitBoxDefinition CurrentHitBoxDefinition { get; set; }


    public void Update()
    {
        if (GetComponentInChildren<HitBox>())
            _hitBox = GetComponentInChildren<HitBox>();
    }

    public void SetHitBoxDefinition(HitBoxDefinition hitBoxDefinition)
    {
        CurrentHitBoxDefinition = hitBoxDefinition;
    }
}