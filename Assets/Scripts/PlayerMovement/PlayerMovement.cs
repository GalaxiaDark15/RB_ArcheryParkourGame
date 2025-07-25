using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 5f;
    public float gravity = 9.81f * 2;   // Positive value, gravity magnitude
    public float jumpPower = 7f;         // Similar to jumpPower in XPlayerMovement

    private Vector3 moveDirection = Vector3.zero;

    private Vector3 lastPosition = Vector3.zero;
    private bool isMoving = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Get inputs on the horizontal plane
        float x = Input.GetAxis("Horizontal"); // Left/Right
        float z = Input.GetAxis("Vertical");   // Forward/Backward

        // Calculate horizontal movement relative to player orientation
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 horizontalMove = forward * z + right * x;
        horizontalMove *= speed;

        // Keep the current vertical velocity so gravity and jumping accumulate
        float verticalVelocity = moveDirection.y;

        // Jumping
        if (controller.isGrounded)
        {
            // If grounded, reset vertical velocity to a small negative to stick to ground
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpPower;
            }
        }
        else
        {
            // If in the air, apply gravity
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Update the moveDirection vector with horizontal and vertical velocities
        moveDirection = horizontalMove;
        moveDirection.y = verticalVelocity;

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);

        // Track movement state
        isMoving = (transform.position != lastPosition) && controller.isGrounded;
        lastPosition = transform.position;


        // Running
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            PlayerRun();
        }
        else
        {
            PlayerStopRunning();
        }
    }

    void PlayerRun()
    {
        speed = 9f;
    }

    void PlayerStopRunning()
    {
        speed = 5f;
    }
}
