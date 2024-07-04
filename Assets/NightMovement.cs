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
    float gravity_ = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private GameObject feet = null;
    [SerializeField] private LayerMask groundMask = new LayerMask();
    float checkRadius = 0.4f;

    Vector3 velocity;

    private void Update()
    {
        if(!isLocalPlayer) return;

        bool isGrounded = check
        PlayerInput();
    }

    private void PlayerInput()
    {
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 move_ = transform.right * X + transform.forward * Z;

        characterController.Move(move_ * moveSpeed * Time.deltaTime);

        velocity.y += gravity_ * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
