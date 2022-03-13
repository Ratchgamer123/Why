using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float movementMultiplier = 10.0f;
    [SerializeField] private float wallRunningMultiplier = 12.0f;
    [SerializeField] private float airMultiplier = 0.5f;
    [SerializeField] private float gravity = 3.0f;

    [Header("Camera")]
    public float sensX = 50.0f;
    public float sensY = 50.0f;
    [SerializeField] Transform cam;
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

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6.0f;
    [SerializeField] private float airDrag = 2.0f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    float groundDistance = 0.4f;

    bool isGrounded;

    WallRun wallRun;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("playerSensX") || !PlayerPrefs.HasKey("playerSensY"))
        {
            PlayerPrefs.SetFloat("playerSensX", 50.0f);
            PlayerPrefs.SetFloat("playerSensY", 50.0f);
        }

        sensX = PlayerPrefs.GetFloat("playerSensX");
        sensY = PlayerPrefs.GetFloat("playerSensY");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        wallRun = GetComponent<WallRun>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TargetRotation = transform.rotation;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        MyInput();

        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
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
        if(!wallRun.isWallRunning)
        {
            rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (wallRun.isWallRunning)
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
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            wallRun.cam.fieldOfView = Mathf.Lerp(wallRun.cam.fieldOfView, wallRun.wallRunfov, wallRun.wallRunfovTime * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            Mathf.Lerp(wallRun.cam.fieldOfView, wallRun.fov, wallRun.wallRunfovTime * Time.deltaTime);
        }
    }

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
        {
            if(slopeHit.normal != Vector3.up)
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
}
