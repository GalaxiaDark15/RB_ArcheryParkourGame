using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float gravity = 9.81f * 2;
    public float jumpPower = 7f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lastPosition = Vector3.zero;
    private float verticalVelocity = 0f;

    public bool isMoving = false;
    public bool wallRunning = false;
    private bool isWallJumping = false;
    private float wallJumpDuration = 0.2f;
    private float wallJumpTimer = 0f;


    private float currentSpeed;

    [Header("References")]
    public TimeManager timeManager;


    // Is the game paused?
    public bool isPaused = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
        currentSpeed = walkSpeed; // default speed
    }

    void Update()
    {
        if (isPaused != true)
        {
            if (isWallJumping)
            {
                wallJumpTimer -= Time.deltaTime;
                if (wallJumpTimer <= 0f)
                    isWallJumping = false;
            }

            if (!wallRunning)
            {
                // Handle gravity
                if (controller.isGrounded && !isWallJumping)
                {
                    verticalVelocity = -2f; // small downward force to stick to ground
                    if (Input.GetButtonDown("Jump"))
                    {
                        verticalVelocity = jumpPower;
                    }
                }
                else if (!isWallJumping)
                {
                    verticalVelocity -= gravity * Time.deltaTime;
                }

                // moveDirection.x and moveDirection.z = horizontal input * speed
                // vertical is verticalVelocity

                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 forward = transform.forward;
                Vector3 right = transform.right;

                Vector3 horizontalMove = forward * z + right * x;
                horizontalMove *= currentSpeed;

                moveDirection = horizontalMove;
                moveDirection.y = verticalVelocity;

                controller.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                if (wallRunning)
                {
                    WallRunMove();
                }
                else
                {
                    WalkMove();
                }
            }

            // Track movement state
            isMoving = (transform.position != lastPosition) && controller.isGrounded;
            lastPosition = transform.position;

            // Bullet time input and conditions:
            if (Input.GetKeyDown(KeyCode.E))
            {
                timeManager.DoNormalTime();
            }
            else if (controller.isGrounded)
            {
                timeManager.DoNormalTime();
            }
            else if (!controller.isGrounded && Input.GetMouseButtonDown(0))
            {
                timeManager.DoBulletTime();
            }
        }
    }

    void WalkMove()
    {
        // Run logic: update speed based on Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        // Get inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate horizontal movement relative to player orientation
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 horizontalMove = forward * z + right * x;
        horizontalMove *= currentSpeed;

        // Keep the current vertical velocity so gravity and jumping accumulate
        float verticalVelocity = moveDirection.y;

        // Jumping
        if (controller.isGrounded)
        {
            // If grounded, reset vertical velocity to a small negative to stick to ground
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
                verticalVelocity = jumpPower;
        }
        else
        {
            // Apply gravity if midair
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Update the moveDirection vector with horizontal and vertical velocities
        moveDirection = horizontalMove;
        moveDirection.y = verticalVelocity;

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    void WallRunMove()
    {
        // Apply gravity manually or limit vertical fall while wall running
        float verticalVelocity = moveDirection.y;
        verticalVelocity = 0f; // Lock vertical while wall running or adjust as desired

        // Horizontal movement along forward
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Move forward along wall with increased speed (can use runSpeed)
        Vector3 wallRunMove = forward * z + right * x;
        wallRunMove = wallRunMove.normalized * runSpeed;

        moveDirection = wallRunMove;
        moveDirection.y = verticalVelocity;

        controller.Move(moveDirection * Time.deltaTime);
    }

    // Method called from WallRunning.cs to apply a wall jump impulse
    public void ApplyWallJump(Vector3 jumpImpulse)
    {
        isWallJumping = true;
        wallJumpTimer = wallJumpDuration;

        verticalVelocity = jumpImpulse.y;

        // Apply sideways velocity immediately - using moveDirection.x and z
        moveDirection.x = jumpImpulse.x;
        moveDirection.z = jumpImpulse.z;
    }
}
