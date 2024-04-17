using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject message;
    public GameObject content;
    public GameObject scrollView;

    private PhotonView _view;

    [FormerlySerializedAs("_chating")] public bool chating = false;
    
    private bool _hasMessage = false;
    private float _timeShowMessage = 5f; 

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !chating)
        {
            OpenChat();
            chating = true;
            _hasMessage = false;
        }
        else if(Input.GetKeyDown(KeyCode.Return) && chating)
        {
            SendMessage();
            chating = false;
            //HaveMessage();
            _view.RPC(nameof(HaveMessage), RpcTarget.AllBuffered);
        }

        if (_hasMessage)
        {
            _timeShowMessage -= Time.deltaTime;
            if (_timeShowMessage <= 0)
            {
                _timeShowMessage = 5f;
                _hasMessage = false;
                scrollView.SetActive(false);
            }
        }
        
    }

    public void SendMessage()
    {
        _view.RPC(nameof(GetMessage), RpcTarget.AllBuffered, (PhotonNetwork.NickName + ": " + inputField.text));
        inputField.text = "";
        
        //_view.RPC(nameof(HaveMessage), RpcTarget.OthersBuffered);
    }

    [PunRPC]
    private void GetMessage(string receiveMessage)
    {
        GameObject mess = Instantiate(message, content.transform);
        mess.GetComponent<Message>().myMessage.text = receiveMessage;
        //scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;;
        StartCoroutine(ScrollToBottom());
    }

    [PunRPC]
    private void HaveMessage()
    {
        _hasMessage = true;
        _timeShowMessage = 5f;
        scrollView.SetActive(true);
        inputField.gameObject.SetActive(false);
    }

    private void OpenChat()
    {
        scrollView.SetActive(true);
        inputField.gameObject.SetActive(true);
        
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void CloseChat()
    {
        scrollView.SetActive(false);
        inputField.gameObject.SetActive(false);
    }
    
    private IEnumerator ScrollToBottom()
    {
        yield return null;
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        yield return new WaitForSeconds(0.1f);
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }
}
