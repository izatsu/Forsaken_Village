using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    [Header("Animation Rigging Aim")]
    [SerializeField] float aimDuration = 0.3f;
    [SerializeField] Rig aimPlayer;

    [Header("Shooting Raycast")]
    private RaycastWeapon weapon;

    private void Start()
    {
        weapon = GetComponentInChildren<RaycastWeapon>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            aimPlayer.weight += Time.deltaTime / aimDuration;           
        }
        else
        {
            aimPlayer.weight -= Time.deltaTime / aimDuration;
        }

        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartFiring();
        }

        if (weapon.isFiring)
        {
            weapon.UpdateFiring(Time.deltaTime);
        }

        weapon.UpdateBullets(Time.deltaTime);

        if (Input.GetMouseButtonUp(0))
        {
            weapon.StopFiring();
        }
        

    }
}
