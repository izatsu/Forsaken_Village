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
    Animator anim;
    AnimatorOverrideController overrides;
    
    void Start()
    {
        anim = GetComponent<Animator> ();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;
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
        else
        {
            HandIK.weight = 0;
            anim.SetLayerWeight(1, 0f);
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

        HandIK.weight = 1f;
        anim.SetLayerWeight(1, 1f);
        Invoke(nameof(SetAnimationDelayed), 0.001f);
        
    }

    void SetAnimationDelayed()
    {
        overrides["Weapon_anim_empty"] = weapon.weaponAnimation;
    }

    [ContextMenu("Save weapon pose")]
    // Lưu vị trí weapon và vị trí tay cầm weapon vào animation
    private void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0f);
        recorder.SaveToClip(weapon.weaponAnimation);
    }
}
