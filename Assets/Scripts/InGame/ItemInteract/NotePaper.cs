using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePaper : MonoBehaviour
{
    public GameObject uiNote;

    public bool isOn = false;

    public void SetUIOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        uiNote.SetActive(true);
    }

    public void SetUIOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uiNote.SetActive(false);
    }
}
