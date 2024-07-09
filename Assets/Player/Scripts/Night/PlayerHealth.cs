using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(HealthValueChanged))] private float healtValue = 100f;
    [SerializeField] private TMP_Text healthText = null;
    [SerializeField] private Slider healthBar = null;

    [SerializeField] private GameObject playerCamera = null, deathCamera = null, playerModelMesh = null;
    [SerializeField] private NightMovement movementScript = null;
    [SerializeField] private CharacterController characterController = null;


    private void Start()
    {
        if (!isLocalPlayer) { return; }

        healthText.text = healtValue.ToString();
        healthBar.value = healtValue;
    }

    [Server]
    public void GetDamage(float damage_)
    {
        healtValue = Mathf.Max(0f, healtValue -= damage_);
    }

    private void HealthValueChanged(float oldHealth, float newHealth)
    {
        if (!isLocalPlayer) { return; }
        healthText.text = healtValue.ToString();
        healthBar.value = healtValue;

        if (newHealth <= 0)
        {
            deathCamera.SetActive(true);
            playerCamera.SetActive(false);
            playerModelMesh.SetActive(true);
            movementScript.enabled = false;
            characterController.enabled = false;
        }
    }

    public float GetHealth()
    {
        return healtValue;
    }
}
