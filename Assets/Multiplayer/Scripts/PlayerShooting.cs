using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float damage = 25f;
    public float range = 100f;
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;
    public AudioSource playerShotSFX;
    RaycastHit hit;

    public Camera playerCamera;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time > m_shootRateTimeStamp)
            {
                ShootMainGun();
                m_shootRateTimeStamp = Time.time + shootRate;
            }
        }
    }

    void ShootMainGun()
    {
        GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemies enemies = hit.transform.GetComponent<Enemies>();

            laser.GetComponent<ShotBehavior>().targetHit(true);
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);

            if (enemies != null)
            {
                enemies.takeDamage(damage);
            }
        }
        else
        {
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            laser.GetComponent<ShotBehavior>().targetHit(false);
            laser.GetComponent<ShotBehavior>().setTarget(rayOrigin + (playerCamera.transform.forward * range));
            GameObject.Destroy(laser, 2f);
        }

        playerShotSFX.PlayOneShot(playerShotSFX.clip);
    }
}
