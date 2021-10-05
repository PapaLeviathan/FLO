using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    GameObject[] _enemiesToAttack;
    GameObject _player;
    //float _enemyDetectRange = 4f;
    float[] _distances;
    
    void Awake()
    {
        _enemiesToAttack = GameObject.FindGameObjectsWithTag("Enemy");
        _player = GameObject.FindGameObjectWithTag("Player");
        _distances = new float[_enemiesToAttack.Length];
    }
    void Update()
    {
        if (_enemiesToAttack == null)
            return;
        


        for (int i = 0; i <= _enemiesToAttack.Length -1; i++)
        {
            _distances[i] = Vector3.Distance(_enemiesToAttack[i].transform.position, _player.transform.position);
            Debug.Log(_distances[i]);
        }
        foreach(float distance in _distances)
        {
        
            for(int i = 0; i <= _enemiesToAttack.Length - 1; i++)
            {
                if(distance != _distances[i])
                {
                    if(distance < _distances[i])
                    {
                        float enemyDistance = Vector3.Distance(_enemiesToAttack[i].transform.position, _player.transform.position);
                    }
                }
            }
        }
    }
}
