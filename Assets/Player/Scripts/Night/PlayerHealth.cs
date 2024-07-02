using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private CustomNetworkManager networkManager;

    private void Start()
    {
        currentHealth = maxHealth;
        networkManager = NetworkManager.singleton as CustomNetworkManager;
    }

    [Server]
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    [Server]
    void Die()
    {
        Debug.Log("Player has died!");
        RpcOnDeath(); // Notify the client that the player has died
        currentHealth = maxHealth; // Reset health
        StartCoroutine(Respawn());
    }

    [ClientRpc]
    void RpcOnDeath()
    {
        // Add any client-side death effects here, e.g., play sound, show UI
    }

    [Server]
    private IEnumerator Respawn()
    {
        NetworkIdentity identity = GetComponent<NetworkIdentity>();
        NetworkConnectionToClient conn = identity.connectionToClient;

        yield return new WaitForSeconds(5); // 5 seconds respawn time

        TeamManager.Instance.RemovePlayerFromTeam(gameObject);
        networkManager.RespawnPlayer(conn, identity.assetId.ToString());
    }
}
