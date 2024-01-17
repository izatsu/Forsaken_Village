using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public TextMeshProUGUI buttonLabel;
    public RoomInfo roomInfo;

    public void SetRoomButtonDetail(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        buttonLabel.text = roomInfo.Name;
    }

    public void JoinRoom()
    {
        UIController.instance.JoinRoom(this.roomInfo);
    }
    
}
