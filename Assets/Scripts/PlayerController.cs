using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [Header("Move & Animation Move")]
    [SerializeField] private float groundSpeed; 
    private CharacterController _characterController;
    private Vector2 _input;
    private float _horizontal;
    private float _vertical;
    private Animator _anim;
    private Vector3 _rootMotion;


    [Header("Camera")] 
    private Vector3 _startingRotation;
    [SerializeField] private Transform viewPoint;
    private Vector2 _mouseInput;
    [SerializeField] private float moveSensitivity = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity;
    [SerializeField] private float stepDowm;
    [SerializeField] private float airControl;
    [SerializeField] private float jumpDamp;
    private bool _isJumping;
    private Vector3 _velocity;

    [Header("Crouch")] 
    private bool _isCrouching = false;
    private float _colliderHeight;
    private float _colliderCenterY;
    private float _crouchHeight = 1.2f;
    private float _crouchCenterY = 0.6f;
    
    [Header("Animation Rigging")]
    [SerializeField] private MultiAimConstraint[] bodyMultiAimConstraint;
    [SerializeField] private Transform lookAt;
    [SerializeField] private RigBuilder rig;

    [Header("Photon")] 
    private PhotonView _view;


    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        _colliderHeight = _characterController.height;
        _colliderCenterY = _characterController.center.y;
        
        if (_view.IsMine)
        {
            lookAt = FindObjectOfType<LookAt>().transform;
        }
        _view.RPC(nameof(SetAim), RpcTarget.All);
    }
    
    [PunRPC]
    private void SetAim()
    {
        //lookAt = FindObjectOfType<LookAt>().transform;
        SetAimTarget(lookAt);
    }
  

    private void Update()
    {
        if (_view.IsMine)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _input = new Vector3(_horizontal, _vertical);

            _anim.SetFloat("Horizontal", _horizontal);
            _anim.SetFloat("Vertical", _vertical);
        
            UpdateRun();
            UpdateCrouch();
        
            if (Input.GetKeyDown(KeyCode.Space) && !_isCrouching)
            {
                Jump();
            }

            if (!_isCrouching)
            {
                _characterController.height = _colliderHeight;
                _characterController.center = new Vector3(_characterController.center.x, _colliderCenterY, _characterController.center.z);
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (_view.IsMine)
        {
            if (_isJumping)
            {
                UpdateInAir();
            }
            else
            {
                UpdateOnGround();
            }
        }
    }
    
    private void LateUpdate()
    {
        if(_view.IsMine)
            CameraRotation();
    }

    private void UpdateOnGround()
    {
        Vector3 stepForWardAmount = _rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDowm;
        _characterController.Move(stepForWardAmount + stepDownAmount);
        _rootMotion = Vector3.zero;
        if (!_characterController.isGrounded) 
            SetInAir(0);
    }

    private void UpdateRun()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        _anim.SetBool("isRun", isRunning);
    }

    private void UpdateCrouch()
    {
         _isCrouching = Input.GetKey(KeyCode.LeftControl);
        _anim.SetBool("isCrouch", _isCrouching);
        _characterController.height = _crouchHeight;
        _characterController.center = new Vector3(_characterController.center.x, _crouchCenterY, _characterController.center.z);
    }

    private void UpdateInAir()
    {
        _velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = _velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        _characterController.Move(displacement);
        _isJumping = !_characterController.isGrounded;
        _rootMotion = Vector3.zero;
        _anim.SetBool("isJumping", _isJumping);
    }
    
    
    private void OnAnimatorMove()
    {
        _rootMotion += _anim.deltaPosition;
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * _input.y) + (transform.right * _input.x)) * (airControl/100); 
    }

    private void CameraRotation()
    {
        if (_startingRotation == null)
        {
            _startingRotation = transform.localRotation.eulerAngles;
        }

        Vector2 deltaInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _startingRotation.x += deltaInput.x * moveSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            _startingRotation.x,
            transform.rotation.eulerAngles.z);
    }
    
    private void Jump()
    {
        if (!_isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        _isJumping = true;
        _velocity = _anim.velocity * (jumpDamp * groundSpeed);
        _velocity.y = jumpVelocity;
        _anim.SetBool("isJumping", true);
    }

    void SetAimTarget(Transform lookAt)
    {
        for(int i = 0;i < bodyMultiAimConstraint.Length;i++)
        {
            var data = bodyMultiAimConstraint[i].data.sourceObjects;
            data.Clear();
            data.Add(new WeightedTransform(lookAt, 1));
            bodyMultiAimConstraint[i].data.sourceObjects = data;
            
        }
        rig.Build();
    }
}
