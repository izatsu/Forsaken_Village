using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig HandIK;
    public Transform weaponParent;

    public Transform weaponRightGrip;
    public Transform weaponLeftGrip;

    [Header("Shooting Raycast")]
    private RaycastWeapon weapon;

    [Header("Animation use weapon")] 
    public Animator rigController;

    bool isHolstered = true;

    void Start()
    {
        RaycastWeapon existWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existWeapon)
        {
            Equip(existWeapon);
        }
    }

    void Update()
    {
        if (weapon != null) 
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

        if(Input.GetKeyDown(KeyCode.X))
        {
            isHolstered = rigController.GetBool("holster_weapon");
            rigController.SetBool("holster_weapon", !isHolstered);
        }        
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Debug.Log("" + weapon.weaponName);
        HandIK.weight = 1.0f;
        rigController.Play($"equip_{weapon.weaponName}");

    }

}
