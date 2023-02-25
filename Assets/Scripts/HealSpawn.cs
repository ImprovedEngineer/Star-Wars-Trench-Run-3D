using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpawn : MonoBehaviour
{
    // Heal objects
    public GameObject heal;
    public MeshRenderer healImage;
    public Collider healCollider;
    public Light healLight;
    public bool isHealActive;
    private float healCounter;

    void Start()
    {
        isHealActive = false;
        healCounter = 1f;

        // Function to respawn heal
        StartCoroutine(respawnHeal());
    }

    IEnumerator respawnHeal()
    {
        while (true)
        {
            // Countdown timer for heal spawn
            if (isHealActive == false && healCounter > 0)
            {
                healCounter -= 1f;

                healImage.enabled = false;
                healCollider.enabled = false;
                healLight.enabled = false;

                yield return new WaitForSeconds(1f);
            }
            else if (isHealActive == false && healCounter == 0) // Countdown over, respawn heal
            {
                healCounter = 30f;
                isHealActive = true;
                heal.transform.position = findSpawnPoint();

                healImage.enabled = true;
                healCollider.enabled = true;
                healLight.enabled = true;

                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }

    // Find new spawnpoint for heal
    private Vector3 findSpawnPoint()
    {
        float randomX = Random.Range(-250, 150);
        float randomY = Random.Range(10, 100);
        float randomZ = Random.Range(-250, 150);

        return new Vector3(randomX, randomY, randomZ);
    }
}
