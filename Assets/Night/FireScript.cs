using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class FireScript : NetworkBehaviour
{
    [SerializeField] private GameObject playerCamera = null;
    [SerializeField] private LayerMask playerMask = new LayerMask();
    private float lastShootTime = 0f;
    private float waitForSecondsBetweenShoots = 0.2f;

    [SerializeField] private GameObject damageTextParent = null;
    [SerializeField] private PlayerHealth playerHealthScript;

    //[SerializeField] private GameObject DeathPanel = null;
    //[SerializeField] private TMP_Text winnerText = null;

    private void Update()
    {
        if (!isLocalPlayer) { return; }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (lastShootTime == 0 || lastShootTime + waitForSecondsBetweenShoots < Time.time)
            {
                lastShootTime = Time.time;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, playerMask))
                {
                    if (hit.collider.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealthScript))
                    {
                        if (playerHealthScript.GetHealth() - 25 <= 0)
                        {
                            //DeathPanel.SetActive(true);
                            //winnerText.text = "You Won!";
                            //RoundOver();
                        }

                        if (playerHealthScript.GetHealth() <= 0f) { return; }

                        GameObject newDamageTextParent = Instantiate(damageTextParent, hit.point, Quaternion.identity);
                        newDamageTextParent.GetComponentInChildren<DamageTextScript>().GetCalled(25, playerCamera);
                        if (isServer)
                        {
                            ServerHit(25, playerHealthScript);
                            return;
                        }

                        CmdHit(25, playerHealthScript);
                    }
                }
            }
        }
    }

    [Command]
    private void CmdHit(float damage, PlayerHealth playerHealthScript)
    {
        ServerHit(damage, playerHealthScript);
    }

    [Server]
    private void ServerHit(float damage, PlayerHealth playerHealthScript)
    {
        playerHealthScript.GetDamage(damage);
    }

    private void RoundOver()
    {
        Invoke(nameof(BeginNewRound), 5f);
    }

    private void BeginNewRound()
    {
        playerHealthScript.BeginNewRound();
    }
}
