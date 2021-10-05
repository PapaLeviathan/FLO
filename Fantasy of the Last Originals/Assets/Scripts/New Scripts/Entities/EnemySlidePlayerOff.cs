using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlidePlayerOff : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        Vector3 normal = contact.normal;
        Debug.Log("Sliding player back");

        if (collision.collider.CompareTag("Player"))
        {
            if (normal.y <= -.1)

            {
                collision.collider.gameObject.transform.position -= Vector3.back;
            }
        }
    }
}
