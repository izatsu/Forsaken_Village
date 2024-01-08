using Cinemachine;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera playerCamera;
    private CinemachinePOV _pov;
    public float verticalRecoil; 
    private void Start()
    {
        _pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    
    public void GenerateRecoil()
    {
        _pov.m_VerticalAxis.Value -= verticalRecoil;
        
    }
}
