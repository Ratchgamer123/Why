using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float distance;
    public Transform origin;

    RaycastHit hit;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            FireRaycast();
        }
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
