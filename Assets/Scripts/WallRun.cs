using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WallRun : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    [Header("Wall Running")]
    [SerializeField] private float wallDistance = 0.5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity = 0.5f;
    [SerializeField] private float wallRunJumpForce = 6f;
    [SerializeField] private LayerMask walls;

    [Header("Camera Effects")]
    public Camera cam;
    public float fov = 90f;
    public float wallRunfov = 110f;
    public float wallRunfovTime = 20f;
    [SerializeField] private float camTilt = 20f;
    [SerializeField] private float camTiltTime = 20f;
    [SerializeField] private float chromAb = 0.1f;
    [SerializeField] private float wallRunChromAb = 0.2f;
    public Volume volume;

    public float tilt { get; private set; }

    ChromaticAberration chromatic;

    private Rigidbody rb;

    public bool isWallRunning = false;

    private bool wallLeft = false;
    private bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        volume.profile.TryGet(out chromatic);
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

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }

            AudioManager.instance.Play("WallRunJumpOff");
        }

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
}