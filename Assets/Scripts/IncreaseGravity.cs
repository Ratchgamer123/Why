using UnityEngine;

public class IncreaseGravity : MonoBehaviour
{
    [SerializeField] private float gravity = 1.0f;
    private Rigidbody rb;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Metal"))
        {
            audioSource.Play();
        }
    }
}
