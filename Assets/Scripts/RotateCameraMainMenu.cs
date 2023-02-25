using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraMainMenu : MonoBehaviour
{
    // Deathstar object
    public GameObject target;

    void Update()
    {
        // Rotate camera around deathstar
        transform.RotateAround(target.transform.position, Vector3.down, 1 * Time.deltaTime);
    }
}
