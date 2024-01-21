using System;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public static SpawnPlayer instance { get; private set; }

    public GameObject prefabPlayer; 
    private GameObject _player; 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
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
