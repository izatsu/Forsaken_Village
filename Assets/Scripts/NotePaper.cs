using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePaper : MonoBehaviour
{
    public GameObject uiNote;

    public void SetUIOn()
    {
        uiNote.SetActive(true);
    }

    public void SetUIOff()
    {
        Debug.Log("Da nhan");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uiNote.SetActive(false);
    }
}
