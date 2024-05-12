using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject boss;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _view.RPC(nameof(SetOffBoss), RpcTarget.AllBuffered);
        _view.RPC(nameof(ActiveBoss), RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void ActiveBoss()
    {
        Invoke(nameof(SetActiveBoss), 60f);
    }
    
    private void SetActiveBoss()
    {
        boss.SetActive(true);
    }

    [PunRPC]
    private void SetOffBoss()
    {
        boss.SetActive(false);
    }
}
