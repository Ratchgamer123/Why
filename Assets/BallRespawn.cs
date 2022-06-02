using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    private Vector3 firstPosition;
    private Rigidbody rb;

    void Start()
    {
        firstPosition = transform.position;
        TryGetComponent(out rb);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            rb.velocity = Vector3.zero;
            transform.position = firstPosition;
        }
    }
}
