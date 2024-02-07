using Photon.Pun;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _anim;
    private AudioSource _sound;

    public bool inReach;

    private bool _isOpen;

    public int idChest = 0;
    public bool isLock = true;

    private PhotonView _view;
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _isOpen = false;
        inReach = false;
        _anim = GetComponent<Animator>();
        _sound = GetComponent<AudioSource>();
    }


    private void LateUpdate()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (!isLock)
            {
                _view.RPC(nameof(OpenChest), RpcTarget.AllBuffered);
            }
            else
            {
                _view.RPC(nameof(LockChest), RpcTarget.AllBuffered);
            }
        }
    }


    [PunRPC]
    private void OpenChest()
    {
        _isOpen = !_isOpen;
        inReach = false;
        _anim.SetBool("isOpen", _isOpen);
        _sound.Play();
    }

    [PunRPC]
    private void LockChest()
    {
        Debug.Log("Ruong bi khoa");
        inReach = false;
    }
}
