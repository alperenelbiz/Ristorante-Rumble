using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class TeamManager : NetworkBehaviour
{
    public static TeamManager Instance;

    public List<GameObject> teamA = new List<GameObject>();
    public List<GameObject> teamB = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayerToTeam(GameObject player)
    {
        if (teamA == null || teamB == null)
        {
            Debug.LogError("Team lists are not initialized.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player is null in AddPlayerToTeam.");
            return;
        }

        var playerComponent = player.GetComponent<PlayerNightMovement>();
        if (playerComponent == null)
        {
            Debug.LogError("Player component is not found on the player object.");
            return;
        }

        if (teamA.Count <= teamB.Count)
        {
            teamA.Add(player);
            playerComponent.team = "A";
        }
        else
        {
            teamB.Add(player);
            playerComponent.team = "B";
        }
    }

    public void RemovePlayerFromTeam(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null in RemovePlayerFromTeam.");
            return;
        }

        var playerComponent = player.GetComponent<PlayerNightMovement>();
        if (playerComponent == null)
        {
            Debug.LogError("Player component is not found on the player object.");
            return;
        }

        if (playerComponent.team == "A")
        {
            teamA.Remove(player);
        }
        else if (playerComponent.team == "B")
        {
            teamB.Remove(player);
        }
    }
}
