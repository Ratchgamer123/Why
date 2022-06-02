using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrierTrigger : MonoBehaviour
{
    public static event Action BallWentInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            BallWentInside?.Invoke();
        }
    }
}
