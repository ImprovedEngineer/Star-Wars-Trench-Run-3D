using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoSpawn : MonoBehaviour
{
    // Torpedo objects
    public GameObject torpedo;
    public bool isTorpedoActive;
    private float torpedoCounter;

    // Setup torpedo
    void Start()
    {
        isTorpedoActive = false;
        torpedoCounter = 1f;

        // Function to respawn torpedo
        StartCoroutine(respawnTorpedo());
    }

    // Respawn torpedo
    IEnumerator respawnTorpedo()
    {
        while (true)
        {
            // Countdown till respawn
            if (isTorpedoActive == false && torpedoCounter > 0)
            {
                torpedoCounter -= 1;
                yield return new WaitForSeconds(1f);
            }
            else if (isTorpedoActive == false && torpedoCounter == 0) // Respawn torpedo
            {
                torpedoCounter = 5f;
                isTorpedoActive = true;
                Instantiate(torpedo, findSpawnPoint(), Quaternion.identity);

                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }


    // Find random point in map
    private Vector3 findSpawnPoint()
    {
        float randomX = Random.Range(-250, 150);
        float randomY = Random.Range(10, 100);
        float randomZ = Random.Range(-250, 150);

        return new Vector3(randomX, randomY, randomZ);
    }
}
