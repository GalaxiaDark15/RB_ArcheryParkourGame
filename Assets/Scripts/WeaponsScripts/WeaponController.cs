using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    // [SerializeField]
    // private Text firePowerText;

    [SerializeField]
    private SFXManager soundFXManager;

    [SerializeField]
    private Bow bow;

    [SerializeField]
    private float maxFirePower;

    [SerializeField]
    private float firePowerSpeed;

    [SerializeField]
    private GameObject reloadingText;

    // Store original values of Max Fire Power and Fire Power Speed
    private float originalMaxFirePower;

    private float originalFirePowerSpeed;

    private float firePower;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float minRotation;

    [SerializeField]
    private float maxRotation;

    private float mouseY;

    private bool fire;

    // Is the game paused?
    public bool isPaused = false;

    void Start()
    {
        // Store the original values when the game starts
        originalMaxFirePower = maxFirePower;
        originalFirePowerSpeed = firePowerSpeed;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isPaused) return;

        // Show or hide the Reloading text based on Bow's reload state
        if (reloadingText != null)
        {
            reloadingText.SetActive(bow.IsReloading());
        }

        // Block inputs while Bow is reloading
        if (bow.IsReloading()) return;

        mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;
        mouseY = Mathf.Clamp(mouseY, minRotation, maxRotation);

        if (Input.GetMouseButtonDown(0))
        {
            fire = true;
            soundFXManager.playDrawBowSound();
            bow.PrepareArrow();
        }

        if (fire && firePower < maxFirePower)
        {
            firePower += Time.deltaTime * firePowerSpeed;
        }

        if (fire && Input.GetMouseButtonUp(0))
        {
            soundFXManager.stopDrawBowSound();
            soundFXManager.playArrowSound();
            bow.Fire(firePower);
            firePower = 0;
            fire = false;
        }
    }


    public void SetBulletTimeFireValues()
    {
        maxFirePower = 1000f;
        firePowerSpeed = 2000f;
    }

    public void ResetFireValues()
    {
        maxFirePower = originalMaxFirePower;
        firePowerSpeed = originalFirePowerSpeed;
    }
}
