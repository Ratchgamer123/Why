using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PortalPhysicsObject : PortalTraveller 
{

    float timeElapsed;
    float lerpDuration = 1f;


    public float force = 10f;
    new Rigidbody rigidbody;

    void Awake () 
    {
        rigidbody = GetComponent<Rigidbody> ();
    }

    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) 
    {
        base.Teleport (fromPortal, toPortal, pos, rot);
        rigidbody.velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (rigidbody.velocity));
        rigidbody.angularVelocity = toPortal.TransformVector (fromPortal.InverseTransformVector (rigidbody.angularVelocity));

        //StartCoroutine(RotBack());
    }

    IEnumerator RotBack()
    {
        Quaternion currentRot = transform.rotation;
        Quaternion buffer;
        //rigidbody.freezeRotation = false;
        while (timeElapsed < lerpDuration)
        {
            buffer = Quaternion.LerpUnclamped(currentRot, Quaternion.identity, timeElapsed / lerpDuration);
            rigidbody.MoveRotation(buffer);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0;
        //rigidbody.freezeRotation = true;
    }
}