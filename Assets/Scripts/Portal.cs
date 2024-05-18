using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static Portal  instance{ get; private set;}
    
    public GameObject uiWinGame ;
    public bool winGame = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winGame = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            uiWinGame.SetActive(true);
        }
    }
}
