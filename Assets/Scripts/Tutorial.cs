using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    // UI objects
    private bool hasMoved;
    private bool hasBoost;
    private bool hasChangedCam;
    private bool hasShot1;
    private bool hasShot2;
    private bool hasKilled;

    public GameObject hasMovedUI;
    public GameObject hasBoostUI;
    public GameObject hasChangedCamUI;
    public GameObject hasShot1UI;
    public GameObject hasShot2UI;
    public GameObject hasKilledUI;

    public GameObject enemy;

    // Setup start UI
    void Start()
    {
        hasMoved = false;
        hasBoost = false;
        hasChangedCam = false;
        hasShot1 = false;
        hasShot2 = false;

        hasMovedUI.SetActive(true);
        hasBoostUI.SetActive(false);
        hasChangedCamUI.SetActive(false);
        hasShot1UI.SetActive(false);
        hasShot2UI.SetActive(false);
    }

    void Update()
    {
        // Completed Tutorial -> go next scene (level 1)
        if (hasMoved == true & hasBoost == true & hasChangedCam == true & hasShot1 == true & hasShot2 == true & hasKilled == true)
        {
            SceneManager.LoadScene("Scene1");
        }

        // Tutotal levels

        if(hasMoved == false & hasBoost == false & hasShot1 == false & hasShot2 == false & hasKilled == false)
        {
            if(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d"))
            {
                hasMovedUI.SetActive(false);
                hasBoostUI.SetActive(true);
                hasMoved = true;
            }
        }

        if (hasMoved == true & hasBoost == false & hasChangedCam == false & hasShot1 == false & hasShot2 == false & hasKilled == false)
        {
            if (Input.GetKey("w") && Input.GetKey(KeyCode.LeftShift))
            {
                hasBoostUI.SetActive(false);
                hasChangedCamUI.SetActive(true);
                hasBoost = true;
            }
        }

        if (hasMoved == true & hasBoost == true & hasChangedCam == false & hasShot1 == false & hasShot2 == false & hasKilled == false)
        {
            if (Input.GetKey("c"))
            {
                hasChangedCamUI.SetActive(false);
                hasShot1UI.SetActive(true);
                hasChangedCam = true;
            }
        }

        if (hasMoved == true & hasBoost == true & hasChangedCam == true & hasShot1 == false & hasShot2 == false & hasKilled == false)
        {
            if (Input.GetMouseButton(0))
            {
                hasShot1UI.SetActive(false);
                hasShot2UI.SetActive(true);
                hasShot1 = true;
            }
        }

        if (hasMoved == true & hasBoost == true & hasChangedCam == true & hasShot1 == true & hasShot2 == false & hasKilled == false)
        {
            if (Input.GetMouseButton(1))
            {
                hasShot2UI.SetActive(false);
                hasKilledUI.SetActive(true);
                hasShot2 = true;
            }
        }

        if (hasMoved == true & hasBoost == true & hasChangedCam == true & hasShot1 == true & hasShot2 == true & hasKilled == false)
        {
            if (enemy == null)
            {
                hasKilledUI.SetActive(false);
                hasKilled = true;
            }
        }
    }
}
