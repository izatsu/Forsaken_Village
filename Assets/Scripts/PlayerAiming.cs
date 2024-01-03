using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] float aimDuration = 0.3f;
    [SerializeField] Rig aimPlayer;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimPlayer.weight += Time.deltaTime / aimDuration;
        }
        else
        {
            aimPlayer.weight -= Time.deltaTime / aimDuration;
        }
    }
}
