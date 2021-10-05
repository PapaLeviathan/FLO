using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaunchPhysics : MonoBehaviour
{
    CharacterController _characterController;
    Rigidbody _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();    
    }
    public void LaunchUp()
    {
        transform.position += Vector3.up;
        //StartCoroutine(MoveOverTime(Vector3.up, 10));
    }
    IEnumerator MoveOverTime(Vector3 velocity, float time)
    {
        float knockUpTimer = time;

        while (knockUpTimer > 0f)
        {
            transform.position += velocity * Time.deltaTime;
            knockUpTimer -= Time.deltaTime;
            yield return null;
        }
    }
    public void LaunchBack()    
    {
        var knockBack = new Vector3(0, 0, -1);
        _characterController.Move(knockBack);
    }
}
