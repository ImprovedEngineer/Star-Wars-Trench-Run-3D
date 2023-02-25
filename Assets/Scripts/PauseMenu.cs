using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // UI objects
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject playerUI;

    void Update()
    {
        // Pause / Unpause
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

    // Pause / Unpause game
    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void pauseGame()
    {
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Return to mainmenu
    public void mainMenu()
    {
        resumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    // Exit game
    public void quitGame()
    {
        Application.Quit();
    }
}
