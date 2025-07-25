using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbobbing : MonoBehaviour
{
    public PlayerMovement playerMovement; // <-- Reference to PlayerMovement
    public GameObject Camera;

    void Update()
    {
        bool isPlayerMoving = false;
        if (playerMovement != null)
            isPlayerMoving = playerMovement.isMoving;

        if (!isPlayerMoving)
        {
            CameraCancelAnimation();
        }
        else
        {
            // Check if running or walking
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                CameraRunAnimation();
            }
            else
            {
                CameraWalkAnimation();
            }
        }
    }

    void CameraCancelAnimation()
    {
        Animator anim = Camera.GetComponent<Animator>();
        anim.Play("New State");
    }

    void CameraWalkAnimation()
    {
        Animator anim = Camera.GetComponent<Animator>();
        anim.speed = 1.0f;
        anim.Play("HeadBobbing");
    }

    void CameraRunAnimation()
    {
        Animator anim = Camera.GetComponent<Animator>();
        anim.speed = 1.5f;
        anim.Play("HeadBobbing");
    }
}
