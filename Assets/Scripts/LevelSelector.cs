using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    // UI objects
    private bool isLevel1 = false;
    private bool isLevel2 = false;
    private bool isLevel1Done = false;
    private bool isLevel2Done = false;
    private bool isLevelActive = false;

    public GameObject level1UI;
    public GameObject level1UIText;

    public GameObject level2UI;
    public GameObject level2UIText;

    // Setup start UI
    void Start()
    {
        level1UI.SetActive(false);
        level1UIText.SetActive(false);

        level2UI.SetActive(false);
        level2UIText.SetActive(false);
    }

    // Update UI as game goes
    void Update()
    {
        if (isLevelActive == false && isLevel1 == true && isLevel2 == false)
        {
            isLevelActive = true;
            level1();
        }
        if (isLevelActive == false && isLevel1 == false && isLevel2 == true)
        {
            isLevelActive = true;
            level2();
        }
    }

    // Level selector
    public void startLevel1()
    {
        isLevel1 = true;
    }

    public void endLevel1()
    {
        isLevel1 = false;
        isLevelActive = false;
        isLevel1Done = true;
    }

    public bool getLevel1()
    {
        return isLevel1;
    }

    public bool getLevel1Done()
    {
        return isLevel1Done;
    }

    public void startLevel2()
    {
        isLevel2 = true;
    }

    public void endLevel2()
    {
        isLevel2 = false;
        isLevelActive = false;
        isLevel2Done = true;
    }

    public bool getLevel2()
    {
        return isLevel2;
    }
    public bool getLevel2Done()
    {
        return isLevel2Done;
    }

    void level1()
    {
        StartCoroutine(level1UISetup());
    }

    // Delayed UI changer for visual effects
    IEnumerator level1UISetup()
    {
        yield return new WaitForSeconds(1);
        level1UI.SetActive(true);
        level1UIText.SetActive(true);
        yield return new WaitForSeconds(4);
        level1UIText.SetActive(false);
    }

    void level2()
    {
        level1UI.SetActive(false);

        StartCoroutine(level2UISetup());
    }

    // Delayed UI changer for visual effects
    IEnumerator level2UISetup()
    {
        yield return new WaitForSeconds(1);
        level2UI.SetActive(true);
        level2UIText.SetActive(true);
        yield return new WaitForSeconds(4);
        level2UIText.SetActive(false);
    }
}
