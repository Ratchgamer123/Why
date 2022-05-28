using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Orientation")]
    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float movementMultiplier = 10.0f;
    [SerializeField] private float wallRunningMultiplier = 12.0f;
    [SerializeField] private float airMultiplier = 0.5f;
    [SerializeField] private float gravity = 3.0f;

    [Header("Camera")]
    public float sensX = 50.0f;
    public float sensY = 50.0f;
    [SerializeField] private Transform camPos;
    [SerializeField] private Camera cam;
    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    public Quaternion TargetRotation { private set; get; }

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4.0f;
    [SerializeField] float sprintSpeed = 6.0f;
    [SerializeField] float acceleration = 10.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 27.0f;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6.0f;
    [SerializeField] private float airDrag = 2.0f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    float groundDistance = 0.4f;

    [Header("Wall Running")]
    [SerializeField] private float wallDistance = 0.6f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private float wallRunGravity = 0.2f;
    [SerializeField] private float wallRunJumpForce = 15.0f;
    [SerializeField] private LayerMask walls;

    [Header("Camera Effects")]
    public float fov = 90.0f;
    public float wallRunfov = 110.0f;
    public float wallRunfovTime = 20.0f;
    [SerializeField] private float camTilt = 15.0f;
    [SerializeField] private float camTiltTime = 25.0f;
    [SerializeField] private float chromAb = 0.1f;
    [SerializeField] private float wallRunChromAb = 1.0f;
    public Volume volume;

    public float tilt { get; private set; }

    ChromaticAberration chromatic;

    public bool isWallRunning = false;

    private bool wallLeft = false;
    private bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    Vector2 walkInput;
    Vector2 camRotInput;

    bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    private MainControls controls;

    #region Exterior Events
    public static event Action InteractBindingPressed;
    public static event Action PauseMenuBindingPressed;
    #endregion

    private void Awake()
    {
        controls = new MainControls();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        volume.profile.TryGet(out chromatic);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TargetRotation = transform.rotation;
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Main.Jumping.performed += Jump;
        controls.Main.Interact.performed += InteractBindingEventCall;
        controls.Main.PauseMenu.performed += PauseMenuBindingEventCall;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Main.Jumping.performed -= Jump;
        controls.Main.Interact.performed -= InteractBindingEventCall;
        controls.Main.PauseMenu.performed -= PauseMenuBindingEventCall;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();

        ControlDrag();
        ControlSpeed();

        CheckWall();
        if (AbleToWallRun())
        {
            if (wallLeft || wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        Debug.Log(controls.Main.CamRot.ReadValue<Vector2>());
    }

    private void MyInput()
    {
        walkInput = controls.Main.WalkDirection.ReadValue<Vector2>();
        horizontalMovement = walkInput.x;
        verticalMovement = walkInput.y;

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        camRotInput = controls.Main.CamRot.ReadValue<Vector2>();

        mouseX = camRotInput.x;
        mouseY = camRotInput.y;

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camPos.transform.localRotation = Quaternion.Euler(xRotation, yRotation, tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void InteractBindingEventCall(InputAction.CallbackContext ctx)
    {
        InteractBindingPressed?.Invoke();
    }

    private void PauseMenuBindingEventCall(InputAction.CallbackContext ctx)
    {
        PauseMenuBindingPressed?.Invoke();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }

        WallJump();
    }

    private void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (!isWallRunning)
        {
            rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isWallRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * wallRunningMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private void ControlSpeed()
    {
        if (controls.Main.Sprinting.ReadValue<float>() == 1.0f && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        }
    }

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }



    private bool AbleToWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, walls);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, walls);
    }

    private void StartWallRun()
    {
        rb.useGravity = false;

        chromatic.intensity.value = wallRunChromAb;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        chromatic.intensity.value = wallRunChromAb;

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if (!isWallRunning) { AudioManager.instance.Play("OnWallRun"); }

        isWallRunning = true;
    }

    private void StopWallRun()
    {
        rb.useGravity = true;

        AudioManager.instance.StopAudio("OnWallRun");

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);

        chromatic.intensity.value = Mathf.Lerp(chromatic.intensity.value, chromAb, camTiltTime * Time.deltaTime);

        isWallRunning = false;
    }

    private void WallJump()
    {
        if(AbleToWallRun())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                AudioManager.instance.Play("WallRunJumpOff");
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                AudioManager.instance.Play("WallRunJumpOff");
            }
        }
    }
}