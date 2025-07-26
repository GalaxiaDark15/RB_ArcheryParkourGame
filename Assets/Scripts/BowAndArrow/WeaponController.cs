using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    // [SerializeField]
    // private Text firePowerText;

    [SerializeField]
    private Bow bow;

    [SerializeField]
    private string enemyTag;

    [SerializeField]
    private float maxFirePower;

    [SerializeField]
    private float firePowerSpeed;

    private float firePower;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float minRotation;

    [SerializeField]
    private float maxRotation;

    private float mouseY;

    private bool fire;

    void Start()
    {
        bow.SetEnemyTag(enemyTag);
        bow.Reload();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed;
        mouseY = Mathf.Clamp(mouseY, minRotation, maxRotation);

        if (Input.GetMouseButtonDown(0))
        {
            fire = true;
        }

        if (fire && firePower < maxFirePower)
        {
            firePower += Time.deltaTime * firePowerSpeed;
        }

        if (fire && Input.GetMouseButtonUp(0))
        {
            bow.Fire(firePower);
            firePower = 0;
            fire = false;
        }

        if (fire)
        {
            // firePowerText.text = firePower.ToString();
        }
    }
}
