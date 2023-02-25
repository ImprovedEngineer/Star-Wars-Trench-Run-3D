using UnityEngine;
using UnityEngine.UI;

public class Enemies : MonoBehaviour
{
    // Enemy objects
    private float health;
    private float maxHealth = 100f;
    public float bulletDamage = 5;

    public Transform player;
    public float rotationSpeed = 1f;
    public float speed = 15f;
    public Rigidbody rb;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;

    //Attacking
    public float timeBetweenAttacks;
    public float bulletRange;

    public float shootRate;
    private float m_shootRateTimeStamp;
    public GameObject m_shotPrefab;
    public AudioSource tieFighterShotSFX;
    public AudioSource tieFighterExplosionSFX;

    //States
    public float attackRange, sightRange;
    public bool playerInAttackRange, playerInSightRange;

    public GameObject healthBarUI;
    public Slider healthBar;

    public GameObject explosion;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    // Setup UI and enemy stats
    private void Start()
    {
        health = 100;
        healthBar.value = findHealth();
        healthBarUI.SetActive(false);
    }

    private void Update()
    {
        healthBar.transform.LookAt(player);

        // Check player range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // Do action if player is in range
        if (!playerInSightRange && !playerInAttackRange) patrolling();
        if (playerInSightRange && !playerInAttackRange) chasePlayer();
        if (playerInSightRange && playerInAttackRange) attackPlayer();

        // Update health stats
        healthBar.value = findHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // return HP
    private float findHealth()
    {
        return health/maxHealth;
    }

    // Deal dmg to enemy
    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
    }

    // Kill enemy
    public void Die()
    {
        tieFighterExplosionSFX.PlayOneShot(tieFighterExplosionSFX.clip);
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Patrol around map
    private void patrolling()
    {
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet)
        {
            Vector3 movePosition = Vector3.MoveTowards(transform.position, walkPoint, speed * Time.deltaTime);
            rb.MovePosition(movePosition);

            Quaternion targetRotation = Quaternion.LookRotation(walkPoint - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 10f)
        {
            walkPointSet = false;
        }
    }

    // Find new patrolling point
    private void searchWalkPoint()
    {
        float randomX = Random.Range(-280, 190);
        float randomY = Random.Range(10, 116);
        float randomZ = Random.Range(-280, 190);

        walkPoint = new Vector3(randomX, randomY, randomZ);
        walkPointSet = true;
    }

    // Follow player
    private void chasePlayer()
    {
        Vector3 movePosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        rb.MovePosition(movePosition);

        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Attack player
    private void attackPlayer()
    {
        chasePlayer();

        if (Time.time > m_shootRateTimeStamp)
        {
            GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;

            laser.GetComponent<ShotBehavior>().setTarget(player.position);

            m_shootRateTimeStamp = Time.time + shootRate;

            tieFighterShotSFX.PlayOneShot(tieFighterShotSFX.clip);
        }
    }
}
