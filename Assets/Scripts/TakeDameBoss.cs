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
            PhotonView pv = other.GetComponent<PhotonView>();
            if(pv.IsMine)
                EnemyState.instance.TakeDamage(1);
        }
    }
}
