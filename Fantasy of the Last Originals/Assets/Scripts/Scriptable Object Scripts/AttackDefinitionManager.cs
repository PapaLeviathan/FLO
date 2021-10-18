using System;
using Unity.Mathematics;
using UnityEngine;

public class AttackDefinitionManager : MonoBehaviour
{
    public AttackDefinition CurrentAttackDefinition { get; set; }

    private HitBox _hitBox;

    public void Update()
    {
        if (GetComponentInChildren<HitBox>())
            _hitBox = GetComponentInChildren<HitBox>();
    }

    public void SetAttackDefinition(AttackDefinition attackDefinition)
    {
        CurrentAttackDefinition = attackDefinition;
    }

    public void SetDefinitionEffectPosition(EffectPositionDefinition effectPositionDefinition)
    {
        CurrentAttackDefinition.EffectPosition = transform.InverseTransformPoint(effectPositionDefinition.EffectPosition);
    }

    public void PlayVFX()
    {
        var effect = Instantiate(CurrentAttackDefinition.Effect);
        effect.transform.localPosition = _hitBox.EffectPosition.position;
        effect.transform.localRotation = _hitBox.EffectPosition.rotation;
        //Instantiate(CurrentAttackDefinition.Effect, CurrentAttackDefinition.EffectPosition, CurrentAttackDefinition.EffectRotation);
    }
}