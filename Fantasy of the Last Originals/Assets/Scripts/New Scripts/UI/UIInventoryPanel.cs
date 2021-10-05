using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryPanel : MonoBehaviour
{
    public event Action OnSelectionChanged;
    
    [ContextMenu("Swap 01")]
    public void Swap01() => _inventory.Move(0, 1);
    
    private Inventory _inventory;
    public UIInventorySlot[] Slots;

    public int SlotCount => Slots.Length;
    
    public UIInventorySlot Selected { get; set; }
    
    private void Awake()
    {
        Slots = FindObjectsOfType<UIInventorySlot>()
            .OrderBy(t => t.SortIndex)
            .ThenBy(t => t.name)
            .ToArray();

        RegisterSlotsForClickCallback();
    }
    
    
    private void RegisterSlotsForClickCallback()
    {
        foreach (var slot in Slots)
        {
            slot.OnSlotClicked += HandleSlotClicked;
        }
    }
    
    private void HandleSlotClicked(UIInventorySlot slot)
    {
        if (Selected != null)
        {
            if (SlotCanHoldItem(slot, Selected.Item))
            {
                Swap(slot);
                Selected.BecomeUnSelected();
                Selected = null;
            }
        }
        else if (slot.IsEmpty == false)
        {
            Selected = slot;
            Selected.BecomeSelected();
        }
        OnSelectionChanged?.Invoke();
    }

    private bool SlotCanHoldItem(UIInventorySlot slot, Item selectedItem)
    {
        return slot.SlotType == SlotType.General || slot.SlotType == selectedItem.SlotType;
    }

    private void Swap(UIInventorySlot slot)
    {
        var slotIndex1 = GetSlotIndex(Selected);
        _inventory.Move(GetSlotIndex(Selected), GetSlotIndex(slot));
    }

    private int GetSlotIndex(UIInventorySlot selected)
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if (Slots[i] == selected)
                return i;
        }

        return -1;
    }
    
    public void Bind(Inventory inventory)
    {
        if (_inventory != null)
        {
            _inventory.ItemPickedUp -= HandleItemPickedUp;
            _inventory.OnItemChanged -= HandleItemChanged;
        }

        _inventory = inventory;

        if (_inventory != null)
        {
            _inventory.ItemPickedUp += HandleItemPickedUp;
            _inventory.OnItemChanged += HandleItemChanged;
            RefreshSlots();
        }
        else
        {
            ClearSlots();
        }
    }

    private void HandleItemChanged(int slotNumber)
    {
        Slots[slotNumber].SetItem(_inventory.GetItemInSlot(slotNumber));
    }
    
    private void ClearSlots()
    {
        foreach (var slot in Slots)
        {
            slot.Clear();
        }
    }

    private void RefreshSlots()
    {
        for (var i = 0; i < Slots.Length; i++)
        {
            var slot = Slots[i];
            if (_inventory.Items.Count > i)
                slot.SetItem(_inventory.Items[i]);
            else
                slot.Clear();
        }
    }


    private void HandleItemPickedUp(Item item)
    {
        RefreshSlots();
    }

}