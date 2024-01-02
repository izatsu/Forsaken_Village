using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move & Animation Move")]
    [SerializeField] float moveSpeed;
    float horizontal;
    float vertical;
    Animator anim;

    [Header("Camera")]
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

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        camPlayer = Camera.main;
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
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouch", true);
            anim.SetBool("isRun", false);
        }

        else
        {
            anim.SetBool("isCrouch", false);
            anim.SetBool("isRun", false);
        }
    }

    private void CameraRotation()
    {
        // Lấy giá trị của mouse
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * moveSensitivity;

        //Set player xoay theo theo trục X của mouse
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + MouseInput.x,
            transform.rotation.eulerAngles.z);


        verticalRotStore += MouseInput.y;

        //Giới hạn góc nhìn mouse Y
        verticalRotStore = Mathf.Clamp(verticalRotStore, -60f, 40f);

        // Set góc nhìn trục Y 
        viewPoint.rotation = Quaternion.Euler(
        -verticalRotStore,
        viewPoint.rotation.eulerAngles.y,
        viewPoint.rotation.eulerAngles.z);

        camPlayer.transform.SetLocalPositionAndRotation(viewPoint.position, viewPoint.rotation);

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
}
