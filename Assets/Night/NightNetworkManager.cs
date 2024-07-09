using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NightNetworkManager : NetworkManager
{
    [SerializeField] private GameObject enterAddressPanel = null, landingPage = null, lobbyUI = null;
    [SerializeField] private TMP_InputField addressField = null;
    [SerializeField] private GameObject startGameButton = null;
    public List<PlayerScript> playersList = new List<PlayerScript>();
    [SerializeField] private GameObject playerGO = null;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        PlayerScript playerStartPrefab = conn.identity.GetComponent<PlayerScript>();

        playersList.Add(playerStartPrefab);

        if (playersList.Count == 2)
        {
            startGameButton.SetActive(true);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        PlayerScript playerStartPrefab = conn.identity.GetComponent<PlayerScript>();

        playersList.Remove(playerStartPrefab);

        startGameButton.SetActive(false);
    }

    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinButton()
    {
        enterAddressPanel.SetActive(true);
        landingPage.SetActive(false);
    }

    public void JoinLobby()
    {
        NetworkManager.singleton.networkAddress = addressField.text;
        NetworkManager.singleton.StartClient();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        lobbyUI.SetActive(true);
        landingPage.SetActive(false);
        enterAddressPanel.SetActive(false);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        landingPage.SetActive(true);
        lobbyUI.SetActive(false);
        enterAddressPanel.SetActive(false);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    public void StartGame()
    {
        ServerChangeScene("Night Phase");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (SceneManager.GetActiveScene().name.StartsWith("Night Phase"))
        {
            foreach (PlayerScript player in playersList)
            {
                var connectionTC = player.connectionToClient;
                GameObject playerP = Instantiate(playerGO, GetStartPosition().transform.position,Quaternion.identity);
                NetworkServer.ReplacePlayerForConnection(connectionTC, playerP);
            }
        }
    }
}
