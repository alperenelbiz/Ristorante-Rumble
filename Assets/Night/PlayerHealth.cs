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
    [SerializeField] private FireScript fireScript= null;
    private Vector3 startPosition;

    [SerializeField] private GameObject DeathPanel = null;
    [SerializeField] private TMP_Text winnerText = null;

    private void Start()
    {
        if (!isLocalPlayer) { return; }

        startPosition = transform.position;
        healthText.text = healtValue.ToString();
        healthBar.value = healtValue;
    }

    public void NewRoundCall()
    {
        CmdMaxHealth();
    }

    [Command]
    private void CmdMaxHealth()
    {
        ServerMaxHealth();
    }

    [Server]
    private void ServerMaxHealth()
    {
        healtValue = 100;
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
            movementScript.enabled = false;
            characterController.enabled = false;
            fireScript.enabled = false;
            DeathPanel.SetActive(true);
            winnerText.text = "You Lost!";
            deathCamera.SetActive(true);
            playerCamera.SetActive(false);
            playerModelMesh.SetActive(true);

            Invoke(nameof(BeginNewRound), 5f);
        }
    }

    public void BeginNewRound()
    {
        fireScript.enabled = true;
        DeathPanel.SetActive(false );
        NewRoundCall();

        movementScript.enabled = false;
        characterController.enabled = false;

        transform.position = startPosition;
        playerCamera.SetActive(true);
        deathCamera.SetActive(false);
        playerModelMesh.SetActive(false);
        movementScript.enabled = true;
        characterController.enabled = true;
    }

    public float GetHealth()
    {
        return healtValue;
    }
}
