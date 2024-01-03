using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponFab;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon =other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon != null)
        {
            RaycastWeapon newWeapon = Instantiate(weaponFab);
            activeWeapon.Equip(newWeapon);
        }
    }
}
