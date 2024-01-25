using System;
using Photon.Pun;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    private GameObject _flashLight;

    [SerializeField]
    private AudioSource _turnOff;
    [SerializeField]
    private AudioSource _turnOn;

    private bool _on;
    private bool _off;

    private PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    void Start()
    {
        _turnOff = GetComponent<AudioSource>();
        _turnOn = GetComponent<AudioSource>();
        _off = false;
        _on = true;
        _flashLight.SetActive(true);
    }

 
    void Update()
    {
        if(_view.IsMine)
            LightOnOff();
    }
    
    
    private void LightOnOff()
    {
        if (_off && Input.GetKeyDown(KeyCode.F))
        {
            //_flashLight.SetActive(true);
            _view.RPC(nameof(OnLight), RpcTarget.All);
            _turnOn.Play();
            _off = false;
            _on = true;
        }
        else if (_on && Input.GetKeyDown(KeyCode.F))
        {
            //_flashLight.SetActive(false);
            _view.RPC(nameof(OffLight), RpcTarget.All);
            _turnOn.Play();
            _off = true;
            _on = false;
        }
    }

    [PunRPC]
    private void OffLight()
    {
        _flashLight.SetActive(false);
    }

    [PunRPC]
    private void OnLight()
    {
        _flashLight.SetActive(true);
    }
}
