using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlots
    {
        Primary = 0, 
        Secondary = 1
    }

    CrossHairTarget crossHairTarget;
    //public UnityEngine.Animations.Rigging.Rig HandIK;
    public Transform[] weaponSlots;

    [Header("Shooting Raycast")]
    private RaycastWeapon[] equiped_Weapons = new RaycastWeapon[2];
    int activeWeaponIndex; 

    [Header("Animation use weapon")] 
    public Animator rigController;

    void Start()
    {
        crossHairTarget = FindObjectOfType<CrossHairTarget>();
        RaycastWeapon existWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existWeapon)
        {
            Equip(existWeapon);
        }
        //HandIK.weight = 1.0f;
    }

    void Update()
    {
        var weapon = Getweapon(activeWeaponIndex);
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

    RaycastWeapon Getweapon(int index)
    {
        if (index < 0 || index >= equiped_Weapons.Length)
        {
            return null;
        }
        return equiped_Weapons[index];
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
        weapon.raycastDestination = crossHairTarget.gameObject.transform;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equiped_Weapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);

    }

    void SetActiveWeapon(WeaponSlots weaponSlot)
    {
        int activateIndex = (int)weaponSlot;
        StartCoroutine(SwitchWeapon(activateIndex));
    }

    IEnumerator SwitchWeapon(int activateIndex)
    {
        if (equiped_Weapons[activateIndex] != null)
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
            activeWeaponIndex = activateIndex;
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
