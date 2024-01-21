using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private Light lightFlicker;

    [SerializeField]
    private AudioSource lightSound;

    public float _minTime;
    public float _maxTime;
    public float _time;

    void Start()
    {
        _time = Random.Range(_minTime, _maxTime);   
    }

    void Update()
    {
        LightFlickering();
    }
    private void LightFlickering()
    {
        if(_time >0)
        {
            _time -= Time.deltaTime;
        }
        if( _time <= 0)
        {
            lightFlicker.enabled = !lightFlicker.enabled;
            _time = Random.Range(_minTime, _maxTime);
            lightSound.Play();  
        }
    }
}
