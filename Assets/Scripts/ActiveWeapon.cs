using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlots
    {
        Primary = 0, 
        Secondary = 1
    }

    CrossHairTarget _crossHairTarget;

    public Transform[] weaponSlots;

    [Header("Shooting Raycast")]
    private RaycastWeapon[] _equipedWeapons = new RaycastWeapon[2];
    int _activeWeaponIndex; 

    [Header("Animation use weapon")] 
    public Animator rigController;

    [Header("Weapon Recoil")]
    public Cinemachine.CinemachineVirtualCamera playerCamera;

    private PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _crossHairTarget = FindObjectOfType<CrossHairTarget>();
        RaycastWeapon existWeapon = GetComponentInChildren<RaycastWeapon>();

        if (existWeapon)
        {
            _activeWeaponIndex = (int)existWeapon.weaponSlot;
            Equip(existWeapon);
        }

    }

    void Update()
    {
        if (_view.IsMine)
        {
            var weapon = Getweapon(_activeWeaponIndex);
            if (weapon) 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    weapon.StartFiring();
                }

                // Nếu vẫn chưa ngừng giữ chuột bắn thì đạn tiếp tục bắn ra
                if (weapon.isFiring)
                {
                    weapon.UpdateFiring(Time.deltaTime);
                }

                // Cập nhật đường đi, điểm chạm của đạn và xóa đạn khi va chạm
                weapon.UpdateBullets(Time.deltaTime);

                // Sau khi ngừng giữ chuột bắn thì sẽ dừng bắn
                if (Input.GetMouseButtonUp(0))
                {
                    weapon.StopFiring();
                }
            }    
        
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetActiveWeapon(WeaponSlots.Primary);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActiveWeapon(WeaponSlots.Secondary);
            }
        }
    }

    public RaycastWeapon GetActiveWeapon()
    {
        return Getweapon(_activeWeaponIndex);
    }

    RaycastWeapon Getweapon(int index)
    {
        if (index < 0 || index >= _equipedWeapons.Length)
        {
            return null;
        }
        return _equipedWeapons[index];
    }
    
    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex =(int)newWeapon.weaponSlot;
        var weapon = Getweapon(weaponSlotIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = _crossHairTarget.gameObject.transform;
        weapon.recoil.playerCamera = playerCamera;
        //weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        _equipedWeapons[weaponSlotIndex] = weapon;
        SetActiveWeapon(newWeapon.weaponSlot);

    }

    void SetActiveWeapon(WeaponSlots weaponSlot)
    {
        int activateIndex = (int)weaponSlot;
        StartCoroutine(SwitchWeapon(activateIndex));
    }

    IEnumerator SwitchWeapon(int activateIndex)
    {
        var weapon = Getweapon(_activeWeaponIndex);

        if (weapon == null) Debug.Log("Loi ne");
        
        if ((_equipedWeapons[activateIndex] != null) &&  !weapon.reloading)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (i != activateIndex)
                {
                    var otherWeapon = Getweapon(i);
                    if (otherWeapon)
                        otherWeapon.gameObject.SetActive(false);
                }
            }

            yield return StartCoroutine(ActivateWeapon(activateIndex));
            _activeWeaponIndex = activateIndex;
        }     
    }

    
    IEnumerator ActivateWeapon(int index)
    {        
        var weapon = Getweapon(index);
        if (weapon)
        {
            weapon.gameObject.SetActive(true);
            rigController.Play($"equip_{weapon.weaponName}");
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
            
        }
    }


    
}
