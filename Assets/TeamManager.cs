using Mirror;
using UnityEngine;

public enum Team
{
    TeamA,
    TeamB
}

public class TeamManager : NetworkBehaviour
{
    [SyncVar] public Team team;

    public Restaurant teamARestaurant;
    public Restaurant teamBRestaurant;

    private void Start()
    {
        if (isServer)
        {
            AssignTeam();
        }
    }

    private void AssignTeam()
    {
        // Example team assignment logic, you might want to change this
        team = NetworkServer.connections.Count % 2 == 0 ? Team.TeamA : Team.TeamB;
    }

    public Restaurant GetTeamRestaurant()
    {
        return team == Team.TeamA ? teamARestaurant : teamBRestaurant;
    }
}
