using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletTimeAlert;

    [SerializeField]
    private SFXManager soundFXManager;

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

    private float initialSpawnPointY = 0f;

    private float mouseY;

    private bool fire;

    private bool jumpTimerActive = false;
    public float jumpTimer = 0f;
    public float jumpTimerDuration = 0.4f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
        currentSpeed = walkSpeed; // default speed

        // Save initial spawn point Y coordinate
        initialSpawnPointY = transform.position.y;

        // Ensure alert is hidden at the start
        if (bulletTimeAlert != null)
            bulletTimeAlert.SetActive(false);
    }

    void Update()
    {
        if (isPaused) return;

        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0f)
                isWallJumping = false;
        }

        if (!wallRunning)
        {
            if (controller.isGrounded && !isWallJumping)
            {
                verticalVelocity = -2f; // small downward force to stick to ground
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = jumpPower;

                    // Start jump timer here
                    jumpTimerActive = true;
                    jumpTimer = jumpTimerDuration;

                    // Hide alert on new jump
                    if (bulletTimeAlert != null)
                        bulletTimeAlert.SetActive(false);
                }
            }
            else if (!isWallJumping)
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            WalkMove();
        }
        else
        {
            if (wallRunning)
            {
                WallRunMove();
            }
        }

        // Track movement state
        isMoving = (transform.position != lastPosition) && controller.isGrounded;
        lastPosition = transform.position;

        // Update jump timer logic
        if (jumpTimerActive)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                jumpTimerActive = false;
                // Only show alert if player is midair
                if (!controller.isGrounded && bulletTimeAlert != null)
                    bulletTimeAlert.SetActive(true);
            }
            else
            {
                // Always hide alert during the countdown
                if (bulletTimeAlert != null)
                    bulletTimeAlert.SetActive(false);
            }
        }

        // If player lands, hide alert (prevent leftover alert on landing)
        if (controller.isGrounded && bulletTimeAlert != null)
        {
            bulletTimeAlert.SetActive(false);
        }

        // Bullet time input and conditions:
        if (Input.GetKeyDown(KeyCode.E))
        {
            timeManager.DoNormalTime();
        }
        else if (controller.isGrounded)
        {
            timeManager.DoNormalTime();
        }
        else if (!controller.isGrounded && !jumpTimerActive && Input.GetMouseButtonDown(0))
        {
            // Only allow Bullet Time if the jump timer is finished and player is midair
            timeManager.DoBulletTime();

            // Hide the alert when bullet time is used
            if (bulletTimeAlert != null)
                bulletTimeAlert.SetActive(false);
        }
    }

    void WalkMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 horizontalMove = forward * z + right * x;
        horizontalMove *= currentSpeed;

        moveDirection = horizontalMove;
        moveDirection.y = verticalVelocity;

        controller.Move(moveDirection * Time.deltaTime);

        if (isMoving == true && Input.GetKey(KeyCode.LeftShift))
        {
            soundFXManager.playFastFootstepsSound();
        }
        else if (isMoving == true)
        {
            soundFXManager.playFootstepsSound();
        }
        else if (isMoving == false)
        {
            soundFXManager.stopFootstepsSound();
        }
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
