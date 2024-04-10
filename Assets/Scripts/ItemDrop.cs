using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject objectDrop;

    public AudioSource audio;
    public AudioClip soundDrop;

    private bool _isWork = false; 
    
    void Start()
    {
        rb = objectDrop.GetComponent<Rigidbody>();  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_isWork)
        {
            _isWork = true;
            rb.useGravity = true;
            audio.clip = soundDrop;
            audio.Play();
        }
           
    }

}
