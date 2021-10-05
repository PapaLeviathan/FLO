using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;

    private void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(_minX, _maxX), -25.46186f, Random.Range(_minZ, _maxZ));
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, randomPosition, Quaternion.identity);
        
        player.transform.Find("Character Cam").
            gameObject.SetActive(true);
        player.transform.Find("Virtual Cams").
            gameObject.SetActive(true);
    }
}
