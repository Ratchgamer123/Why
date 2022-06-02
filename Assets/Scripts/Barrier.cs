using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private int barrierHealth = 2;
    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject barrier;

    private void OnEnable()
    {
        BarrierTrigger.BallWentInside += DecreaseBarrierHealth;
    }

    private void OnDisable()
    {
        BarrierTrigger.BallWentInside -= DecreaseBarrierHealth;
    }

    private void DecreaseBarrierHealth()
    {
        barrierHealth--;

        if (barrierHealth <= 0)
        {
            DeactivateBarrier();
        }
    }

    private void DeactivateBarrier()
    {
        barrier.SetActive(false);
        source.Play();
    }
}
