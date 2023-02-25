using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Towers : MonoBehaviour
{
    // Tower objects/stats
    private float health;
    private float maxHealth = 100f;
    public float damage = 10f;
    public float bulletSpeed = 100f;
    public float attackRange = 20f;

    public Transform towerHead;
    public Transform towerGunTips;

    public float shootRate;
    private float m_shootRateTimeStamp;
    public GameObject m_shotPrefab;
    public AudioSource towerShotSFX;
    public GameObject explosion;
    public AudioSource towerExplosion;

    public Transform playerTarget;
    public LayerMask whatIsPlayer;

    private bool playerInAttackRange;

    public GameObject healthBarUI;
    public Slider healthBar;

    // Setup starting tower stats and UI
    private void Start()
    {
        health = 100;
        healthBar.value = findHealth();
        healthBarUI.SetActive(false);
    }

    private void Update()
    {
        // Update health/towerUI
        healthBar.transform.LookAt(playerTarget);
        healthBar.value = findHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        // Check if player is in range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Attack player
        if (playerInAttackRange)
        {
            // Rotate tower head
            Vector3 directionToLookY = new Vector3(playerTarget.transform.position.x, towerHead.transform.position.y, playerTarget.transform.position.z);
            towerHead.transform.LookAt(directionToLookY);

            // Rotate gun tips
            Vector3 directionToLookX = playerTarget.position - towerGunTips.transform.position;
            Quaternion rotationX = Quaternion.LookRotation(directionToLookX);
            towerGunTips.transform.rotation = Quaternion.Lerp(towerGunTips.rotation, rotationX, 1f);

            if (Time.time > m_shootRateTimeStamp)
            {
                attackPlayer();
                m_shootRateTimeStamp = Time.time + shootRate;
            }
        }
    }

    // Attack player
    private void attackPlayer()
    {
        if (Time.time > m_shootRateTimeStamp)
        {
            GameObject laser = GameObject.Instantiate(m_shotPrefab, towerGunTips.transform.position, towerGunTips.transform.rotation) as GameObject;

            laser.GetComponent<ShotBehavior>().setTarget(playerTarget.position);

            m_shootRateTimeStamp = Time.time + shootRate;

            towerShotSFX.PlayOneShot(towerShotSFX.clip);
        }
    }

    // Return tower HP
    private float findHealth()
    {
        return health / maxHealth;
    }

    // Damage tower
    public void takeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(Die), 0.5f);
    }

    // Destroy tower
    public void Die()
    {
        towerExplosion.PlayOneShot(towerExplosion.clip);
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
