using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Linq;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))] public string PlayerName;
    [SyncVar] public int ConnectionId;

    public bool IsGameLeader = false;
    [SyncVar(hook = nameof(HandlePlayerReadyStatusUpdate))] public bool IsReady = false;

    [SerializeField] public GameObject PlayerLobbyUI;
    [SerializeField] public GameObject Player1ReadyPanel;
    [SerializeField] public GameObject Player2ReadyPanel;
    [SerializeField] public GameObject Player3ReadyPanel;
    [SerializeField] public GameObject Player4ReadyPanel;
    [SerializeField] public GameObject Player5ReadyPanel;
    [SerializeField] public GameObject Player6ReadyPanel;

    [SerializeField] public GameObject startGameButton;
    [SerializeField] public Button readyButton;

    private const string PlayerPrefsNameKey = "PlayerName";

    // Create reference to NetworkManager
    private NetworkManagerGame game;
    private NetworkManagerGame Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = NetworkManagerGame.singleton as NetworkManagerGame;
        }
    }

    public override void OnStartClient() // Add player to player list when client starts
    {
        Game.LobbyPlayers.Add(this);
        Debug.Log("Added to GamePlayer list: " + this.PlayerName);
    }

    public override void OnStopClient() // Remove player from player list on client stop
    {
        Debug.Log(PlayerName + " is quiting the game.");
        Game.LobbyPlayers.Remove(this);
        Debug.Log("Removed player from the GamePlayer list: " + this.PlayerName);
    }

    public override void OnStartAuthority() // Check if player is host
    {
        CmdSetPlayerName(PlayerPrefs.GetString(PlayerPrefsNameKey));
        if (!PlayerLobbyUI.activeInHierarchy)
            PlayerLobbyUI.SetActive(true);
        gameObject.name = "LocalLobbyPlayer";
    }
    [Command]
    private void CmdSetPlayerName(string playerName) // Set player name
    {
        PlayerName = playerName;
        Debug.Log("Player display name set to: " + playerName);
    }

    public void UpdateLobbyUI() // Activate player UI for clients
    {
        Debug.Log("Updating UI for: " + this.PlayerName);
        GameObject localPlayer = GameObject.Find("LocalLobbyPlayer");
        if (localPlayer != null)
        {
            localPlayer.GetComponent<LobbyPlayer>().ActivateLobbyUI();
        }
    }

    public void ActivateLobbyUI() // Activate player lobby UI (up to 6 players)
    {
        Debug.Log("Activating lobby UI");
        if (!PlayerLobbyUI.activeInHierarchy)
            PlayerLobbyUI.SetActive(true);

        if (Game.LobbyPlayers.Count() > 0)
        {
            Player1ReadyPanel.SetActive(true);
            Debug.Log("Player1 Ready Panel activated");
        }
        else
        {
            Debug.Log("Player1 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
        }
        if (Game.LobbyPlayers.Count() > 1)
        {
            Player2ReadyPanel.SetActive(true);
            Debug.Log("Player2 Ready Panel activated");
        }
        else
        {
            Debug.Log("Player2 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
        }
        if (Game.LobbyPlayers.Count() > 2)
        {
            Player3ReadyPanel.SetActive(true);
            Debug.Log("Player3 Ready Panel activated");
        }
        else
        {
            Debug.Log("Player3 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
        }
        if (Game.LobbyPlayers.Count() > 3)
        {
            Player4ReadyPanel.SetActive(true);
            Debug.Log("Player4 Ready Panel activated");
        }
        else
        {
            Debug.Log("Player4 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
        }
        if (Game.LobbyPlayers.Count() > 4)
        {
            Player5ReadyPanel.SetActive(true);
            Debug.Log("Player5 Ready Panel activated");
        }
        else
        {
            Debug.Log("Player5 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
        }
        if (Game.LobbyPlayers.Count() > 5)
        {
            Player6ReadyPanel.SetActive(true);
            Debug.Log("Player6 Ready Panel activated");
        }
        else
        {
            Debug.Log("Player6 Ready Panel not activated. Player count: " + Game.LobbyPlayers.Count().ToString());
        }
        UpdatePlayerReadyText();
    }

    public void UpdatePlayerReadyText() // Update ready status UI of lobby player
    {
        if (Player1ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 0)
        {
            foreach (Transform childText in Player1ReadyPanel.transform)
            {
                if (childText.name == "Player1Name")
                    childText.GetComponent<Text>().text = Game.LobbyPlayers[0].PlayerName;
                if (childText.name == "Player1ReadyText")
                {
                    bool isPlayerReady = Game.LobbyPlayers[0].IsReady;
                    if (isPlayerReady)
                    {
                        childText.GetComponent<Text>().text = "Ready";
                        childText.GetComponent<Text>().color = Color.green;
                    }
                    else
                    {
                        childText.GetComponent<Text>().text = "Not Ready";
                        childText.GetComponent<Text>().color = Color.red;
                    }
                }
            }
        }
        if (Player2ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 1)
        {
            foreach (Transform childText in Player2ReadyPanel.transform)
            {
                if (childText.name == "Player2Name")
                    childText.GetComponent<Text>().text = Game.LobbyPlayers[1].PlayerName;
                if (childText.name == "Player2ReadyText")
                {
                    bool isPlayerReady = Game.LobbyPlayers[1].IsReady;
                    if (isPlayerReady)
                    {
                        childText.GetComponent<Text>().text = "Ready";
                        childText.GetComponent<Text>().color = Color.green;
                    }
                    else
                    {
                        childText.GetComponent<Text>().text = "Not Ready";
                        childText.GetComponent<Text>().color = Color.red;
                    }
                }
                Debug.Log("Updated Player2 Ready panel with player name: " + Game.LobbyPlayers[1].PlayerName + " and ready status: " + Game.LobbyPlayers[1].IsReady);
            }
        }
        if (Player3ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 2)
        {
            foreach (Transform childText in Player2ReadyPanel.transform)
            {
                if (childText.name == "Player3Name")
                    childText.GetComponent<Text>().text = Game.LobbyPlayers[2].PlayerName;
                if (childText.name == "Player3ReadyText")
                {
                    bool isPlayerReady = Game.LobbyPlayers[2].IsReady;
                    if (isPlayerReady)
                    {
                        childText.GetComponent<Text>().text = "Ready";
                        childText.GetComponent<Text>().color = Color.green;
                    }
                    else
                    {
                        childText.GetComponent<Text>().text = "Not Ready";
                        childText.GetComponent<Text>().color = Color.red;
                    }
                }
                Debug.Log("Updated Player3 Ready panel with player name: " + Game.LobbyPlayers[2].PlayerName + " and ready status: " + Game.LobbyPlayers[2].IsReady);
            }
        }
        if (Player4ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 3)
        {
            foreach (Transform childText in Player2ReadyPanel.transform)
            {
                if (childText.name == "Player4Name")
                    childText.GetComponent<Text>().text = Game.LobbyPlayers[3].PlayerName;
                if (childText.name == "Player4ReadyText")
                {
                    bool isPlayerReady = Game.LobbyPlayers[3].IsReady;
                    if (isPlayerReady)
                    {
                        childText.GetComponent<Text>().text = "Ready";
                        childText.GetComponent<Text>().color = Color.green;
                    }
                    else
                    {
                        childText.GetComponent<Text>().text = "Not Ready";
                        childText.GetComponent<Text>().color = Color.red;
                    }
                }
                Debug.Log("Updated Player4 Ready panel with player name: " + Game.LobbyPlayers[3].PlayerName + " and ready status: " + Game.LobbyPlayers[3].IsReady);
            }
        }
        if (Player5ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 4)
        {
            foreach (Transform childText in Player5ReadyPanel.transform)
            {
                if (childText.name == "Player5Name")
                    childText.GetComponent<Text>().text = Game.LobbyPlayers[4].PlayerName;
                if (childText.name == "Player5ReadyText")
                {
                    bool isPlayerReady = Game.LobbyPlayers[4].IsReady;
                    if (isPlayerReady)
                    {
                        childText.GetComponent<Text>().text = "Ready";
                        childText.GetComponent<Text>().color = Color.green;
                    }
                    else
                    {
                        childText.GetComponent<Text>().text = "Not Ready";
                        childText.GetComponent<Text>().color = Color.red;
                    }
                }
                Debug.Log("Updated Player5 Ready panel with player name: " + Game.LobbyPlayers[4].PlayerName + " and ready status: " + Game.LobbyPlayers[4].IsReady);
            }
        }
        if (Player6ReadyPanel.activeInHierarchy && Game.LobbyPlayers.Count() > 5)
        {
            foreach (Transform childText in Player6ReadyPanel.transform)
            {
                if (childText.name == "Player6Name")
                    childText.GetComponent<Text>().text = Game.LobbyPlayers[5].PlayerName;
                if (childText.name == "Player6ReadyText")
                {
                    bool isPlayerReady = Game.LobbyPlayers[5].IsReady;
                    if (isPlayerReady)
                    {
                        childText.GetComponent<Text>().text = "Ready";
                        childText.GetComponent<Text>().color = Color.green;
                    }
                    else
                    {
                        childText.GetComponent<Text>().text = "Not Ready";
                        childText.GetComponent<Text>().color = Color.red;
                    }
                }
                Debug.Log("Updated Player6 Ready panel with player name: " + Game.LobbyPlayers[5].PlayerName + " and ready status: " + Game.LobbyPlayers[5].IsReady);
            }
        }
    }

    [Command]
    public void CmdReadyUp() // Update ready status
    {
        IsReady = !IsReady;
        Debug.Log("Ready status changed for: " + PlayerName);
    }

    public void HandlePlayerNameUpdate(string oldValue, string newValue) // Update player name text
    {
        Debug.Log("Player name has been updated for: " + oldValue + " to new value: " + newValue);
        UpdateLobbyUI();
    }

    public void HandlePlayerReadyStatusUpdate(bool oldValue, bool newValue) // Update player ready text
    {
        Debug.Log("Player ready status has been has been updated for " + this.PlayerName + ": " + oldValue + " to new value: " + newValue);
        UpdateLobbyUI();
        CheckIfAllPlayersAreReady();
    }

    public void CheckIfAllPlayersAreReady() // Check if players are all ready, if yes, activate start game button
    {
        Debug.Log("Checking if all players are ready.");
        bool arePlayersReady = false;
        foreach (LobbyPlayer player in Game.LobbyPlayers)
        {
            if (!player.IsReady)
            {
                Debug.Log(player.PlayerName + " is not ready.");
                arePlayersReady = false;
                startGameButton.SetActive(false);
                break;
            }
            else
            {
                arePlayersReady = true;
            }

        }
        if (arePlayersReady)
            Debug.Log("All players are ready");

        if (arePlayersReady && IsGameLeader && Game.LobbyPlayers.Count() >= Game.minPlayers)
        {
            Debug.Log("All players are ready and minimum number of players in game. Activating the StartGame button on Game leader's UI.");
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }

    [Command]
    public void CmdStartGame() // Start actual game
    {
        Game.StartGame();
    }

    public void QuitLobby() // Exit server, stop clients or server
    {
        if (hasAuthority)
        {
            if (IsGameLeader)
            {

                Game.StopHost();
            }
            else
            {

                Game.StopClient();
            }
        }
    }
}
