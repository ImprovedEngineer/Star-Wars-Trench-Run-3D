using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Collision : MonoBehaviour
{
    // Player objects / references

    public Camera mainCamera;
    public Camera deathCamera;

    public AudioListener mainCameraAudio;
    public AudioListener deathCameraAudio;

    public AudioSource playerDeathSFX;

    public GameObject explosion;
    public GameObject returnToWorldUI;
    public Text timerText;

    float deathTimer = 10f;
    bool deathTimerActive;

    public float playerHealth = 100;
    public Image hpImage;

    public Button respawnButton;

    public bool hasTorpedo;
    public GameObject torpedo;
    private RaycastHit Shot;
    private float distanceToPlayer;

    public GameObject eventSystem;

    private void Start()
    {
        // Setup starting UI
        returnToWorldUI.SetActive(false);
        deathTimerActive = false;
        respawnButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Return to map UI
        if (deathTimerActive)
        {
            deathTimer -= Time.deltaTime;
            string sec = (deathTimer % 60).ToString("f2");

            if(deathTimer >= 0)
            {
                timerText.text = "0" + sec;
            }

            if(deathTimer <= 0)
            {
                returnToWorldUI.SetActive(false);
                killPlayer();
            }
        }

        // Torpedo follows player
        if(hasTorpedo && torpedo != null)
        {
            torpedo.transform.LookAt(transform);

            if(Physics.Raycast(torpedo.transform.position, torpedo.transform.TransformDirection(Vector3.forward), out Shot))
            {
                distanceToPlayer = Shot.distance;

                if(distanceToPlayer >= 5f)
                {
                    float step = 25f * Time.deltaTime;

                    torpedo.transform.position = Vector3.MoveTowards(torpedo.transform.position, transform.position, step);
                }
            }
        }
    }

    // Player Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            returnToWorldUI.SetActive(false);
            killPlayer();
        }
        if (other.gameObject.tag == "Enemies")
        {
            killPlayer();
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "WorldBorder")
        {
            returnToWorldUI.SetActive(false);
            deathTimer = 10f;
            deathTimerActive = false;
        }
        if (other.gameObject.tag == "Turret")
        {
            killPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "WorldBorder")
        {
            returnToWorldUI.SetActive(true);
            deathTimerActive = true;
        }
        if (other.gameObject.tag == "Heal")
        {
            other.GetComponent<HealSpawn>().isHealActive = false;
            healPlayer(25);
        }
        if (other.gameObject.tag == "Torpedo")
        {
            hasTorpedo = true;
            torpedo = other.gameObject;
        }
    }

    // Player take dmg
    public void takeDamage(float damageTaken)
    {
        playerHealth -= damageTaken;
        hpImage.fillAmount = playerHealth / 100;
        if (playerHealth <= 0)
        {
            killPlayer();
        }
    }

    // Heal player
    public void healPlayer(float heal)
    {
        playerHealth += heal;

        if(playerHealth > 100)
        {
            playerHealth = 100;
        }

        hpImage.fillAmount = playerHealth / 100;
    }

    // Kill player
    private void killPlayer()
    {
        mainCameraAudio.enabled = false;
        deathCameraAudio.enabled = true;
        respawnButton.gameObject.SetActive(true);
        hpImage.fillAmount = 0;
        mainCamera.enabled = false;
        deathCamera.transform.position = mainCamera.transform.position;
        deathCamera.transform.rotation = mainCamera.transform.rotation;
        deathCamera.enabled = true;
        Instantiate(explosion, mainCamera.transform.position, Quaternion.identity);
        playerDeathSFX.PlayOneShot(playerDeathSFX.clip);
        gameObject.SetActive(false);
        
    }
}
