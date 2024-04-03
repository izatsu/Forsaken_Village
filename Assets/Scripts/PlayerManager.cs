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

    public int countPlayerDie = 0;
    public bool isGameOver = false;
    public bool isOnUIGameOver = false;
    public GameObject uiGameOver;

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
        if ((players.Count > 0) && (countPlayerDie >= players.Count) && !isOnUIGameOver)
        {
            //_view.RPC(nameof(GameOver), RpcTarget.AllBuffered);
            isGameOver = true;
            isOnUIGameOver = true;
            AudioListener.pause = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            uiGameOver.SetActive(true);
        }
            
        
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
        countPlayerDie--;
        Transform pointSpawn = SpawnManager.instance.GetSpawnPointPlayer();
        player.transform.position = pointSpawn.position;
        cam.SetActive(true);
        player.SetActive(true);
    }

    private void ResetPlayerDead()
    {
        if (!isGameOver)
        {
            foreach (var player in players)
            {
                if (player.GetComponent<PlayerState>().isDie == true)
                {
                    countPlayerDie++;
                    player.GetComponent<PlayerState>().isDie = false;
                    StartCoroutine(DeadAction(player, player.GetComponent<PlayerCamera>().newCam));
                }
            }
        }
    }
}
