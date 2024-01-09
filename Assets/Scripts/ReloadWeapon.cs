using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    
    private GameObject _magazineHand;
    private void Start()
    {
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    private void Update()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        if (weapon)
        {
            if ((Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <= 0) && weapon.ammoCount != weapon.clipSize)
            {
                rigController.SetTrigger("reload_Weapon");
                
            }
        }
    }

    void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "stop_firing":
                StopFiring();
                break;
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attach_magazine":
                AttachMagazine();
                break;
            case "can_firing":
                CanFiring();
                break;
        }
    }

    private void StopFiring()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();      
        weapon.reloading = true;
    }
    private void DetachMagazine()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        _magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }

    private void DropMagazine()
    {
        GameObject dropMagazine = Instantiate(_magazineHand,_magazineHand.transform.position, _magazineHand.transform.rotation);
        dropMagazine.AddComponent<Rigidbody>();
        //dropMagazine.AddComponent<BoxCollider>();
        _magazineHand.SetActive(false);
    }

    private void RefillMagazine()
    {
        _magazineHand.SetActive(true);
    }

    private void AttachMagazine()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(_magazineHand);
        weapon.ammoCount = weapon.clipSize;
        rigController.ResetTrigger("reload_Weapon");
    }

    private void CanFiring()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();      
        weapon.reloading = false;
    }
}
 