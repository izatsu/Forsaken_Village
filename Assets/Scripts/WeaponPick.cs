using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPick : MonoBehaviour
{
    private ActiveWeapon _activeWeapon;
    [SerializeField] private float distance = 10f;
    private bool _canGrab;
    private void Start()
    {
        _activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
            Debug.Log("da nhannn");
        }
    }

    private void PickUp()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance))
        {
            if (hitInfo.transform.tag == "canGrab")
            {
                _canGrab = true;
                Debug.Log("da nhat");
                RaycastWeapon newWeaPon = Instantiate(hitInfo.transform.GetComponent<WeaponPickup>().weaponFab);
                _activeWeapon.Equip(newWeaPon);
            }
            else
            {
                _canGrab = false;
            }
        }
    }
}
