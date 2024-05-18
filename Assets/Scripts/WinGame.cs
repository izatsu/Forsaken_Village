using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WinGame : MonoBehaviour
{

    private PhotonView _view;

    public GameObject Portal;
    private bool _isOpen = false;

    public bool winGame = false;

    public GameObject videoPortalCut;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        Portal.SetActive(false);
    }

    private void Update()
    {
        if (EnemyState.instance.isDie && !_isOpen)
        {
            _isOpen = true;
            _view.RPC(nameof(SetActive), RpcTarget.AllBuffered);
            videoPortalCut.SetActive(true);
            Invoke(nameof(TurnOffVideo), 7f);
        }

    }
    private void TurnOffVideo()
    {
        videoPortalCut.SetActive(false);
    }

    [PunRPC]
    private void SetActive()
    {
        Portal.SetActive(true);
    }
}