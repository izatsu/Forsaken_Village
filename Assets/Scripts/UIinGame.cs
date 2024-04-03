using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UIinGame : MonoBehaviourPunCallbacks
{
    private PhotonView _view;


    public void ButtonLeave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MenuGame");
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
