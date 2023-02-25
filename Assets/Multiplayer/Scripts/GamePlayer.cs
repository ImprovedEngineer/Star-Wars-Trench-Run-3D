using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Linq;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar] public string PlayerName;
    [SyncVar] public int ConnectionId;

    // Make a reference to NetworkManager
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
    public override void OnStartAuthority() // Set host authority
    {
        gameObject.name = "LocalGamePlayer";
        Debug.Log("Labeling the local player: " + this.PlayerName);
    }
    public override void OnStartClient() // Add player to game player list
    {
        DontDestroyOnLoad(gameObject);
        Game.GamePlayers.Add(this);
        Debug.Log("Added to GamePlayer list: " + this.PlayerName);
    }
    public override void OnStopClient() // Remove player from game player list
    {
        Debug.Log(PlayerName + " is quiting the game.");
        Game.GamePlayers.Remove(this);
        Debug.Log("Removed player from the GamePlayer list: " + this.PlayerName);
    }
    [Server]
    public void SetPlayerName(string playerName) // Set player name to gamePlayer object
    {
        this.PlayerName = playerName;
    }
    [Server]
    public void SetConnectionId(int connId) // Set connection id to gamePlayer object
    {
        this.ConnectionId = connId;
    }
}
