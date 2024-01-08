using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    [Header("Move & Animation Move")]
    float horizontal;
    float vertical;
    Animator anim;


    [Header("Camera")]
    private Vector3 startingRotation;
    private new Camera camPlayer;
    [SerializeField] Transform viewPoint;
    Vector2 MouseInput;
    float verticalRotStore = 0;
    [SerializeField] private float moveSensitivity = 5f;

    [Header("Check Ground")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform GoundCheckPoint;
    public bool isGrounded;

    [Header("Jump")]
    Rigidbody rb;
    [SerializeField] float jumpForce = 5f;

    [Header("Crouch")]
    CapsuleCollider playerCollider;
    float colliderHeight;
    float colliderCenterY;
    float crouchHeight = 1.2f;
    float crouchCenterY = 0.6f;

    [Header("Animation Rigging")]
    [SerializeField] MultiAimConstraint[] body_multiAimConstraint;
    [SerializeField]Transform lookAt;
    [SerializeField] RigBuilder rig;


    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        lookAt = FindObjectOfType<LookAt>().transform;
        Cursor.lockState = CursorLockMode.Locked;
        camPlayer = Camera.main;

        colliderHeight = playerCollider.height;
        colliderCenterY = playerCollider.center.y;

        SetAimTarget(lookAt);
    }

    private void FixedUpdate()
    {
        PlayerMove();
        Jump();
    }


    private void LateUpdate()
    {
        CameraRotation();
    }

    private void PlayerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isCrouch", false);
            anim.SetBool("isRun", true);
            playerCollider.height = colliderHeight;
            playerCollider.center = new Vector3(playerCollider.center.x, colliderCenterY, playerCollider.center.z);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouch", true);
            playerCollider.height = crouchHeight;
            playerCollider.center = new Vector3(playerCollider.center.x, crouchCenterY, playerCollider.center.z);
            anim.SetBool("isRun", false);
        }

        else
        {
            anim.SetBool("isCrouch", false);
            anim.SetBool("isRun", false);
            playerCollider.height = colliderHeight;
            playerCollider.center = new Vector3(playerCollider.center.x, colliderCenterY, playerCollider.center.z);
        }
    }

    private void CameraRotation()
    {
        if (startingRotation == null)
        {
            startingRotation = transform.localRotation.eulerAngles;
        }

        Vector2 deltaInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        startingRotation.x += deltaInput.x * moveSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            startingRotation.x,
            transform.rotation.eulerAngles.z);
    }

    private bool CheckGround()
    {
        return isGrounded = Physics.Raycast(GoundCheckPoint.position, Vector3.down, 0.25f, groundLayer);
    }

    private void Jump()
    {
        //if (CheckGround()) anim.SetBool("isJump", false);
        if (Input.GetButton("Jump") && CheckGround())
        {
            //anim.SetBool("isJump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void SetAimTarget(Transform lookAt)
    {
        for(int i = 0;i < body_multiAimConstraint.Length;i++)
        {
            var data = body_multiAimConstraint[i].data.sourceObjects;
            data.Clear();
            data.Add(new WeightedTransform(lookAt, 1));
            body_multiAimConstraint[i].data.sourceObjects = data;
            
        }
        rig.Build();
    }
}
