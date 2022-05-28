using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PortalPlayerObject : PortalTraveller
{
    float rotTimeElapsed;
    float rotLerpDuration = 1.0f;

    public Vector3 desiredScale;
    public Vector3 originalScale;

    public float force = 10f;
    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot, bool isRotated)
    {
        base.Teleport(fromPortal, toPortal, pos, rot);
        
        rigidbody.angularVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(rigidbody.angularVelocity));
        if(isRotated)
        {
            rigidbody.velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(rigidbody.velocity)) * force;
            StartCoroutine(RotBack());
        }
        else
        {
            rigidbody.velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(rigidbody.velocity));
        }
    }

    public override void EnterPortalThreshold(bool isRotated)
    {
        base.EnterPortalThreshold(isRotated);
        if (isRotated)
        {
            AvoidClipping(desiredScale);
        }
    }

    public override void ExitPortalThreshold(bool isRotated)
    {
        base.ExitPortalThreshold(isRotated);
        if(isRotated)
        {
            AvoidClipping(originalScale);
        }
    }

    private void AvoidClipping(Vector3 scale)
    {
        LeanTween.scale(gameObject, scale, 0.5f);
    }

    IEnumerator RotBack()
    {
        Quaternion currentRot = transform.rotation;
        Quaternion buffer;
        //rigidbody.freezeRotation = false;
        while (rotTimeElapsed < rotLerpDuration)
        {
            buffer = Quaternion.Slerp(currentRot, Quaternion.identity, rotTimeElapsed / rotLerpDuration);
            rigidbody.MoveRotation(buffer);
            rotTimeElapsed += Time.smoothDeltaTime;
            yield return null;
        }
        rotTimeElapsed = 0;
        //rigidbody.freezeRotation = true;
    }
}