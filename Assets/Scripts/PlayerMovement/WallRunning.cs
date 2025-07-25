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

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && shiftPressed && !exitingWall)
        {
            // Only start wall run if timer not expired
            if (!pm.wallRunning && wallRunTimer > 0)
                StartWallRun();

            // Decrease wall run timer only while wall running and timer > 0
            if (pm.wallRunning && wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            // Once timer hits 0 while running, start exiting
            if (wallRunTimer <= 0 && pm.wallRunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
                Debug.Log("WallRun timer ended - entering exit state.");
            }

            // Wall jump
            if (Input.GetKeyDown(jumpKey))
                WallJump();
        }
        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallRunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
            {
                exitingWall = false;
                wallRunTimer = maxWallRunTime;  // Reset timer only after exit finishes
                Debug.Log("ExitWall timer ended - can wall run again.");
            }
        }
        // State 3 - None
        else
        {
            if (pm.wallRunning)
                StopWallRun();
        }
    }


    private void StartWallRun()
    {
        pm.wallRunning = true;
        wallRunTimer = maxWallRunTime;  // Reset timer only upon starting
    }

    private void StopWallRun()
    {
        pm.wallRunning = false;
        wallRunTimer = maxWallRunTime;  // Reset timer ready for next run
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

        StopWallRun();  // Stop wall running on jump
    }
}
