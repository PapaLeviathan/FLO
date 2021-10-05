using System.Collections;
using UnityEngine;

public class UIBinding : MonoBehaviour
{
    private IEnumerator Start()
    {
        var player = FindObjectOfType<Player>();

        while (player == null)
        {
            yield return null;

            player = FindObjectOfType<Player>();
        }

        GetComponent<UIInventoryPanel>().Bind(player.GetComponent<Inventory>());
        //player.GetComponent<Inventory>().Slots = GetComponent<UIInventoryPanel>().Slots;
    }
}