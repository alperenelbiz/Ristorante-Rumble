using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string team;

    public override void OnStartClient()
    {
        if (TeamManager.Instance == null)
        {
            Debug.LogError("TeamManager instance is not found in Player.");
            return;
        }

        TeamManager.Instance.AddPlayerToTeam(gameObject);
    }

    private void OnDestroy()
    {
        if (isServer && TeamManager.Instance != null)
        {
            TeamManager.Instance.RemovePlayerFromTeam(gameObject);
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // Example team-specific behavior
            if (team == "A")
            {
                // Team A behavior
            }
            else if (team == "B")
            {
                // Team B behavior
            }
        }
    }
}
