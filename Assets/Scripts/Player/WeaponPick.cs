using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;

public class WeaponPick : MonoBehaviour
{
    private ActiveWeapon _activeWeapon;
    [SerializeField] private float distance = 10f;

    private PhotonView _view;
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _view.IsMine)
        {
            PickUp();
        }
    }

    
    private void PickUp()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, distance))
        {
            if (hitInfo.transform.tag == "canGrab" && _view.IsMine)
            {
                /*RaycastWeapon newWeaPon = 
                    PhotonNetwork.Instantiate(hitInfo.transform.GetComponent<WeaponPickup>().weaponFab.gameObject.name, 
                        Vector3.zero, quaternion.identity).GetComponent<RaycastWeapon>();
                        */

                RaycastWeapon newWeaPon = Instantiate(hitInfo.transform.GetComponent<WeaponPickup>().weaponFab);
                _activeWeapon.Equip(newWeaPon);
            }
        }
    }
    
}
