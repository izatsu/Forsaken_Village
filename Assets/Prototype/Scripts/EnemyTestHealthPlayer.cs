using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestHealthPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState.instance.TakeDamage(1);
        }
    }
}
