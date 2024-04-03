using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Jumpscare : MonoBehaviour
{
    public GameObject ghost;
    public GameObject _pointTran;
    private bool _isOn = false;

    private PhotonView _view;

    public float timeActive = 5f;
    private float _timeOff;
    public float timeReset = 180f;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _timeOff = timeActive;
        _pointTran.transform.position = ghost.transform.position;
    }

    private void Update()
    {
        if (_isOn)
        {
            timeActive -= Time.deltaTime;
            timeReset -= Time.deltaTime;
            
            if (timeActive <= 0f)
            {
                _view.RPC(nameof(ResetGhost), RpcTarget.AllBuffered);
            }

            if (timeReset <= 0)
            {
                _view.RPC(nameof(ResetJumpScare), RpcTarget.AllBuffered);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isOn)
        {
            _view.RPC(nameof(SetActiveGhost), RpcTarget.All);
        }
    }

    [PunRPC]
    private void SetActiveGhost()
    {
        _isOn = true;
        ghost.SetActive(true);
    }

    [PunRPC]
    private void ResetGhost()
    {
        ghost.SetActive(false);
        ghost.transform.position = _pointTran.transform.position;
        timeActive = _timeOff;
    }

    [PunRPC]
    private void ResetJumpScare()
    {
        _isOn = false;
        timeActive = _timeOff;
        timeReset = 180f;
    }
}
