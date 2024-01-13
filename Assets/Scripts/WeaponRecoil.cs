using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera playerCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
  
    private CinemachinePOV _pov;
    
    [SerializeField]private float verticalRecoil;
    public float duration;

    private float _time;
    private int _index; 

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        
    }
    private void LateUpdate()
    {
        _pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        if (_time > 0)
        {
            _pov.m_VerticalAxis.Value -= (verticalRecoil * Time.deltaTime) / duration;
            _time -= Time.deltaTime;
        }
        
    }
    
    public void GenerateRecoil(string weaponName)
    {
        _time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);
    }
}
