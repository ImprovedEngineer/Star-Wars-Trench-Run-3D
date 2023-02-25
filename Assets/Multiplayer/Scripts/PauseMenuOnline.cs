using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuOnline : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject playerUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void resumeGame() // Remove pause UI
    {
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        isPaused = false;
    }

    private void pauseGame() // Activate pause UI
    {
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        isPaused = true;
    }

    public void mainMenu() // Return to main menu
    {
        resumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame() // Exit the game
    {
        Application.Quit();
    }
}
