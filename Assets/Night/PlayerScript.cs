using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class PlayerScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(handleSteamIdUpdated))] private ulong steamId;
    [SerializeField] private TMP_Text nameText= null;
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartClient();
    }

    public void SetSteamId(ulong steamId)
    {
        this.steamId = steamId;
    }

    private void handleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        var cSteamId = new CSteamID(newSteamId);

        nameText.text = SteamFriends.GetFriendPersonaName(cSteamId);
    }
}
