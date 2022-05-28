using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public float distance;
    public Transform origin;

    RaycastHit hit;

    private void OnEnable()
    {
        PlayerMovement.InteractBindingPressed += FireRaycast;
    }

    private void OnDisable()
    {
        PlayerMovement.InteractBindingPressed -= FireRaycast;
    }

    private void FireRaycast()
    {
        if(Physics.Raycast(origin.position, origin.forward, out hit, distance))
        {
            if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}
