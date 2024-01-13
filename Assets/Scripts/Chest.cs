using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _anim;
    private AudioSource _sound;

    public bool inReach;

    private bool _isOpen;

    public int idChest = 0;
    public bool isLock = true;
    private void Start()
    {
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
                _isOpen = !_isOpen;
                inReach = false;
                _anim.SetBool("isOpen", _isOpen);
                _sound.Play();
                
            }
            else
            {
                Debug.Log("Ruong bi khoa");
                inReach = false;
            }
        }
    }
}
