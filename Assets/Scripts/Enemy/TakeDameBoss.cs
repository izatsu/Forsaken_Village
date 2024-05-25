using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TakeDameBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        { 
            EnemyState.instance.TakeDamage(1);
        }
    }
}
