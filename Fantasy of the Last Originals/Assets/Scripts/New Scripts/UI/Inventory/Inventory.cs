using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public event Action<int> OnItemChanged;
    public event Action<Item> ItemPickedUp;

    [SerializeField] Transform _rightHand;
    [SerializeField] private Transform _scabbard;
    [SerializeField] private Transform _sheathRoot;

    private Item[] _items = new Item[DEFAULT_INVENTORY_SIZE];
    private Transform _itemRoot;
    public UIInventorySlot[] Slots { get; set; }

    public const int DEFAULT_INVENTORY_SIZE = 28;
    public Item ActiveItem { get; private set; }
    public List<Item> Items => _items.ToList();
    public int Count => _items.Count(t => t != null);
    public Weapon ActiveWeapon { get; set; }


    private void Awake()
    {
        _itemRoot = new GameObject("Items").transform;
        _itemRoot.transform.SetParent(transform);
    }


    public void PickUp(Item item, int? slot = null)
    {
        Debug.Log($"Picked Up {item.gameObject.name}");
        if (slot.HasValue == false)
            slot = FindFirstAvailableSlot();
        if (slot.HasValue == false)
        {
            return;
        }

        _items[slot.Value] = item;
        item.transform.SetParent(_itemRoot);
        ItemPickedUp?.Invoke(item);
        item.WasPickedUp = true;
        StartCoroutine(SetInactive(item));
    }

    private bool InventoryIsFull()
    {
        if (_items.Length == Slots.Length)
            return true;

        return false;
    }

    private bool FindFirstMatchingSlotType(Item item)
    {
        if (item.SlotType == SlotType.General)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].SlotType == SlotType.General)
                    if (Slots[i].IsEmpty)
                        return true;
            }
        }
        else if (item.SlotType == SlotType.Weapon)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].SlotType == SlotType.Weapon)
                    if (Slots[i].IsEmpty)
                        return true;
            }
        }

        return false;
    }

    private bool IsAvailableWeaponSlot()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].SlotType == SlotType.Weapon && Slots[i].IsEmpty)
                return true;
        }

        return false;
    }

    private bool IsAvailableGeneralSlot()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].SlotType == SlotType.General)
                if (Slots[i].IsEmpty)
                    return true;
        }

        return false;
    }

    private int? FindFirstAvailableSlot()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null)
                return i;
        }

        return null;
    }

    private IEnumerator SetInactive(Item item)
    {
        yield return new WaitForSeconds(.01f);

        item.gameObject.SetActive(false);
    }

    public void Equip(Item item)
    {
        item.transform.SetParent(_rightHand);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.WasEquipped = true;
        ActiveItem = item;
    }

    public void UnSheathWeapon(Item item)
    {
        item.transform.SetParent(_rightHand);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }
    
    public void SheathWeapon(Item item)
    {
        item.transform.SetParent(_sheathRoot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        ActiveItem = null;
    }

    public void Move(int sourceSlot, int destinationSlot)
    {
        var destinationItem = _items[destinationSlot];
        _items[destinationSlot] = _items[sourceSlot];
        _items[sourceSlot] = destinationItem;

        OnItemChanged?.Invoke(destinationSlot);
        OnItemChanged?.Invoke(sourceSlot);
    }

    public Item GetItemInSlot(int slot)
    {
        return _items[slot];
    }
}