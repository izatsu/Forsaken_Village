using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ChairJumpScare : MonoBehaviour
{
    private Animator _anim;
    private PhotonView _view;
    

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _view.RPC(nameof(JumpScare), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void JumpScare()
    {
        _anim.SetBool("hasPlayer", true);
    }
}
