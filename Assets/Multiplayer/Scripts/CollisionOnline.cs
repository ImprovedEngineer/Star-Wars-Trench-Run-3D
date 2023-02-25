using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollisionOnline : MonoBehaviour
{
    public Camera mainCamera;

    public AudioListener mainCameraAudio;

    public AudioSource playerDeathSFX;

    public GameObject explosion;
    public GameObject returnToWorldUI;
    public Text timerText;

    float deathTimer = 10f;
    bool deathTimerActive;

    public float playerHealth = 100;
    public Image hpImage;

    public Button respawnButton;

    private void Start() // Setup initial player UI
    {
        returnToWorldUI.SetActive(false);
        deathTimerActive = false;
        respawnButton.gameObject.SetActive(false);
    }

    private void Update() // Check if player is out of world bound
    {
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
    }

    private void OnTriggerEnter(Collider other) // Check for collisions
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
    }

    private void OnTriggerExit(Collider other) // Check for collisions
    {
        if (other.gameObject.tag == "WorldBorder")
        {
            returnToWorldUI.SetActive(true);
            deathTimerActive = true;
        }
    }

    public void takeDamage(float damageTaken) // Player take damage
    {
        playerHealth -= damageTaken;
        hpImage.fillAmount = playerHealth / 100;
        if (playerHealth <= 0)
        {
            killPlayer();
        }
    }

    public void healPlayer(float heal) // Heal player
    {
        playerHealth += heal;

        if(playerHealth > 100)
        {
            playerHealth = 100;
        }

        hpImage.fillAmount = playerHealth / 100;
    }

    private void killPlayer() // Player killed
    {
        gameObject.GetComponent<PlayerControllerOnline>().isDead = true;
        //respawnButton.gameObject.SetActive(true);
        hpImage.fillAmount = 0;
        //Instantiate(explosion, mainCamera.transform.position, Quaternion.identity);
        //playerDeathSFX.PlayOneShot(playerDeathSFX.clip);
        respawnPlayer();
    }

    private void respawnPlayer()
    {
        gameObject.GetComponent<PlayerControllerOnline>().isDead = false;
        hpImage.fillAmount = 100;
        gameObject.transform.position = getSpawnPoint();
        gameObject.transform.rotation = Quaternion.Euler(0,0,0);
    }

    private Vector3 getSpawnPoint() // Spawn players within world space
    {
        float randomX = Random.Range(-280, 190);
        float randomY = Random.Range(10, 116);
        float randomZ = Random.Range(-280, 190);

        return new Vector3(randomX, randomY, randomZ);
    }
}
