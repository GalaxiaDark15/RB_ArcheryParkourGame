using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask Wall;
    public LayerMask Ground;
    public float wallRunForce = 5f;
    public float maxWallRunTime = 2f;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Direction")]
    public float wallCheckDistance = 0.6f;
    public float minJumpHeight = 1.5f;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private CharacterController controller;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        wallRunTimer = maxWallRunTime;
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

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && shiftPressed)
        {
            if (!pm.wallRunning)
                StartWallRun();
        }
        else
        {
            if (pm.wallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallRunning = true;
        wallRunTimer = maxWallRunTime;
    }

    private void StopWallRun()
    {
        pm.wallRunning = false;
        wallRunTimer = maxWallRunTime;
    }
}
