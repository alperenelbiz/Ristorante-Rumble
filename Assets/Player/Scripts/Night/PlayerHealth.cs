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
        healthText.text = healtValue.ToString();
        healthBar.value = healtValue;

        if (newHealth <= 0)
        {
            print("Die");
        }
    }

    public float GetHealth()
    {
        return healtValue;
    }
}
