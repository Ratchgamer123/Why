using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action GetPlayerLocation;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Respawn"))
        {
            GetPlayerLocation?.Invoke();
        }
    }

    private void OnEnable()
    {
        CheckpointSystem.SetPlayerTransform += SetPlayerTransform;
    }

    private void OnDisable()
    {
        CheckpointSystem.SetPlayerTransform -= SetPlayerTransform;
    }

    private void SetPlayerTransform(Transform playerTransform)
    {
        gameObject.transform.position = playerTransform.position;
        gameObject.transform.rotation = playerTransform.rotation;
    }
}
