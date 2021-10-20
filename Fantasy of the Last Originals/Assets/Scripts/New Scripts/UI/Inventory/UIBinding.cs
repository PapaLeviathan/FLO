using System.Collections;
using UnityEngine;

public class UIBinding : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        GetComponent<UIInventoryPanel>().Bind(_player.GetComponent<Inventory>());
    }
}