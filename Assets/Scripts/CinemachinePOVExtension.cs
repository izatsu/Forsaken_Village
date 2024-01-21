using UnityEngine;
using Cinemachine;
using Photon.Pun;
public class CinemachinePovExtension : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 180f;
    [SerializeField]
    private float verticalSpeed = 60f;
    [SerializeField] 
    private float clampAngle = 80f;

    private Vector3 _startingRotation;
    
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (_startingRotation == null)
                {
                    _startingRotation = transform.localRotation.eulerAngles;
                }

                Vector2 deltaInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

                _startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                _startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                _startingRotation.y  = Mathf.Clamp(_startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
                
            }
        }
    }
}