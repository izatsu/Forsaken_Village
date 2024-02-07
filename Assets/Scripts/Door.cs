using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    private Animator _animDoor;
    private AudioSource _doorSound;

    public bool inReach;

    private bool _isOpen;

    public int idDoor = 0;
    public bool isLock = true;

    private PhotonView _view; 
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _isOpen = false;
        inReach = false;
        _animDoor = GetComponent<Animator>();
        _doorSound = GetComponent<AudioSource>();
    }


    private void LateUpdate()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (!isLock)
            {
                _view.RPC(nameof(OpenCloseDoor), RpcTarget.All);
            }
            else
            {
                _view.RPC(nameof(IsLockDoor), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenCloseDoor()
    {
        isLock = false;
        _isOpen = !_isOpen;
        inReach = false;
        _animDoor.SetBool("Open", _isOpen);
        _doorSound.Play();
    }

    [PunRPC]
    private void IsLockDoor()
    {
        Debug.Log("Cua bi khoa");
        _animDoor.Play("Lock");
        inReach = false;
    }
}
