using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Update scenes
public class MainMenuController : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void playMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
