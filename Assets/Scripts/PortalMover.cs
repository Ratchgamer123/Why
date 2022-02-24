using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMover : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private float duration;
    private void OnEnable()
    {
        Checkpoint.ShowNotification += MovePortal;
    }

    private void OnDisable()
    {
        Checkpoint.ShowNotification -= MovePortal;
    }

    private void MovePortal()
    {
        LeanTween.moveLocal(gameObject, destination.position, duration);
    }
}
