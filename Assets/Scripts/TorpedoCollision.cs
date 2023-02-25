using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manage torpedo collisions

public class TorpedoCollision : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == "Terrain")
        {
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }*/
        if (other.gameObject.tag == "Enemies") // Hit on ennemies
        {
            other.GetComponent<Enemies>().Die();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Turret") // Hit on turret
        {
            other.GetComponent<Towers>().Die();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "ExhaustVent") // Hit on exhaust vent, end game
        {
            SceneManager.LoadScene("EndCredits");
        }
    }
}
