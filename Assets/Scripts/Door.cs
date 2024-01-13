using System;
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
    private void Start()
    {
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
                _isOpen = !_isOpen;
                inReach = false;
                _animDoor.SetBool("Open", _isOpen);
                _doorSound.Play();
                
            }
            else
            {
                Debug.Log("Cua bi khoa");
                _animDoor.Play("Lock");
                inReach = false;
            }
        }
    }
}
