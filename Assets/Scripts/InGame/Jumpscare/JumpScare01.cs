using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class JumpScare01 : MonoBehaviour
{
    private PhotonView _view;

    public GameObject ghost;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _view.RPC(nameof(SetActiveGhost), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SetActiveGhost()
    {
        ghost.SetActive(false);
    }
}
