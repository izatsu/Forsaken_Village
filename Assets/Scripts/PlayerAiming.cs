using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    [Header("Animation Rigging Aim")]
    [SerializeField] float aimDuration = 0.3f;
    [SerializeField] Rig aimPlayer;

    

    private void Start()
    {
        
    }

    private void Update()
    {
        /*if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            aimPlayer.weight += Time.deltaTime / aimDuration;           
        }
        else
        {
            aimPlayer.weight -= Time.deltaTime / aimDuration;
        }*/

        aimPlayer.weight = 1;

        // Khi bắt đầu ấn chuột thì đạn sẽ ra
        

    }
}
