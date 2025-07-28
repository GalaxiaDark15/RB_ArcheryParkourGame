using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 1500f;

    float xRotation = 0f;
    float yRotation = 0f;

    // Clamp means that it limits you to 90 degrees up and 90 degrees down,
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get Mouse Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

        // Rotation around the x axis (Looking up and down)
        xRotation -= mouseY;
        
        // Clamp the rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Rotation around the y axis (Looking left and right)
        yRotation += mouseX;

        // Apply rotations to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

}
