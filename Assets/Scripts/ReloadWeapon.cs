using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    
    private GameObject _magazineHand;
    
    [SerializeField] private AudioClip soundReload;

    private PhotonView _view;

    private PlayerState _playerState; 
    
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
            if (weapon)
            {
                if ((Input.GetKeyDown(KeyCode.R) || weapon.ammoCount <= 0) && weapon.ammoCount != weapon.clipSize && !_playerState.isDie)
                {
                    weapon.PlaySound(soundReload);
                    //rigController.SetTrigger("reload_Weapon");
                    PlayReload();
                    Debug.Log("Nap dan");
                
                }
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

    [PunRPC]
    private void PlayReload()
    {   
        if(_playerState.isDie) return;
        rigController.SetTrigger("reload_Weapon");
        Debug.Log("Da chay anim nap");
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
        Destroy(dropMagazine, 2f);
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
        weapon.textAmmo.text = $"{weapon.clipSize}/\u221e";
        rigController.ResetTrigger("reload_Weapon");
    }

    private void CanFiring()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();      
        weapon.reloading = false;
    }
}
 