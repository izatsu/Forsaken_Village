using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject message;
    public GameObject content;

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SendMessage()
    {
        GetComponent<PhotonView>().RPC(nameof(GetMessage), RpcTarget.AllBuffered, (PhotonNetwork.NickName + ": " + inputField.text));
        inputField.text = "";
    }

    [PunRPC]
    public void GetMessage(string receiveMessage)
    {
        GameObject mess =  Instantiate(message, Vector3.zero, quaternion.identity, content.transform);
        mess.GetComponent<Message>().myMessage.text = receiveMessage; 
    }
}
