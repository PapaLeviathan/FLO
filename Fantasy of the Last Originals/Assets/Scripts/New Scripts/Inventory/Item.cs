using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public event Action OnPickedUp;

    [SerializeField] private Sprite _icon;
    [SerializeField] private SlotType _slotType;
    
    protected Player _player;
    protected bool _wasEquipped;
    
    public Sprite Icon => _icon;
    public SlotType SlotType => _slotType;
    public bool WasPickedUp { get; set; }

    void Awake()
    {
        _player = FindObjectOfType<Player>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (WasPickedUp)
            return;

        var inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.PickUp(this);
            OnPickedUp?.Invoke();
        }
    }


    void OnValidate()
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
            if (collider.isTrigger == false)
                collider.isTrigger = true;
    }
}