using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PickUpItemID : MonoBehaviour
{
    public int id;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    public void DestroyObj()
    {
        _view.RPC(nameof(DestroyOb), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void DestroyOb()
    {
        Destroy(this.gameObject);
    }
    
}
