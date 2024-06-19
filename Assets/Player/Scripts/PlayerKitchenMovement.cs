using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerKitchenMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed;
    private Rigidbody rb;
    private Vector3 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!isLocalPlayer)
            this.enabled = false;
    }

    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    { 
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        input = new Vector3(moveX, 0f, moveZ).normalized;
    }
    private void MovePlayer()
    {
        rb.MovePosition(rb.position + input * movementSpeed * Time.fixedDeltaTime);

        if (input != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }
}
