using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
public class PlayerCamera : MonoBehaviour
{
    private PhotonView _view;
    [SerializeField] private CinemachineVirtualCamera _vc;
    [SerializeField] private AudioListener _listener;

    private void Start()
    {
        
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            _listener = Camera.main.GetComponent<AudioListener>();
            _listener.enabled = true;
            _vc.Priority = 1; 
        }
        else
        {
            _vc.Priority = 0;
        }
    }
    
    
}
