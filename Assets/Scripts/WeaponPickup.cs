using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponFab;
    
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActiveWeapon activeWeapon =other.gameObject.GetComponent<ActiveWeapon>();
                if (activeWeapon != null)
                {
                    RaycastWeapon newWeapon = Instantiate(weaponFab);
                    activeWeapon.Equip(newWeapon);
                }
            }
        }
    }*/
}
