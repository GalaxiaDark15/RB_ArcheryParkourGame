using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask Wall;
    public LayerMask Ground;
    public float wallRunForce = 5f;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime = 0.7f;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance = 0.6f;
    public float minJumpHeight = 1.5f;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime = 0.2f;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private CharacterController controller;

    // Local state tracker for wallrunning
    private bool isWallRunning = false;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        wallRunTimer = maxWallRunTime;
        exitingWall = false;
        exitWallTimer = 0f;
    }

    void Update()
    {
        // Update input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check for walls
        CheckForWall();

        // Update wall running state machine
        StateMachine();
    }

    private void CheckForWall()
    {
        // Wall Detection -- ADD WALL LAYERS TO WALL OBJECTS
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, Wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, Wall);
    }

    private bool AboveGround()
    {
        // Check if player is not too close to ground to allow wall run.
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, Ground);
    }

    private void StateMachine()
    {
         // Activate wall running only when pressing shift and conditions met
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool canWallRun = (wallLeft || wallRight) && verticalInput > 0 && AboveGround() && shiftPressed && !exitingWall;

        if (canWallRun)
        {
            if (!isWallRunning)
            {
                StartWallRun();
                isWallRunning = true;
                pm.wallRunning = true; // Update external reference
            }

            if (wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            else
            {
                // Wall run time expired, enter exit state
                exitingWall = true;
                exitWallTimer = exitWallTime;
                Debug.Log("WallRun timer ended - entering exit state.");
            }

            // Wall jump input
            if (Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        }
        else if (exitingWall)
        {
            if (isWallRunning)
            {
                StopWallRun();
                isWallRunning = false;
                pm.wallRunning = false;
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer <= 0)
            {
                exitingWall = false;
                wallRunTimer = maxWallRunTime;
                Debug.Log("ExitWall timer ended - can wall run again.");
            }
        }
        else
        {
            // Not wallrunning or exiting, ensure wallrunning is stopped and timer resets
            if (isWallRunning)
            {
                StopWallRun();
                isWallRunning = false;
                pm.wallRunning = false;
            }

            // Reset timer so wallrun duration is restored for next run
            wallRunTimer = maxWallRunTime;
        }
    }

    private void StartWallRun()
    {
        wallRunTimer = maxWallRunTime; 
        pm.wallRunning = true; 
        Debug.Log("Start Wallrun");
    }

    private void StopWallRun()
    {
        pm.wallRunning = false;
        Debug.Log("Stop Wallrun");
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 jumpUpward = Vector3.up * wallJumpUpForce;
        Vector3 jumpSideways = wallNormal * wallJumpSideForce;

        Vector3 jumpDirection = jumpUpward + jumpSideways;
        pm.ApplyWallJump(jumpDirection);

        StopWallRun();
        isWallRunning = false;
        pm.wallRunning = false;
    }
}
