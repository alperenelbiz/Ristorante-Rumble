using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public Transform teamASpawn;
    public Transform teamBSpawn;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (teamASpawn == null || teamBSpawn == null)
        {
            Debug.LogError("Spawn points are not assigned.");
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned.");
            return;
        }

        if (TeamManager.Instance == null)
        {
            Debug.LogError("TeamManager instance is not found.");
            return;
        }

        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        TeamManager.Instance.AddPlayerToTeam(player);
    }

    [Server]
    public void RespawnPlayer(NetworkConnectionToClient conn, string assetId)
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        AssignPlayerToTeam(newPlayer);

        // Add player to connection
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer);
    }

    private void AssignPlayerToTeam(GameObject player)
    {
        PlayerNightMovement playerMovement = player.GetComponent<PlayerNightMovement>();
        if (playerMovement.team == "A")
        {
            player.transform.position = teamASpawn.position;
        }
        else if (playerMovement.team == "B")
        {
            player.transform.position = teamBSpawn.position;
        }

        TeamManager.Instance.AddPlayerToTeam(player);
    }
}
