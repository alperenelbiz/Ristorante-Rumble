using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNightMovement : NetworkBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Sprinting Effects")]
    [SerializeField] Camera cam;
    [SerializeField] float sprintFOV = 100f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftControl;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;
    Vector3 movement;

    [HideInInspector] public bool isSprinting;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //Camera.main.SetActiv
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            cam.gameObject.SetActive(false);
            this.enabled = false;
            return;
        }

    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        if (isSprinting)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFOV, 8f * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90f, 8f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        MovePlayer();
    }

    void MyInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = orientation.forward * moveVertical + orientation.right * moveHorizontal;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayer()
    {
        Vector3 moveVelocity = movement * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            isSprinting = true;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            isSprinting = false;
        }
    }
}
