using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set;}

    public List<GameObject> players;

    private PhotonView _view;

    private void Awake()
    {
        if (instance == null)
            instance = this; 
        else 
            Destroy(gameObject);

        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        ResetPlayerDead();
    }

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
    }
    
    public IEnumerator DeadAction(GameObject player, GameObject cam)
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Da hoi sinh");
        Transform pointSpawn = SpawnManager.instance.GetSpawnPointPlayer();
        player.transform.position = pointSpawn.position;
        cam.SetActive(true);
        player.SetActive(true);
    }
    
    private void ResetPlayerDead()
    {
        foreach (var player in PlayerManager.instance.players)
        {
            if (player.GetComponent<PlayerState>().isDie == true)
            {
                player.GetComponent<PlayerState>().isDie = false;
                StartCoroutine(DeadAction(player, player.GetComponent<PlayerCamera>().newCam));
            }
        }
    }
}
