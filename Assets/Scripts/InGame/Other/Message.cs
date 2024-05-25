using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    public TextMeshProUGUI myMessage;

    private void Start()
    {
        GetComponent<RectTransform>().SetAsFirstSibling();
    }
}
