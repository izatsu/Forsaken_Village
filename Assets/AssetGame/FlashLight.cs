using System.Collections;
using System.Collections.Generic;
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
    
    void Start()
    {
        _turnOff = GetComponent<AudioSource>();
        _turnOn = GetComponent<AudioSource>();
        _off = true;
        _flashLight.SetActive(false);
    }

 
    void Update()
    {
        LightOnOff();
    }
    private void LightOnOff()
    {
        if (_off && Input.GetKeyDown(KeyCode.F))
        {
            _flashLight.SetActive(true);
            _turnOn.Play();
            _off = false;
            _on = true;
        }
        else if (_on && Input.GetKeyDown(KeyCode.F))
        {
            _flashLight.SetActive(false);
            _turnOn.Play();
            _off = true;
            _on = false;
        }
    }
}
