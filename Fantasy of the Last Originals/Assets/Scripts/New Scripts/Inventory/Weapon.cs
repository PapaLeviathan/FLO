using System;

public class Weapon : Item
{
    private Inventory _playerInventory;

    private void Start()
    {
        _playerInventory = _player.GetComponent<Inventory>();
    }

    void Update()
    {
        if (_player.Stance == PlayerStance.Stance4)
        {
            _playerInventory = GetComponentInParent<Inventory>();

            if (_player.Animator.GetBool("Attacking"))
            {
                if (_wasEquipped)
                    return;

                if (_playerInventory != null)
                {
                    _playerInventory.Equip(this);
                    _wasEquipped = true;
                }
            }
            else
            {
                if (_playerInventory != null)
                {
                    _playerInventory.SheathWeapon(this);
                    _wasEquipped = false;
                }
            }
        }
        else
        {
            if (_playerInventory != null)
            {
                _playerInventory.SheathWeapon(this);
                _wasEquipped = false;
            }
        }
    }
}