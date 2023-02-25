using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;
    [SerializeField] public NetworkManagerGame networkManager;

    [SerializeField] public GameObject PlayerNamePanel;
    [SerializeField] public GameObject HostOrJoinPanel;
    [SerializeField] public GameObject EnterIPAddressPanel;

    [SerializeField] public TMP_InputField playerNameInputField;
    [SerializeField] public TMP_InputField IpAddressField;
    [SerializeField] public Button returnToMainMenu;

    private const string PlayerPrefsNameKey = "PlayerName";

    public void ReturnToMainMenu() // Return play to main menu
    {
        PlayerNamePanel.SetActive(false);
        HostOrJoinPanel.SetActive(false);
        EnterIPAddressPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Setup() // Activate start UI and game
    {
        PlayerNamePanel.SetActive(false);
        HostOrJoinPanel.SetActive(false);
        EnterIPAddressPanel.SetActive(false);
        StartGame();
    }

    void MakeInstance() // Create player instance
    {
        if (instance == null)
            instance = this;
    }

    private void Awake() // On start call these functions
    {
        MakeInstance();
        Setup();
    }

    public void StartGame() // Start game (Multiplayer)
    {
        PlayerNamePanel.SetActive(true);
        GetSavedPlayerName();
    }
    private void GetSavedPlayerName() // Get saved player name
    {
        if (PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            playerNameInputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);
        }
    }
    public void SavePlayerName() // Save player name
    {
        string playerName = null;
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            playerName = playerNameInputField.text;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            PlayerNamePanel.SetActive(false);
            HostOrJoinPanel.SetActive(true);
        }
    }

    public void HostGame() // Create a host/server lobby
    {
        Debug.Log("Hosting a game...");
        networkManager.StartHost();
        HostOrJoinPanel.SetActive(false);
    }
    public void JoinGame() // Join a server
    {
        HostOrJoinPanel.SetActive(false);
        EnterIPAddressPanel.SetActive(true);
    }

    public void ConnectToGame() // Connect to specified IP address
    {
        if (!string.IsNullOrEmpty(IpAddressField.text))
        {
            Debug.Log("Client will connect to: " + IpAddressField.text);
            networkManager.networkAddress = IpAddressField.text;
            networkManager.StartClient();
        }
        EnterIPAddressPanel.SetActive(false);
    }
}
