using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float forwardSpeed = 20f, boostSpeed = 40f, hoverSpeed = 5f;
    private float activeForwardSpeed, activeBoostSpeed, activeHoverSpeed;
    private float forwardAcceleration = 5f, boostAcceleration = 10, hoverAcceleration = 2f;

    private float playerBoost;
    private bool playerBoostActive;
    public Image boostImage;

    public float lookRateSpeed = 90;
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rotateInput;
    public float rotateSpeed = 90f, rotateAcceleration = 3.5f;

    public Light engineLight;

    public Camera playerCamera;

    void Start()
    {
        if(!isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
        }

        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;
        engineLight.enabled = false;

        playerBoost = 100;
        playerBoostActive = true;

        StartCoroutine(restoreBoost());
    }

    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        engineLightSet();

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.x;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        rotateInput = Mathf.Lerp(rotateInput, Input.GetAxisRaw("Rotate"), rotateAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rotateInput * rotateSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey("w") && Input.GetKey(KeyCode.LeftShift) && playerBoostActive == true)
        {
            StartCoroutine(useBoost());

            activeBoostSpeed = Mathf.Lerp(activeBoostSpeed, boostSpeed, boostAcceleration * Time.deltaTime);
            transform.position += transform.forward * activeBoostSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("w"))
        {
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, forwardSpeed, forwardAcceleration * Time.deltaTime);
            transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        }
        else
        {
            activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, hoverSpeed, hoverAcceleration * Time.deltaTime);
            //transform.position += transform.forward * activeHoverSpeed * Time.deltaTime;
        }
    }

    private void engineLightSet()
    {
        if (Input.GetKeyDown("w"))
        {
            engineLight.enabled = true;
        }
        else if(Input.GetKeyUp("w"))
        {
            engineLight.enabled = false;
        }
    }

    IEnumerator useBoost()
    {
        while (playerBoostActive == true)
        {
            if (playerBoost > 0)
            {
                playerBoost -= 1;
                boostImage.fillAmount = playerBoost / 100;
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                playerBoostActive = false;
                yield return null;
            }
        }
    }

    IEnumerator restoreBoost()
    {
        while (true)
        {
            if (playerBoost < 100)
            {
                playerBoost += 1;
                boostImage.fillAmount = playerBoost / 100;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                playerBoostActive = true;
                yield return null;
            }
        }
    }
}
