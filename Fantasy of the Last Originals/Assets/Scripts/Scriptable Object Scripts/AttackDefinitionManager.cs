using System;
using Unity.Mathematics;
using UnityEngine;

public class AttackDefinitionManager : MonoBehaviour
{
    public AttackDefinition CurrentAttackDefinition { get; set; }
    private Inventory _inventory;

    private HitBox _hitBox;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
    }

    public void Update()
    {
        if (GetComponentInChildren<HitBox>())
            _hitBox = GetComponentInChildren<HitBox>();
    }

    public void SetAttackDefinition(AttackDefinition attackDefinition)
    {
        CurrentAttackDefinition = attackDefinition;
    }

    public void SetActiveWeaponHitBox()
    {
        if (_inventory.ActiveWeapon)
            _inventory.ActiveWeapon.SetHitBox();
    }

    public void PlayVFX()
    {
        if (!_inventory.ActiveWeapon)
            return;
        
        var effect = Instantiate(CurrentAttackDefinition.Effect);
        effect.transform.localPosition = _hitBox.EffectPosition.position;
        effect.transform.localRotation = _hitBox.EffectPosition.rotation;
        //Instantiate(CurrentAttackDefinition.Effect, CurrentAttackDefinition.EffectPosition, CurrentAttackDefinition.EffectRotation);
    }
}