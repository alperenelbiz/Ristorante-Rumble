using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FireScript : NetworkBehaviour
{
    [SerializeField] private GameObject playerCamera = null;
    [SerializeField] private LayerMask playerMask = new LayerMask();
    private float lastShootTime = 0f;
    private float waitForSecondsBetweenShoots = 0.2f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (lastShootTime == 0 || lastShootTime + waitForSecondsBetweenShoots < Time.time)
            {
                lastShootTime += Time.time;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, playerMask))
                {
                    if (hit.collider.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealthScript))
                    {
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
}
