using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UIinGame : MonoBehaviourPunCallbacks
{
    private PhotonView _view;

    [Header("Setting")]
    public GameObject settingPanel;

    [Header("PanelAUS")]
    public GameObject AUSPanel;

    public GameObject cutsceneIntro;

    public bool setCutsceneIntro = true;
    public bool checkCutscene = false;
    public bool checkActive = false;

    private void Start()
    {
        ClosePanelSetting();
        ClosePanelAUS();
        cutsceneIntro.SetActive(true);
        Invoke(nameof(SkipCutsceneIntro), 64f);
    }
    private void Update()
    {
        if(setCutsceneIntro)
        {
            checkActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        if(!setCutsceneIntro && !checkCutscene)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            checkCutscene = true;
        }
        PanelSetting();
    }
    public void ButtonLeave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MenuGame");
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void PanelSetting()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            checkActive = !checkActive;

            if (checkActive)
            {
                OpenPanelSetting();
            }
            else
            {
                ClosePanelSetting();
                ClosePanelAUS();
            }
        }
      
            
    }
    public void OpenPanelSetting()
    {
        settingPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ClosePanelSetting()
    {
        checkActive=false;
        settingPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void PanelAUS()
    {
        AUSPanel.SetActive(true);
    }
    public void ClosePanelAUS()
    {
        AUSPanel.SetActive(false);
    }
    public void SkipCutsceneIntro()
    {
        checkActive = false;
        cutsceneIntro.SetActive(false);
        setCutsceneIntro = false;
    }

}
