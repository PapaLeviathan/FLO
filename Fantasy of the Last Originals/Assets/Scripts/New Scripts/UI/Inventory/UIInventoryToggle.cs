using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;

    private void Start()
    {
        _inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
        }
    }
}
