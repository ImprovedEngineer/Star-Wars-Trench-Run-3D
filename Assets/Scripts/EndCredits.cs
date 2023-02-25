using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Play end credits deathstar explosion animation

public class EndCredits : MonoBehaviour
{
    public GameObject target;

    public GameObject UI;

    public ParticleSystem explosion1;
    public ParticleSystem explosion2;
    public ParticleSystem explosion3;
    public ParticleSystem explosion4;
    public ParticleSystem explosion5;
    public ParticleSystem explosion6;
    public ParticleSystem explosion7;
    public ParticleSystem explosion8;
    public ParticleSystem explosion9;
    public ParticleSystem explosion10;
    public ParticleSystem explosion11;
    public ParticleSystem explosion12;
    public ParticleSystem explosion13;
    public ParticleSystem explosion14;
    public ParticleSystem explosion15;

    void Update()
    {
        if(target != null)
        {
            transform.RotateAround(target.transform.position, Vector3.down, 1 * Time.deltaTime);
        }
    }

    void Start()
    {
        UI.SetActive(false);
        StartCoroutine(explosionTimer());
    }

    IEnumerator explosionTimer()
    {
        yield return new WaitForSeconds(2);
        playExplosion1();
        yield return new WaitForSeconds(2);
        playExplosion2();
        yield return new WaitForSeconds(2);
        Destroy(target);
        playExplosion3();
        yield return new WaitForSeconds(2);
        UI.SetActive(true);
    }

    private void playExplosion1()
    {
        explosion1.Play();
        explosion2.Play();
        explosion3.Play();
    }

    private void playExplosion2()
    {
        explosion4.Play();
        explosion5.Play();
        explosion6.Play();
        explosion7.Play();
        explosion8.Play();
        explosion9.Play();
    }

    private void playExplosion3()
    {
        explosion10.Play();
        explosion11.Play();
        explosion12.Play();
        explosion13.Play();
        explosion14.Play();
        explosion15.Play();
    }
}
