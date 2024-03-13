using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Pray : MonoBehaviour
{
    public PickUpItemID[] books;
    public int countAcitveBook = 0;

    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        CheckBookActive();
    }

    private void CheckBookActive()
    {
        if (countAcitveBook == books.Length)
        {
            //Debug.Log("Da full");
        }
    }

    public void SetActiveBook(int id)
    {
        _view.RPC(nameof(ActiveBook), RpcTarget.AllBuffered, id);
    }

    [PunRPC]
    public void ActiveBook(int id)
    {
        foreach (var book in books)
        {
            if(book.id == id) 
                book.gameObject.SetActive(true);
        }
    }  
}
