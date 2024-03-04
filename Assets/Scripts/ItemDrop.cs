using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject objectDrop;
    void Start()
    {
        rb = objectDrop.GetComponent<Rigidbody>();  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            rb.useGravity = true;
    }

}
