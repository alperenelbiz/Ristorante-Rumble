using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkStuff : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerCamera = null
         , playerMesh = null
         ;

    private void Start()
    {
        if (isLocalPlayer)
        {
            playerCamera.SetActive(true);
            playerMesh.SetActive(false);
        }
        else
        {
            playerCamera.SetActive(false);
            playerMesh.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!isLocalPlayer) { return; }

            LeaveGame();
        }
    }

    public void LeaveGame()
    {
        if (isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
