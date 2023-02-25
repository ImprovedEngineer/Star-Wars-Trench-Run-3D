using UnityEngine;
using System.Collections;
using Mirror;

public class ShotBehaviorOnline : NetworkBehaviour 
{
    public Vector3 m_target;
    public GameObject collisionExplosion;
    public float speed;
    public float damage;
    private bool isHit;
    public GameObject gunUser;

    void Update() // Update every fram
    {
        float step = speed * Time.deltaTime;

        if (isHit) // Check for hit
        {
            explode();
        }
        else
        {
            if (transform.position == m_target) // Destroy laser
            {
                Destroy(gameObject);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, m_target, step); // Move laser towards target
        }
    }

    private void OnTriggerEnter(Collider other) // Check for collisions
    {
        if (other.gameObject.tag == "Terrain")
        {
            isHit = true;
        }
        if (other.gameObject.tag == "Enemies" && gunUser.tag == "Player")
        {
            isHit = true;
            other.GetComponent<Enemies>().takeDamage(damage);
        }
        if (other.gameObject.tag == "Player" && gunUser.tag == "Enemies")
        {
            isHit = true;
            other.GetComponent<Collision>().takeDamage(damage);
        }
    }

    public void setTarget(Vector3 target) // Set target location in the world
    {
        m_target = target;
    }

    public void targetHit(bool wasHit) // Update if theres a hit
    {
        isHit = wasHit;
    }

    void explode() // Spawn explosion animation
    {
        if (collisionExplosion != null)
        {
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, m_target, transform.rotation);
            NetworkServer.Spawn(explosion);
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }
}
