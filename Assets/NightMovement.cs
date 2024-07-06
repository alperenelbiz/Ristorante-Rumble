using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NightMovement : NetworkBehaviour
{
    [Header("Character Movement")]
    [SerializeField] private CharacterController characterController = null;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    float gravity_ = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private GameObject feet = null;
    [SerializeField] private LayerMask groundMask = new LayerMask();
    float checkRadius = 0.4f;

    [Header("Animatoions")]
    [SerializeField] private Animator animator;

    Vector3 velocity;

    private void Update()
    {
        if (!isLocalPlayer) return;

        PlayerInput();
    }

    private void PlayerInput()
    {
        bool isGrounded = Physics.CheckSphere(feet.transform.position, checkRadius, groundMask);

        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = -2f;
        }

        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        if (X != 0 || Z != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        Vector3 move_ = transform.right * X + transform.forward * Z;

        characterController.Move(move_ * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded) return;

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity_);
        }

        velocity.y += gravity_ * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
