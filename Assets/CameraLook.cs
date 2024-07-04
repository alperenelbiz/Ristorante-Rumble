using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    [SerializeField] float xSens = 200f, ySens = 200f;

    float xRotation = 0f;

    private void Update()
    {
        PlayerLook();
    }

    private void PlayerLook()
    {
        float xAngle = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime;
        float yAngle = Input.GetAxis("Mouse X") * xSens * Time.deltaTime;

        xRotation -= xAngle;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        player.transform.Rotate(0, yAngle, 0);
    }
}
