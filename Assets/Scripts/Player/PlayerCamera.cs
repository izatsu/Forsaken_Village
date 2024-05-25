using Cinemachine;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerCamera : MonoBehaviour
{
    private PhotonView _view;
    public GameObject newCam;
    
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private CinemachineVirtualCamera _vc;
    [SerializeField] private AudioListener _listener;

    private CinemachineBrain _brain;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            _view.RPC(nameof(CreateCam), RpcTarget.AllBuffered);
            _listener = newCam.GetComponent<AudioListener>();
            _listener.enabled = true;
            _vc.Priority = 1;
        }
        else
        {
            _vc.Priority = 0;
        }
        
    }
    
    private void FixedUpdate()
    {
        // Nếu đối tượng là của mình, gửi thông tin vị trí của newCam đến tất cả người chơi khác
        if (_view.IsMine)
        {
            _view.RPC(nameof(SyncCameraPosition), RpcTarget.OthersBuffered, newCam.transform.position, newCam.transform.rotation);
        }
    }
    
    [PunRPC]
    private void CreateCam()
    {
        newCam = Instantiate(_cameraPrefab);
        _brain = newCam.GetComponent<CinemachineBrain>(); 
    }

    [PunRPC]
    private void SyncCameraPosition(Vector3 position, Quaternion rotation)
    {
        _brain.enabled = false;
        newCam.GetComponent<Camera>().depth = -2;
        // Cập nhật vị trí và quay của newCam trên người chơi khác
        newCam.transform.position = position;
        newCam.transform.rotation = rotation;
        /*if (!_view.IsMine)
        {
            newCam.transform.position = position;
            newCam.transform.rotation = rotation;
        }*/
    }
}
