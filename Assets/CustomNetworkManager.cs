using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public Transform redTeamSpawn;
    public Transform blueTeamSpawn;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Create the player object from the player prefab
        GameObject player = Instantiate(playerPrefab);

        // Assign the player to the connection
        NetworkServer.AddPlayerForConnection(conn, player);

        // Get the PlayerController component
        var playerController = player.GetComponent<PlayerKitchenMovement>();

        // Determine the team based on the current number of players
        int teamId = NetworkServer.connections.Count % 2;
        //playerController.teamId = teamId;

        // Set the player's spawn position based on the team
        if (teamId == 0)
        {
            player.transform.position = redTeamSpawn.position;
        }
        else
        {
            player.transform.position = blueTeamSpawn.position;
        }
    }
}
