using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public bool isOnUIWinGame = false;
    public GameObject uiWinGame;
    
    private void Update()
    {
        if (EnemyState.instance.isDie && !isOnUIWinGame)
        {
            isOnUIWinGame = true;
            Invoke(nameof(SetActiveUI), 7f);
        } 
    }

    private void SetActiveUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        uiWinGame.SetActive(true);
    }
}
