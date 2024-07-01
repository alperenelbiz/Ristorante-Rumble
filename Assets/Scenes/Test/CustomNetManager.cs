using UnityEngine;
using Mirror;

public class CustomNetManager : NetworkManager
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

        if (ManagerTeam.Instance == null)
        {
            Debug.LogError("TeamManager instance is not found.");
            return;
        }

        GameObject player = Instantiate(playerPrefab);

        // Assign the player to a team and set the spawn position
        if (ManagerTeam.Instance.teamA.Count <= ManagerTeam.Instance.teamB.Count)
        {
            player.transform.position = teamASpawn.position;
        }
        else
        {
            player.transform.position = teamBSpawn.position;
        }

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
