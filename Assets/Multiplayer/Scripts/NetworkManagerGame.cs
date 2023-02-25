using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class NetworkManagerGame : NetworkManager
{
    [SerializeField] public int minPlayers = 2;
    [SerializeField] public LobbyPlayer lobbyPlayerPrefab;
    [SerializeField] public GamePlayer gamePlayerPrefab;
    public List<LobbyPlayer> LobbyPlayers { get; } = new List<LobbyPlayer>();
    public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();

    public override void OnStartClient() // Started a new client
    {
        Debug.Log("Starting client...");
    }

    [Obsolete]
    public override void OnClientConnect(NetworkConnection conn) // Client connected to server
    {
        Debug.Log("Client connected.");
        base.OnClientConnect(conn);
    }

    [Obsolete]
    public override void OnClientDisconnect(NetworkConnection conn) // Client disconnected from server
    {
        Debug.Log("Client disconnected.");
        base.OnClientDisconnect(conn);
    }

    public override void OnServerConnect(NetworkConnection conn) // Check on server connect if player is allowed to join
    {
        Debug.Log("Connecting to server...");
        if (numPlayers >= maxConnections) // prevents players from joining if the game is full
        {
            Debug.Log("Too many players. Disconnecting user.");
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().name != "MultiplayerLobby") // prevents players from joining a game that has already started. When the game starts, the scene will no longer be the "MultiplayerLobby"
        {
            Debug.Log("Player did not load from correct scene. Disconnecting user. Player loaded from scene: " + SceneManager.GetActiveScene().name);
            conn.Disconnect();
            return;
        }
        Debug.Log("Server Connected");
    }

    public override void OnServerAddPlayer(NetworkConnection conn) // Add player to player list
    {
        Debug.Log("Checking if player is in correct scene. Player's scene name is: " + SceneManager.GetActiveScene().name.ToString() + ". Correct scene name is: MultiplayerLobby");
        if (SceneManager.GetActiveScene().name == "MultiplayerLobby")
        {
            bool isGameLeader = LobbyPlayers.Count == 0; // isLeader is true if the player count is 0, aka when you are the first player to be added to a server/room

            LobbyPlayer lobbyPlayerInstance = Instantiate(lobbyPlayerPrefab);

            lobbyPlayerInstance.IsGameLeader = isGameLeader;
            lobbyPlayerInstance.ConnectionId = conn.connectionId;

            NetworkServer.AddPlayerForConnection(conn, lobbyPlayerInstance.gameObject);
            Debug.Log("Player added. Player name: " + lobbyPlayerInstance.PlayerName + ". Player connection id: " + lobbyPlayerInstance.ConnectionId.ToString());
        }
    }

    private bool CanStartGame() // Check if game can start
    {
        if (numPlayers < minPlayers)
            return false;
        foreach (LobbyPlayer player in LobbyPlayers)
        {
            if (!player.IsReady)
                return false;
        }
        return true;
    }

    public void StartGame() // Start game, change scene for all players
    {
        if (CanStartGame() && SceneManager.GetActiveScene().name == "MultiplayerLobby")
        {
            ServerChangeScene("MultiplayerGame");
        }
    }

    public override void ServerChangeScene(string newSceneName) // Change scene for every player and create/replace lobbyPlayer by gamePlayer
    {
        //Changing from the menu to the scene
        if (SceneManager.GetActiveScene().name == "MultiplayerLobby" && newSceneName == "MultiplayerGame")
        {
            for (int i = LobbyPlayers.Count - 1; i >= 0; i--)
            {
                var conn = LobbyPlayers[i].connectionToClient;
                var gamePlayerInstance = Instantiate(gamePlayerPrefab, getSpawnPoint(), Quaternion.identity);

                gamePlayerInstance.SetPlayerName(LobbyPlayers[i].PlayerName);
                gamePlayerInstance.SetConnectionId(LobbyPlayers[i].ConnectionId);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
            }
        }
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerDisconnect(NetworkConnection conn) // Remove player from player list when disconnect
    {
        if (conn.identity != null)
        {
            LobbyPlayer player = conn.identity.GetComponent<LobbyPlayer>();
            LobbyPlayers.Remove(player);
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer() // Remove all player when server closes
    {
        LobbyPlayers.Clear();
    }

    private Vector3 getSpawnPoint() // Spawn players within world space
    {
        float randomX = UnityEngine.Random.Range(-280, 190);
        float randomY = UnityEngine.Random.Range(10, 116);
        float randomZ = UnityEngine.Random.Range(-280, 190);

        return new Vector3(randomX, randomY, randomZ);
    }
}
