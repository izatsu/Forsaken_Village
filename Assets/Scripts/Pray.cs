using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pray : MonoBehaviour
{
    public PickUpItemID[] books;
    public int countAcitveBook = 0;

    private void Update()
    {
        CheckBookActive();
    }

    private void CheckBookActive()
    {
        if (countAcitveBook == books.Length)
        {
            Debug.Log("Da full");
        }
    }
}
