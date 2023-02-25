using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootingOffline : MonoBehaviour
{
    // Player objects

    public GameObject player;

    public float range = 100f;
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;
    public AudioSource playerShotSFX;

    public Camera playerCamera;

    private float doubleClickTime = 2f;
    private float lastClickedTime;
    private bool hasClicked;
    public bool hasShotTorpedo;

    public List<GameObject> possibleTargets;
    private GameObject lockedTarget;
    public GameObject targetUI;
    public Image targetImageUI;
    private Vector3 offset = new Vector3(0f, 5f, 0f);

    public GameObject eventSystem;
    public GameObject tieFighterPrefab;
    private int numTieFighter;

    private void Start()
    {
        // Setup start for player
        if(eventSystem != null)
        {
            eventSystem.GetComponent<LevelSelector>().startLevel1();
            spawnEnemies();
            targetUI.SetActive(false);
        }
    }

    void Update()
    {
        if(eventSystem != null)
        {
            numTieFighter = GameObject.FindGameObjectsWithTag("Enemies").Length;

            // Update level
            if (numTieFighter <= 0 && eventSystem.GetComponent<LevelSelector>().getLevel1() == true && eventSystem.GetComponent<LevelSelector>().getLevel2() == false && eventSystem.GetComponent<LevelSelector>().getLevel1Done() == false && eventSystem.GetComponent<LevelSelector>().getLevel2Done() == false)
            {
                eventSystem.GetComponent<LevelSelector>().endLevel1();
                eventSystem.GetComponent<LevelSelector>().startLevel2();
                spawnEnemies();
            }
            else if (numTieFighter <= 0 && eventSystem.GetComponent<LevelSelector>().getLevel1() == false && eventSystem.GetComponent<LevelSelector>().getLevel2() == true && eventSystem.GetComponent<LevelSelector>().getLevel1Done() == true && eventSystem.GetComponent<LevelSelector>().getLevel2Done() == false)
            {
                eventSystem.GetComponent<LevelSelector>().endLevel2();
            }

            eventSystem.GetComponent<LevelSelector>().level1UI.GetComponent<Text>().text = numTieFighter + "/5";
            eventSystem.GetComponent<LevelSelector>().level2UI.GetComponent<Text>().text = numTieFighter + "/3";

            // Update locked target
            if (lockedTarget != null)
            {
                targetUI.transform.position = lockedTarget.transform.position + offset;
                targetImageUI.transform.LookAt(player.transform);
            }
        }

        if (PauseMenu.isPaused == false)
        {
            // Shoot gun
            if (Input.GetMouseButton(0))
            {
                if (Time.time > m_shootRateTimeStamp)
                {
                    ShootMainGun();
                    m_shootRateTimeStamp = Time.time + shootRate;
                }
            }

            float timeSinceLastClick = Time.time - lastClickedTime;

            if(eventSystem != null)
            {
                // Shoot torpedo
                if (Input.GetMouseButtonUp(1) && player.GetComponent<Collision>().torpedo != null)
                {
                    // Activate double click / show locked target
                    if (timeSinceLastClick <= doubleClickTime && hasClicked && player.GetComponent<Collision>().hasTorpedo == true)
                    {
                        hasClicked = false;
                        hasShotTorpedo = true;
                        player.GetComponent<Collision>().hasTorpedo = false;
                    }
                    else
                    {
                        hasClicked = true;
                        findClosest();
                    }

                    if (lockedTarget != null && hasClicked == true)
                    {
                        targetUI.SetActive(true);
                    }

                    lastClickedTime = Time.time;
                }
                else if (timeSinceLastClick > doubleClickTime)
                {
                    hasShotTorpedo = false;
                    hasClicked = false;
                    targetUI.SetActive(false);
                }

                // Shoot torpedo
                if (hasShotTorpedo && player.GetComponent<Collision>().torpedo != null && lockedTarget != null)
                {
                    eventSystem.GetComponent<TorpedoSpawn>().isTorpedoActive = false;
                    float step = 100 * Time.deltaTime;
                    player.GetComponent<Collision>().torpedo.transform.position = Vector3.MoveTowards(player.GetComponent<Collision>().torpedo.transform.position, lockedTarget.transform.position, step);
                    targetUI.SetActive(false);
                }
            }
        }
    }

    // Shoot gun
    void ShootMainGun()
    {
        GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;

        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        laser.GetComponent<ShotBehavior>().setTarget(rayOrigin + (playerCamera.transform.forward * range));

        playerShotSFX.PlayOneShot(playerShotSFX.clip);
    }

    // Find nearest target
    private void findClosest()
    {
        float closestDist = float.MaxValue;
        lockedTarget = null;

        foreach(GameObject targets in possibleTargets)
        {
            if(targets != null && targets.tag != "ExhaustVent")
            {
                if (Vector3.Distance(transform.position, targets.transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(transform.position, targets.transform.position);
                    lockedTarget = targets;
                }
            }
            else if (targets != null && targets.tag == "ExhaustVent") // End game
            {
                if(eventSystem.GetComponent<LevelSelector>().getLevel2Done() == true)
                {
                    if (Vector3.Distance(transform.position, targets.transform.position) < closestDist)
                    {
                        closestDist = Vector3.Distance(transform.position, targets.transform.position);
                        lockedTarget = targets;
                    }
                }
            }
        }
    }

    // Spawn Tie-fighters
    private void spawnEnemies()
    {
        if(eventSystem.GetComponent<LevelSelector>().getLevel1() == true)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject tempTieFighter = Instantiate(tieFighterPrefab, findSpawnPoint(), Quaternion.identity);
                possibleTargets.Add(tempTieFighter);
            }
        }

        if (eventSystem.GetComponent<LevelSelector>().getLevel2() == true)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject tempTieFighter = Instantiate(tieFighterPrefab, findSpawnPoint(), Quaternion.identity);
                possibleTargets.Add(tempTieFighter);
            }
        }
    }

    // Get random spawnpoint in map
    private Vector3 findSpawnPoint()
    {
        float randomX = Random.Range(-250, 150);
        float randomY = Random.Range(10, 100);
        float randomZ = Random.Range(-250, 150);

        return new Vector3(randomX, randomY, randomZ);
    }
}
