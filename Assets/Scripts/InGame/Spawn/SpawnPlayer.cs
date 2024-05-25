using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public static SpawnPlayer instance { get; private set; }

    public GameObject prefabPlayer; 
    private GameObject _player;

    private PhotonView _view;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //_players = new List<GameObject>();
        if (PhotonNetwork.IsConnected)
        {
            Spawn();
        }
    }

   
    public void Spawn()
    {
        Transform pointSpawn = SpawnManager.instance.GetSpawnPointPlayer();
        _player = PhotonNetwork.Instantiate(prefabPlayer.name, pointSpawn.position, pointSpawn.rotation);
    }
    
    
    
}
