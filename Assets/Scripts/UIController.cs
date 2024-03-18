using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class UIController : MonoBehaviourPunCallbacks
{
    public static UIController instance { get; private set; }

    [Header("Màn hình loading")] 
    public GameObject loadingScreen;

    [Header("Màn hình chính")] 
    public GameObject mainScreen;

    [Header("Màn hình tạo phòng")] 
    public GameObject createRoomScreen;
    public TMP_InputField inputRoomName;
    
    [Header("Màn hình tạo Nickname")] 
    public GameObject createNicknameScreen;
    public GameObject errorScreen;
    public TMP_InputField inputNickname; 

    [Header("Màn hình trong Phòng")] 
    public GameObject roomDetailScreen; 
    public TextMeshProUGUI textOneNickname;
    public List<TextMeshProUGUI> nicknames = new();
    public TextMeshProUGUI textRoomName;
    public GameObject buttonStartGame;
    
    [Header("Màn hình danh sách phòng")]
    public GameObject roomListScreen;
    public RoomButton oneRoomButton;
    public List<RoomButton> roomButtons = new();

    [Header("Màn hình Help & Guild")]
    public GameObject helpScreen;


    [Header("Màn hình setting")]
    public GameObject settingScreen;
    
    private PhotonView _view;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        CloseAllScreen();
        StartCoroutine(TimedDelayLoadScreen());
        //PhotonNetwork.ConnectUsingSettings();
    }
    
    private void OpenScreen(GameObject nameScreen)
    {
        nameScreen.SetActive(true);
    }
    
    private void CloseAllScreen()
    {
        loadingScreen.SetActive(false);
        mainScreen.SetActive(false); 
        createRoomScreen.SetActive(false);
        createNicknameScreen.SetActive(false);
        roomDetailScreen.SetActive(false);
        roomListScreen.SetActive(false);
        settingScreen.SetActive(false);
        errorScreen.SetActive(false);
        helpScreen.SetActive(false);
    }

    private void ShowAllPLayer()
    {
        foreach (TextMeshProUGUI player in nicknames)
        {
            Destroy(player.gameObject);
            
        }
        nicknames.Clear();
        
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            TextMeshProUGUI namePlayer = Instantiate(textOneNickname, textOneNickname.transform.parent);
            namePlayer.text = players[i].NickName;
            namePlayer.gameObject.SetActive(true);

            nicknames.Add(namePlayer);
        }
        Debug.Log("Cập nhật lại danh sách player trong phòng");
        
    }

    IEnumerator TimedDelayLoadScreen()
    {
        OpenScreen(loadingScreen);
        yield return new WaitForSeconds(7f);
        PhotonNetwork.ConnectUsingSettings();
    }

    #region PhotonPunFuction

    // Hàm sẽ chạy sau khi kết nối thành công máy chủ
    public override void OnConnectedToMaster()
    {
        // tham gia sảnh có thể tìm phòng or tạo phòng chơi
        PhotonNetwork.JoinLobby();
        // tự động đồng bộ hóa scene
        PhotonNetwork.AutomaticallySyncScene = true;
        
        Debug.Log("Đã kết nối máy chủ");
    }

    public override void OnJoinedLobby()
    {
        CloseAllScreen();
        OpenScreen(createNicknameScreen);
    }

    // Chạy khi tạo phòng thất bại
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Phòng này đã được tạo");
    }
    
    // Khi vào được phòng 
    public override void OnJoinedRoom()
    {
        CloseAllScreen();
        ShowAllPLayer();
        OpenScreen(roomDetailScreen);
        Debug.Log("Đã vào phòng");
    }

    // Hàm này chạy khi có player mới vào phòng ngoài chủ phòng
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player mới vào phòng");
        CloseAllScreen();
        ShowAllPLayer();
        OpenScreen(roomDetailScreen);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Player roi phong");
        foreach (var name in nicknames)
        {
            if (name.text == otherPlayer.NickName)
            {
                nicknames.Remove(name);
               Destroy(name.gameObject);
            } 
        }
        ShowAllPLayer();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomButtons)
        {
            Destroy(room.gameObject);
        }

        roomButtons.Clear();
        
        oneRoomButton.gameObject.SetActive(false);

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(oneRoomButton, oneRoomButton.transform.parent); 
                newButton.SetRoomButtonDetail(roomList[i]);
                newButton.gameObject.SetActive(true);
                
                roomButtons.Add(newButton);
            }
        }

    }

    #endregion

    #region ButtonFunction

    public void ButtonCreateRoom()
    {
        CloseAllScreen();
        OpenScreen(createRoomScreen);
    }

    public void ButtonCreatePunRoom()
    {
        var roomName = inputRoomName.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Tên phòng không hợp lệ");
            OpenScreen(errorScreen);
            return;
        }

        // Tạo phòng trên Pun và đặt giới hạn sl người chơi trong phòng
        RoomOptions roomOptions = new()
        {
            MaxPlayers = 4
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        
        textRoomName.text = roomName; 
        
        
        
        OpenScreen(roomDetailScreen);
    }

    public void ButtonCreateNickname()
    {
        var nickname = inputNickname.text;
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.Log("Tên phòng không hợp lệ");
            OpenScreen(errorScreen);
            return;
        }

        PhotonNetwork.NickName = nickname;
        
        CloseAllScreen();
        OpenScreen(mainScreen);
        
        Debug.Log("PLayer mới vào đã đặt tên");
        Debug.Log($"name newPlayer: {PhotonNetwork.NickName}");
    }

    public void ButtonLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseAllScreen();
        OpenScreen(mainScreen);
    }

    public void ButtonStartGame()
    {
        _view.RPC(nameof(CloseAllSceenMutil), RpcTarget.AllBuffered);
        PhotonNetwork.LoadLevel("VFX");
    }

    [PunRPC]
    private void CloseAllSceenMutil()
    {
        CloseAllScreen();
    }

    public void ButtonJoinRoom()
    {
        CloseAllScreen();
        OpenScreen(roomListScreen);
    }

    public void ButtonLeaveRoomList()
    {
        CloseAllScreen();
        OpenScreen(mainScreen);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        textRoomName.text = roomInfo.Name;
        // Chủ phòng thì hiện nút start
        if (PhotonNetwork.IsMasterClient)
        {
            buttonStartGame.SetActive(true);
        }
        else buttonStartGame.SetActive(false);
        CloseAllScreen();
    }

    public void ButtonSetting()
    {
        CloseAllScreen();
        OpenScreen(settingScreen);
    }
    public void ButtonBackSetting()
    {
        CloseAllScreen();
        OpenScreen(mainScreen);
    }
    public void ButtonExitErrorScreen()
    {
        errorScreen.SetActive(false);
    }
    public void ButtonHelpGuild()
    {
        CloseAllScreen();
        helpScreen.SetActive(true);
    }
    public void ButtonBackHelpGuild()
    {
        helpScreen.SetActive(false);
        OpenScreen(mainScreen);
    }
    #endregion
}
