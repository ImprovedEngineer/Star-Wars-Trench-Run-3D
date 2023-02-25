using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour 
{
    public Vector3 m_target;
    public GameObject collisionExplosion;
    public float speed;
    public float damage;
    private bool isHit;
    public GameObject gunUser;

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (isHit)
        {
            explode();
        }
        else
        {
            if (transform.position == m_target)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, m_target, step);
        }
    }

    private void OnTriggerEnter(Collider other)
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
        if (other.gameObject.tag == "Turret" && gunUser.tag == "Player")
        {
            isHit = true;
            other.GetComponent<Towers>().takeDamage(damage);
        }
        if (other.gameObject.tag == "Player" && (gunUser.tag == "Enemies" || gunUser.tag == "Turret"))
        {
            isHit = true;
            other.GetComponent<Collision>().takeDamage(damage);
        }
    }

    public void setTarget(Vector3 target)
    {
        m_target = target;
    }

    public void targetHit(bool wasHit)
    {
        isHit = wasHit;
    }

    void explode()
    {
        if (collisionExplosion != null)
        {
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }
}
