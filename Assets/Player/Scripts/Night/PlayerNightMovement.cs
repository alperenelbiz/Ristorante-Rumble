using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNightMovement : NetworkBehaviour
{
    [SyncVar]
    public string team;

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

    [Header("Case Operations")]
    [SyncVar] private int carryingMoney = 0;
    private bool isStealing = false;

    [SerializeField] private float stealInterval = 0.5f;
    [SerializeField] private int stealingAmount = 5;
    public KeyCode stealKey = KeyCode.E;
    public KeyCode putKey = KeyCode.Q;

    private Rigidbody rb;
    private bool isGrounded;
    Vector3 movement;

    [HideInInspector] public bool isSprinting;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Enable the local player's camera
        cam.gameObject.SetActive(true);

        // Optionally, you can lock the cursor here
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer)
        {
            // Disable the camera for non-local players
            cam.gameObject.SetActive(false);
        }

        // Add the player to the team
        if (TeamManager.Instance != null)
        {
            TeamManager.Instance.AddPlayerToTeam(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (isServer && TeamManager.Instance != null)
        {
            TeamManager.Instance.RemovePlayerFromTeam(gameObject);
        }
    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
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

            /*if (Vector3.Distance(transform.position, caseTransform.position) < 2.0f && Input.GetKeyDown(stealKey))
            {
                isStealing = true;
            }
            else if (Input.GetKeyUp(stealKey))
            {
                isStealing = false;
            }*/

            if (isStealing)
            {
                StealAmountOverTime();
            }

            if (Input.GetKey(putKey))
            {
                PutMoney();
            }
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


    [Command]
    private void StealAmountOverTime()
    {
        if (Time.time % stealInterval < Time.deltaTime)
        {
           
        }
    }

    [Command]
    private void PutMoney()
    {
        
    }
}
