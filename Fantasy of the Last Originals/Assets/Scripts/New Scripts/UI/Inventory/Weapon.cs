using System;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private HitBox _hitbox;
    private Inventory _playerInventory;

    private void Start()
    {
        _playerInventory = _player.GetComponent<Inventory>();
    }

    void Update()
    {
        if (_player.Stance == PlayerStance.Stance4 && WasEquipped)
        {
            if (_playerInventory == null)
                return;
            
            if (_player.Animator.GetBool("Attacking"))
            {
                _playerInventory.UnSheathWeapon(this);
            }
            else
            {
                _playerInventory.SheathWeapon(this);
            }
        }
    }

    public void SetHitBox()
    {
        _hitbox.gameObject.SetActive(!_hitbox.gameObject.activeSelf);
    }
}