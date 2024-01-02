using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speedMove = 5f;
    Vector3 movement, moveDirection; 
    CharacterController characterController;
    Camera camera;

    [SerializeField] GameObject attackFX;
     

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        ChangeRotation();
        Attack();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0, vertical);
        movement = (transform.forward * moveDirection.z) + (transform.right * moveDirection.x).normalized; 
        characterController.Move(movement * speedMove * Time.deltaTime);

        
    }

    void ChangeRotation()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        Vector3 rot_ = new Vector3(horizontal, 0, vertical);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rot_.x,
            transform.rotation.eulerAngles.z);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackFX.SetActive(true);
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin = camera.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            StartCoroutine(Close());
        }
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(0.2f);
        attackFX.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Win"))
        {
            Debug.Log("Win Game");
        }
    }
}
