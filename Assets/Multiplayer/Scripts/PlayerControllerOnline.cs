using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerControllerOnline : NetworkBehaviour
{
    public float forwardSpeed = 20f, boostSpeed = 40f, hoverSpeed = 5f;
    private float activeForwardSpeed, activeBoostSpeed, activeHoverSpeed;
    private float forwardAcceleration = 5f, boostAcceleration = 10, hoverAcceleration = 2f;

    private float playerBoost;
    private bool playerBoostActive;
    public Image boostImage;

    public float lookRateSpeed = 90;
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rotateInput;
    public float rotateSpeed = 90f, rotateAcceleration = 3.5f;

    public Light engineLight;

    public Camera playerCamera;

    public float damage = 25f;
    public float range = 100f;
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;
    public AudioSource playerShotSFX;
    RaycastHit hit;

    public bool isDead;

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

    void Start()
    {
        if (!isLocalPlayer) // Check if its local player, if good then activate its camera
        {
            playerCamera.gameObject.SetActive(false);
        }

        isDead = false;

        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;
        engineLight.enabled = false;

        playerBoost = 100;
        playerBoostActive = true;

        StartCoroutine(restoreBoost());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if(!isDead) // If dead disable player controls
        {
            if (Input.GetMouseButton(0)) // Player wants to shoot
            {
                if (Time.time > m_shootRateTimeStamp)
                {
                    ShootMainGun();
                    m_shootRateTimeStamp = Time.time + shootRate;
                }
            }

            engineLightSet(); // Activate engine lights

            // Move spaceship

            lookInput.x = Input.mousePosition.x;
            lookInput.y = Input.mousePosition.y;

            mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.x;
            mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
            rotateInput = Mathf.Lerp(rotateInput, Input.GetAxisRaw("Rotate"), rotateAcceleration * Time.deltaTime);

            transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rotateInput * rotateSpeed * Time.deltaTime, Space.Self);

            if (Input.GetKey("w") && Input.GetKey(KeyCode.LeftShift) && playerBoostActive == true) // Move with boost
            {
                StartCoroutine(useBoost());

                activeBoostSpeed = Mathf.Lerp(activeBoostSpeed, boostSpeed, boostAcceleration * Time.deltaTime);
                transform.position += transform.forward * activeBoostSpeed * Time.deltaTime;
            }
            else if (Input.GetKey("w")) // Move regular
            {
                activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, forwardSpeed, forwardAcceleration * Time.deltaTime);
                transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
            }
            else // Move hover
            {
                activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, hoverSpeed, hoverAcceleration * Time.deltaTime);
                transform.position += transform.forward * activeHoverSpeed * Time.deltaTime;
            }
        }
    }

    private void engineLightSet() // Activate engine lights
    {
        if (Input.GetKeyDown("w"))
        {
            engineLight.enabled = true;
        }
        else if (Input.GetKeyUp("w"))
        {
            engineLight.enabled = false;
        }
    }

    IEnumerator useBoost() // Use boost
    {
        while (playerBoostActive == true)
        {
            if (playerBoost > 0)
            {
                playerBoost -= 1;
                boostImage.fillAmount = playerBoost / 100;
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                playerBoostActive = false;
                yield return null;
            }
        }
    }

    IEnumerator restoreBoost() // Restore boost
    {
        while (true)
        {
            if (playerBoost < 100)
            {
                playerBoost += 1;
                boostImage.fillAmount = playerBoost / 100;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                playerBoostActive = true;
                yield return null;
            }
        }
    }

    void ShootMainGun() // Create laser and set target in the world
    {
        GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;

        NetworkServer.Spawn(laser);

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemies enemies = hit.transform.GetComponent<Enemies>();

            laser.GetComponent<ShotBehaviorOnline>().targetHit(true);
            laser.GetComponent<ShotBehaviorOnline>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);

            if (enemies != null)
            {
                enemies.takeDamage(damage);
            }
        }
        else
        {
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            laser.GetComponent<ShotBehaviorOnline>().targetHit(false);
            laser.GetComponent<ShotBehaviorOnline>().setTarget(rayOrigin + (playerCamera.transform.forward * range));
            GameObject.Destroy(laser, 2f);
        }

        playerShotSFX.PlayOneShot(playerShotSFX.clip);
    }
}
