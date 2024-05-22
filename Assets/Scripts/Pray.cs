using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Pray : MonoBehaviour
{
    //public static Pray instance; 
    public PickUpItemID[] books;
    public int countAcitveBook = 0;

    private PhotonView _view;

    public bool checkBookFull = false;
    private void Start()
    {
        /*if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }*/
        _view = GetComponent<PhotonView>();
        
    }

    private void Update()
    {
        CheckBookActive();
    }

    private void CheckBookActive()
    {
        foreach (var book in books)
        {
            if(countAcitveBook == books.Length) 
                break;
            if (book.gameObject.activeSelf)
                countAcitveBook++;
            else
            {
                countAcitveBook = 0;
            }
        }
        if (countAcitveBook == books.Length)
        {
            //Debug.Log("Da full");
            //checkBookFull = true;
            _view.RPC(nameof(FullBook), RpcTarget.AllBuffered);
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

    [PunRPC]
    public void AddBook()
    {
        countAcitveBook++;
        Debug.Log("Dat sach");
    }
    
    [PunRPC]
    public void FullBook()
    {
        checkBookFull = true;
    }
}
