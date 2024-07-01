using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string team;

    public override void OnStartClient()
    {
        if (ManagerTeam.Instance == null)
        {
            Debug.LogError("TeamManager instance is not found in Player.");
            return;
        }

        ManagerTeam.Instance.AddPlayerToTeam(gameObject);
    }

    private void OnDestroy()
    {
        if (isServer && ManagerTeam.Instance != null)
        {
            ManagerTeam.Instance.RemovePlayerFromTeam(gameObject);
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
